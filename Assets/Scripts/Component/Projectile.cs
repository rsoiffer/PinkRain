using UnityEngine;

namespace PinkRain.Component
{
    public class Projectile : MonoBehaviour
    {
        public float damage = 1;

        public void OnCollisionEnter2D(Collision2D other)
        {
            var otherHealth = other.gameObject.GetComponentInParent<Health>();
            if (otherHealth != null)
            {
                otherHealth.currentHealth -= damage;
            }

            Destroy(gameObject);
        }
    }
}