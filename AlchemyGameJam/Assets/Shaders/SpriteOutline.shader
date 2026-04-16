Shader "Custom/SpriteOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineSize ("Outline Size", Range(0, 10)) = 2
        _OutlineEnabled ("Outline Enabled", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineSize;
            float _OutlineEnabled;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;

                if (_OutlineEnabled > 0.5)
                {
                    float2 texelSize = _MainTex_TexelSize.xy * _OutlineSize;

                    // Sample neighbouring pixels
                    float upAlpha    = tex2D(_MainTex, IN.texcoord + float2(0, texelSize.y)).a;
                    float downAlpha  = tex2D(_MainTex, IN.texcoord - float2(0, texelSize.y)).a;
                    float rightAlpha = tex2D(_MainTex, IN.texcoord + float2(texelSize.x, 0)).a;
                    float leftAlpha  = tex2D(_MainTex, IN.texcoord - float2(texelSize.x, 0)).a;

                    float maxNeighbour = max(max(upAlpha, downAlpha), max(rightAlpha, leftAlpha));

                    // If current pixel is transparent but a neighbour is not — draw outline
                    if (c.a < 0.1 && maxNeighbour > 0.1)
                    {
                        c = _OutlineColor;
                        c.rgb *= c.a;
                        return c;
                    }
                }

                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}
