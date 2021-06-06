using System.Collections;
using PinkRain.Utility;
using UnityEngine;

namespace PinkRain.Component
{
    public class Gun : MonoBehaviour
    {
        private const float BulletSpeed = 30;
        private const int ClipSize = 16;
        private const float FireRate = 20;
        private const float ReloadTime = 0.5f;
        private const float Spread = 5;

        [SerializeField] private GameObject? bulletPrefab;

        private int ammo = ClipSize;
        private new Camera? camera;
        private Hand hand;
        private Coroutine? reloading;
        private bool shooting;

        private enum Hand
        {
            Left,
            Right
        }

        private void Start() => camera = Camera.main;

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
            Requires.NotNull(bulletPrefab, nameof(bulletPrefab));
            Requires.NotNull(camera, nameof(camera));

            var position = transform.position + (hand == Hand.Left ? Vector3.up : Vector3.down) / 3;
            var target = camera.ScreenToWorldPoint(Input.mousePosition);

            var targetDirection = ((Vector2) target - (Vector2) position).normalized;
            var angle = Vector2.SignedAngle(Vector2.right, targetDirection) + Spread * (Random.value - 0.5f);
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var direction = rotation * Vector3.right;

            var bullet = Instantiate(bulletPrefab, position, rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * BulletSpeed;
        }
    }
}