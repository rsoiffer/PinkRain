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
            var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            var direction = (Camera.main!.ScreenToWorldPoint(Input.mousePosition) - position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = speed * direction;
        }
    }
}
