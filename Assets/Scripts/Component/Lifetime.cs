using UnityEngine;

namespace PinkRain.Component
{
    public class Lifetime : MonoBehaviour
    {
        [SerializeField] private float lifetime;

        private void Start() => Destroy(gameObject, lifetime);
    }
}