using UnityEngine;
using UnityEngine.UI;

namespace PinkRain.Component
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;

        public Slider? uiSlider;

        public void Start()
        {
            currentHealth = maxHealth;
        }

        public void Update()
        {
            if (uiSlider != null)
            {
                uiSlider.value = currentHealth / maxHealth;
            }
            if (currentHealth < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}