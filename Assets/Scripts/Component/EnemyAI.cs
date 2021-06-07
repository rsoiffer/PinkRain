using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject? player;
    public GameObject? bulletPrefab;

    public float bulletSpeed = 20;
    public float shotCooldown = 1;
    public bool shooting;

    public float maxAlertTime = 5;

    public float currentAlertTime;
    private Vector2 lastKnownPlayerPos;

    private NavMeshAgent? myNavMeshAgent;

    private void Start()
    {
        player = GameObject.Find("Player");

        myNavMeshAgent = GetComponent<NavMeshAgent>();
        myNavMeshAgent.enabled = true;
        myNavMeshAgent.updateRotation = false;
        myNavMeshAgent.updateUpAxis = false;
    }

    void Update()
    {
        if (player != null)
        {
            var toTarget = player!.transform.position - transform.position;
            if (!Physics2D.Raycast(transform.position, toTarget,
                toTarget.magnitude, 1 << 6))
            {
                currentAlertTime = maxAlertTime;
                lastKnownPlayerPos = player!.transform.position;
                myNavMeshAgent!.SetDestination(transform.position);

                if (!shooting)
                {
                    StartCoroutine(Shoot(toTarget));
                }
            }
            else if (currentAlertTime > 0)
            {
                myNavMeshAgent!.SetDestination(lastKnownPlayerPos);
            }
        }

        currentAlertTime -= Time.deltaTime;
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