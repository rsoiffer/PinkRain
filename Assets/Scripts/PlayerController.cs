using UnityEngine;

namespace PinkRain
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;

        private new Rigidbody2D rigidbody2D;

        private void Start() => rigidbody2D = GetComponent<Rigidbody2D>();

        private void Update()
        {
            Move();
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        private void Move()
        {
            const float speed = 5;

            var x = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            var y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
            var direction = new Vector2(x, y).normalized;
            rigidbody2D.velocity = speed * direction;
        }

        private void Shoot()
        {
            const float speed = 10;

            var position = transform.position;
            var target = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            var direction = ((Vector2)target - (Vector2)position).normalized;
            var rotation = Quaternion.FromToRotation(Vector3.right, direction);

            var bullet = Instantiate(bulletPrefab, position, rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = speed * direction;
        }
    }
}
