using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    PlayerMovementTutorial playerObject;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;
    public Transform projectileSpawn;
    public float cooldown = 10f;
    public GameObject enemyBullet;
    private float bulletTime;
    public float bulletSpeed;


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Enemy type
    public int enemyID;
    public bool countable;
    public bool isBoss;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        transform.LookAt(player);



        if (!alreadyAttacked)
        {
            ///Attack code here
            if (enemyID == 1)
            {

                ShootAtPlayer();
                //Instantiate(projectile, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
            ///End of attack code


        }
        if (enemyID == 0)
        {
            //transform.LookAt(player);
            agent.SetDestination(player.position);
            if (!playerInSightRange)
            {
                Patroling();
            }
        }
    }

    void ShootAtPlayer()
    {
        GameObject bulletObj = Instantiate(enemyBullet, projectileSpawn.transform.position, projectileSpawn.transform.rotation) as GameObject;
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * bulletSpeed);
        Destroy(bulletObj, 5f);
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        PlayerMovementTutorial player = gameObject.GetComponent<PlayerMovementTutorial>();
        if (countable == true)
        {
            player.enemiesKilled += 1;
            player.enemiesKilledText.text = "Enemies Killed: " + player.enemiesKilled + "/11";
        }
        if (isBoss == true)
        {
            player.bossBoarKilled = true;
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovementTutorial player = collision.gameObject.GetComponent<PlayerMovementTutorial>();
        if (collision.gameObject.tag == "Player")
        {
            player.UpdateHealth(-1);
        }
    }
}
