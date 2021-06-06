using UnityEngine;

namespace PinkRain.Component
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;

        public void Start()
        {
            currentHealth = maxHealth;
        }

        public void Update()
        {
            if (currentHealth < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}