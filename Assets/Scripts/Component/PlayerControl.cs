using PinkRain.Utility;
using UnityEngine;

namespace PinkRain.Component
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float speed = 8;

        private new Rigidbody2D? rigidbody2D;

        private void Start() => rigidbody2D = GetComponent<Rigidbody2D>();

        private void Update() => Move();

        private void Move()
        {
            Requires.NotNull(rigidbody2D, nameof(rigidbody2D));

            var x = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            var y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
            var direction = new Vector2(x, y).normalized;
            rigidbody2D.velocity = speed * direction;
        }
    }
}