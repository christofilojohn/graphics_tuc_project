using UnityEngine;
using UnityEngine.UI;

namespace Level3.scripts
{
    public class HealthBarManager : MonoBehaviour
    {
        [SerializeField] private Image[] healthSegments;
        [SerializeField] private Image meatBooster;
        [SerializeField] private Canvas rottenMeatCanvas;
        
        private const int MAX_HEALTH = 3;
        private int _currentHealth;

        void Start()
        {
            HideMeatBooster();
            HideRottenMeatCanvas();
            _currentHealth = healthSegments.Length;
            UpdateHealthBar();
        }

        public void TakeDamage()
        {
            _currentHealth -= 1;
            UpdateHealthBar();
        }

        public void Heal()
        {
            _currentHealth = Mathf.Min(_currentHealth+1, MAX_HEALTH);
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            for (int i = 0; i < healthSegments.Length; i++)
            {
                healthSegments[i].enabled = i < _currentHealth;
            }
        }

        public void HideMeatBooster()
        {
            if (meatBooster.gameObject.activeSelf)
                meatBooster.gameObject.SetActive(false);
        }
        

        public void ShowMeatBooster()
        {
            meatBooster.gameObject.SetActive(true);
        }
        
        public void HideRottenMeatCanvas()
        {
            if (rottenMeatCanvas.gameObject.activeSelf)
                rottenMeatCanvas.gameObject.SetActive(false);
        }
        

        public void ShowRottenMeatCanvas()
        {
            rottenMeatCanvas.gameObject.SetActive(true);
        }

        public bool Alive()
        {
            return _currentHealth > 0;
        }
    }
}