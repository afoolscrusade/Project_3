using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class ThirdPersonMovement : MonoBehaviour
{

    
    
    public CharacterController controller;
    public Transform cam;
    public Camera camera;

    // Player Movement
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    float horizontalInput;
    float verticalInput;

    // Charge Shot
    [SerializeField]
    private GameObject chargedFireball;
    public float chargeSpeed;
    public float chargeTime;
    bool isCharging;

    //jump
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    public bool isGrounded;
    public bool isJump;

    //player Attack
    public bool canAttack;
    public float AttackCooldown = 0.05f;
    public float attackCooldownNormal = 3f;
    public GameObject projectile;
    public float projectileSpeed;
    public Transform projectileSpawn;
    public bool isAttacking;
    public bool isStrongAttacking;

    // Player Health
    public float maxHealth;
    public float currentHealth;
    public float invincibleTimer;
    public float timeInvincible = 2.0f;
    bool isInvincible;

    // Potions
    public static int HealthP = 1;
    public static int ManaP = 1;
    public TextMeshProUGUI HealthPText;

    public TextMeshProUGUI ManaPText;

    public TextMeshProUGUI crystalCountText;

    public TextMeshProUGUI enemiesKilledText;

    public float maxMana;
    public float currentMana;
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public Animator animator;
    bool isWalking;

    public int enemiesKilled;
    public bool bossBoarKilled;

    public float crystalsCollected;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //Audio
    public AudioSource audioSource;
    public AudioSource levelFootsteps;
    public AudioClip potionDrink;
    public AudioClip fireballSound;

    // Saving
    public int levelSaved;
    private void Awake()
    {
        maxHealth = 10;
        maxMana = 10;
        currentHealth = maxHealth;
        currentMana = maxMana;
        enemiesKilled = 0;
        bossBoarKilled = false;
        crystalsCollected = 0;
        canAttack = true;

        //Potions
        SetCurrentHP();
        SetCurrentMP();

        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);
        }
        // Starts charging fireball
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetButtonDown("Fire1") && chargeTime < 3)
        {
            isCharging = true;
            if (isCharging == true)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
            }
        }

        //fireball attack
        if (Input.GetButtonDown("Fire1") && currentMana > 0 && canAttack == true)
        {
            FireballAttack();
            chargeTime = 0;
            UpdateMana(-1);
        }
        else if (Input.GetButtonUp("Fire1") && chargeTime >= 2 && currentMana > 4 && canAttack == true)
        {
            ReleaseCharge();
            UpdateMana(-5);
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }    
        }
        //Testing Laser
        /*if(Input.GetButtonDown("Fire2"))
        {
            LaserAttack();
        }*/

        //NPC interaction
        /*if (Input.GetKeyDown("x"))
        {
            Debug.Log("NPC");
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent(out NPC_Script NPC_Script))
                {
                    NPC_Script.Interact();
                }
        }*/
        //Potions use
        if ((Input.GetKeyDown(KeyCode.Y) || Input.GetButtonDown("Usehealth")) && HealthP > 0 && currentHealth < maxHealth)
        {
            audioSource.PlayOneShot(potionDrink);
            HealthP -= 1;
            SetCurrentHP();
            UpdateHealth(+5);
        }

        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Usemana")) && ManaP > 0 && currentMana < maxMana)
        {
            audioSource.PlayOneShot(potionDrink);
            ManaP -= 1;
            SetCurrentMP();
            UpdateMana(+5);
        }

        // Animations

        if (horizontalInput != 0 || verticalInput != 0)
        {
            animator.SetBool("isWalking", true);
            levelFootsteps.Play();
        }
        if (horizontalInput == 0 && verticalInput == 0)
        {
            animator.SetBool("isWalking", false);
            levelFootsteps.Stop();
        }

        /*if (isGrounded == false && isJump == true)
        {
            animator.SetTrigger("jumping");
        }
        else if (isGrounded == true)
        {
            animator.SetBool("isJumping", false);
        }*/
        if (isAttacking == true)
        {
            animator.SetBool("isAttacking", true);
        }
        else if (isAttacking == false)
        {
            animator.SetBool("isAttacking", false);
        }

        if (enemiesKilled >= 10 && bossBoarKilled == true)
        {
            SceneManager.LoadScene("MainHub");
        }

        if (crystalsCollected == 4)
        {
            SceneManager.LoadScene("MainHub");
        }
    }

    void Jump()
    {

        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //isJump = true;
            animator.SetTrigger("jumping");
        }

        else if (isJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJump = false;
            animator.SetTrigger("jumping");
        }

    }
    public void FireballAttack()
    {

        //Rigidbody rb = fireball.GetComponent<Rigidbody>();

        //rb.velocity = Camera.main.transform.forward * projectileSpeed;

        //new fireball shooting
        animator.SetTrigger("attack");
        audioSource.PlayOneShot(fireballSound);
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 direction = targetPoint - projectileSpawn.position;

        GameObject currentFireball = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);

        currentFireball.transform.forward = direction.normalized;

        currentFireball.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);



        StartCoroutine(ResetAttackCooldown());
        isAttacking = false;
    }

    void ReleaseCharge()
    {
        //Instantiate(chargedFireball, projectileSpawn);
        //rigidbody rb2 = chargedFireball.GetComponent<Rigidbody>();

        //rb.velocity = Camera.main.transform.forward * projectileSpeed;

        //new fireballattack
        animator.SetTrigger("strongAttack");
        audioSource.PlayOneShot(fireballSound);
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 direction = targetPoint - projectileSpawn.position;

        GameObject currentFireball = Instantiate(chargedFireball, projectileSpawn.position, Quaternion.identity);

        currentFireball.transform.forward = direction.normalized;

        currentFireball.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);
        isCharging = false;
        chargeTime = 0;

    }

    public void UpdateHealth(float amount) // receives value and changes health accordingly
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth <= 0)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(PlayerDeathAnimation());
            Debug.Log("Player should be dead");
            SceneManager.LoadScene("GameOver");
        }
    }

    // Mana
    public void UpdateMana(float amount) // receives value and changes Mana accordingly
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        if (currentMana < maxMana)
        {
            if (regen != null)

                StopCoroutine(regen);

            regen = StartCoroutine(RegenMana());

        }

    }

    IEnumerator ResetAttackCooldown()
    {

        yield return new WaitForSeconds(AttackCooldown);

    }

    IEnumerator PlayerDeathAnimation()
    {
        yield return new WaitForSeconds(5);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Touched enemy");
        }
    }
    void OnTriggerEnter(Collider other)
    {

        //Send player to different scenes
        if (other.CompareTag("LevelOne"))
        {
            SceneManager.LoadScene("Level1");
        }

        if (other.CompareTag("LevelTwo"))
        {
            SceneManager.LoadScene("Level2");
        }

        if (other.CompareTag("LevelThree"))
        {
            SceneManager.LoadScene("Level3");
        }

        if (other.CompareTag("Health"))
        {
            HealthP += 1;
            SetCurrentHP();
        }
        if (other.CompareTag("Mana"))
        {
            ManaP += 1;
            SetCurrentMP();
        }
        if (other.CompareTag("Crystal"))
        {
            crystalsCollected += 1; // Set to 0.5 due to weird doubling bug
            SetCurrentCrystals();
        }

    
    }


    //Potion collection
    public void SetCurrentHP()
    {
        HealthPText.text = "HP: " + HealthP.ToString();
    }

    public void SetCurrentMP()
    {
        ManaPText.text = "MP: " + ManaP.ToString();
    }

    public void SetCurrentCrystals()
    {
        crystalCountText.text = "Crystals Collected: " + crystalsCollected.ToString() + "/4";
    }

    private IEnumerator RegenMana()
    {
        yield return new WaitForSeconds(2);

        while (currentMana < maxMana)
        {
            currentMana += maxMana / 100;

            yield return regenTick;

        }
        regen = null;
    }




}
