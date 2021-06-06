using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace PinkRain
{
    public class Gun : MonoBehaviour
    {
        private const float BulletSpeed = 15;
        private const int ClipSize = 16;
        private const float FireRate = 20;
        private const float ReloadTime = 0.5f;

        [SerializeField] private GameObject bulletPrefab;

        private int ammo = ClipSize;
        private Hand hand;
        [CanBeNull] private Coroutine reloading;
        private bool shooting;

        private enum Hand
        {
            Left,
            Right
        }

        private void Update()
        {
            if (ammo > 0 && !shooting && Input.GetMouseButton(0))
            {
                if (!(reloading is null))
                {
                    StopCoroutine(reloading);
                }

                StartCoroutine(Shoot());
                reloading = StartCoroutine(Reload());
            }
        }

        private IEnumerator Shoot()
        {
            shooting = true;
            ammo--;
            SpawnBullet();
            yield return new WaitForSeconds(1 / FireRate);
            hand = hand == Hand.Left ? Hand.Right : Hand.Left;
            shooting = false;
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(ReloadTime);
            ammo = ClipSize;
        }

        private void SpawnBullet()
        {
            var position = transform.position;
            var target = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            var direction = ((Vector2) target - (Vector2) position).normalized;
            var rotation = Quaternion.FromToRotation(Vector3.right, direction);
            var offset = rotation * ((hand == Hand.Left ? Vector3.up : Vector3.down) / 3);
            var bullet = Instantiate(bulletPrefab, position + offset, rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = BulletSpeed * direction;
        }
    }
}