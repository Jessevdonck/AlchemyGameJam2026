using UnityEngine;

namespace AudioManager
{
    public class MainMenuMusic : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.Instance.PlayMenuMusic();
        }
    }
}