using UnityEngine;

namespace AudioManager
{
    public class InGameMusic : MonoBehaviour
    {
        public void Start()
        {
            AudioManager.Instance.PlayGameMusic();
        }
    }
}