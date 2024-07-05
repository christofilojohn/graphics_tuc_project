using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Level3.scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UImanager uiManager;
        [SerializeField] private Player player;
        [SerializeField] private NPC npcPawn;
        [SerializeField] private NPC npcQueen;
        [SerializeField] private NPC npcShopkeeper;
        [SerializeField] private NPC npcViking;
        
        private bool enemyAlivePawn;
        private bool enemyAliveQueen;
        private bool enemyAliveShopkeeper;
        private bool enemyAliveViking;
        private bool playerAlive;
        public bool isPaused;
        
        void Start()
        {
            Screen.SetResolution(1920, 1080, true);
        }
        
        // Update is called once per frame
        void Update() {
            CheckPause();
            playerAlive = player.Alive();
            enemyAlivePawn = npcPawn.Alive();
            enemyAliveQueen = npcQueen.Alive();
            enemyAliveShopkeeper = npcShopkeeper.Alive();
            enemyAliveViking = npcViking.Alive();
            
            if (!playerAlive)
            {
                GameOver();
            }
            if (!enemyAlivePawn && !enemyAliveQueen && !enemyAliveShopkeeper && !enemyAliveViking)
            {
                GameWin();    
            }
        }

        private void GameWin()
        {
            uiManager.FadeToWhite("You win !");
        }

        public void GameOver()
        {
            uiManager.FadeToBlack("Game Over...");
        }
        
        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        public void CheckPause() {
            if (!isPaused && Input.GetKeyDown(KeyCode.Escape)) {
                isPaused = true;
                uiManager.ShowPauseMenu();
            } else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = false;
                uiManager.HidePauseMenu();
            }
        }

        public void Resume() {
            if (isPaused) {
                isPaused = false;
                uiManager.HidePauseMenu();
            }
        }
    }

}