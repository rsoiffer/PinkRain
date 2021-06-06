using UnityEngine;

namespace PinkRain
{
    public class PlayerController : MonoBehaviour
    {
        private new Rigidbody2D rigidbody2D;

        private void Start() => rigidbody2D = GetComponent<Rigidbody2D>();

        private void Update()
        {
            var direction =
                new Vector2(
                    Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0,
                    Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0)
                .normalized;

            const float speed = 5;
            rigidbody2D.velocity = speed * direction;
        }
    }
}
