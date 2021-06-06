using System.Collections;
using UnityEngine;

namespace PinkRain
{
    public class Gun : MonoBehaviour
    {
        private const float BulletSpeed = 15;
        private const int ClipSize = 100;
        private const float FireRate = 20;
        private const float ReloadTime = 1;

        [SerializeField] private GameObject bulletPrefab;

        private int ammo = ClipSize;
        private bool shooting;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                StartCoroutine(Shoot());
            }
        }

        private IEnumerator Shoot()
        {
            if (ammo <= 0 || shooting)
            {
                yield break;
            }

            var position = transform.position;
            var target = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            var direction = ((Vector2) target - (Vector2) position).normalized;
            var rotation = Quaternion.FromToRotation(Vector3.right, direction);

            var bullet = Instantiate(bulletPrefab, position, rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = BulletSpeed * direction;

            ammo--;
            yield return Ready();
        }

        private IEnumerator Ready()
        {
            shooting = true;
            if (ammo <= 0)
            {
                yield return new WaitForSeconds(ReloadTime);
                ammo = ClipSize;
            }
            else
            {
                yield return new WaitForSeconds(1 / FireRate);
            }

            shooting = false;
        }
    }
}
