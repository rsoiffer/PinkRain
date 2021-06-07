using UnityEngine;

namespace PinkRain.Component
{
    public class Projectile : MonoBehaviour
    {
        public float damage = 1;

        private Rigidbody2D? myRigidbody2d;

        public void Start()
        {
            myRigidbody2d = GetComponent<Rigidbody2D>();
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            var otherHealth = other.gameObject.GetComponentInParent<Health>();
            if (otherHealth != null)
            {
                otherHealth.Damage(damage, -other.relativeVelocity);
            }

            Destroy(gameObject);
        }
    }
}