using UnityEngine;

namespace PinkRain.Component
{
    public class WinLevel : MonoBehaviour
    {
        public Procgen? procgen;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                procgen!.Generate();
            }
        }
    }
}