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
                ShootThenReload();
            }
        }

        private void ShootThenReload()
        {
            if (!(reloading is null))
            {
                StopCoroutine(reloading);
            }

            StartCoroutine(Shoot());
            reloading = StartCoroutine(Reload());
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

            var position = transform.position;
            var target = camera.ScreenToWorldPoint(Input.mousePosition);
            var rotation = AimRotation(position, target);

            var handPosition = position + rotation * (hand == Hand.Left ? Vector3.up : Vector3.down) / 3;
            var inaccuracy = Quaternion.AngleAxis(Spread * (Random.value - 0.5f), Vector3.forward);
            var handRotation = inaccuracy * AimRotation(handPosition, target);

            var bullet = Instantiate(bulletPrefab, handPosition, handRotation);
            bullet.GetComponent<Rigidbody2D>().velocity = handRotation * Vector3.right * BulletSpeed;
        }

        private static Quaternion AimRotation(Vector2 origin, Vector2 target)
        {
            var direction = (target - origin).normalized;
            var angle = Vector2.SignedAngle(Vector2.right, direction);
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}