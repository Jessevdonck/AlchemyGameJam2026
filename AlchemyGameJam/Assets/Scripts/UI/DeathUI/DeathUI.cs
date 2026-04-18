using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.DeathUI
{
    public class DeathUI : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        private void Start()
        {
            var player = FindObjectOfType<PlayerStats>();
            player.OnDied += Show;
        
            root.SetActive(false);
        }

        private void Show()
        {
            root.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Respawn()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("SampleScene");
        }
    }
}