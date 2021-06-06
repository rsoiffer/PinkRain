using System.Collections;
using UnityEngine;

namespace PinkRain
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;

        private bool cycling;

        private void Update()
        {
            if (!cycling && Input.GetMouseButton(0))
            {
                StartCoroutine(Shoot());
            }
        }

        private IEnumerator Shoot()
        {
            const float speed = 15;
            const float fireRate = 20;

            var position = transform.position;
            var target = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            var direction = ((Vector2) target - (Vector2) position).normalized;
            var rotation = Quaternion.FromToRotation(Vector3.right, direction);

            var bullet = Instantiate(bulletPrefab, position, rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = speed * direction;

            cycling = true;
            yield return new WaitForSeconds(1 / fireRate);
            cycling = false;
        }
    }
}
