using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject? player;
    public GameObject? bulletPrefab;

    public float walkForce = 10;
    public float walkDamping = 10;

    public float bulletSpeed = 20;
    public float shotCooldown = 1;
    public bool shooting;

    private Rigidbody2D? myRigidbody2D;

    private void Start()
    {
        player = GameObject.Find("Player");
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;
        var toTarget = player!.transform.position - transform.position;
        if (!Physics2D.Raycast(transform.position, toTarget,
            toTarget.magnitude, 1 << 6))
        {
            myRigidbody2D!.AddForce(walkForce * toTarget.normalized);
            if (!shooting)
            {
                StartCoroutine(Shoot(toTarget));
            }
        }

        myRigidbody2D!.AddForce(-walkDamping * myRigidbody2D.velocity);
    }

    IEnumerator Shoot(Vector3 toTarget)
    {
        shooting = true;
        var bullet = Instantiate(bulletPrefab!);
        bullet.transform.SetPositionAndRotation(
            transform.position,
            Quaternion.FromToRotation(Vector2.right, toTarget));
        bullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * toTarget.normalized;

        yield return new WaitForSeconds(shotCooldown);
        shooting = false;
    }
}