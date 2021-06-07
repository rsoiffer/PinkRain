using UnityEngine;
using UnityEngine.UI;

namespace PinkRain.Component
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        public Slider? uiSlider;
        public GameObject? damageEffectPrefab;

        private float currentHealth;

        public void Start()
        {
            currentHealth = maxHealth;
        }

        public void Damage(float amt, Vector2 direction)
        {
            currentHealth -= amt;
            if (damageEffectPrefab != null)
            {
                var damageEffect = Instantiate(damageEffectPrefab);
                damageEffect.transform.SetPositionAndRotation(transform.position,
                    Quaternion.FromToRotation(Vector2.right, direction));
            }
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