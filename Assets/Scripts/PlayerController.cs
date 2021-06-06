using UnityEngine;

namespace PinkRain
{
    public class PlayerController : MonoBehaviour
    {
        private new Rigidbody2D rigidbody2D;

        private void Start() => rigidbody2D = GetComponent<Rigidbody2D>();

        private void Update() => Move();

        private void Move()
        {
            const float speed = 5;

            var x = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            var y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
            var direction = new Vector2(x, y).normalized;
            rigidbody2D.velocity = speed * direction;
        }
    }
}