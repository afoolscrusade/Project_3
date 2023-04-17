using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovementTutorial : MonoBehaviour
{


    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask GroundMask;
    public bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    //crouch
    private Vector3 scaleChange;
    private GameObject plr;

    //player Attack
    public bool CanAttack;
    public float AttackCooldown = 0.05f;
    public float attackCooldownNormal = 3f;
    public GameObject projectile;
    public float projectileSpeed;
    public Transform projectileSpawn;
    [SerializeField]
    private GameObject fireball;

    public Camera camera;

    // Charge Shot
    [SerializeField]
    private GameObject chargedFireball;
    public float chargeSpeed;
    public float chargeTime;
    bool isCharging;

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

    public float maxMana;
    public float currentMana;
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    Animator animator;
    bool isWalking;

    public int enemiesKilled;
    public bool bossBoarKilled;



    private void Awake()
    {
        maxHealth = 10;
        maxMana = 10;
        currentHealth = maxHealth;
        currentMana = maxMana;
        enemiesKilled = 0;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        //Potions
        SetCurrentHP();
        SetCurrentMP();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, GroundMask);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // Player invinsibility frames

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        //Potions use
        if (Input.GetKeyDown("1") && HealthP > 0 && currentHealth < maxHealth)
        {
            HealthP -= 1;
            SetCurrentHP();
            UpdateHealth(+5);
        }

        if (Input.GetKeyDown("2") && ManaP > 0 && currentMana < maxMana)
        {
            ManaP -= 1;
            SetCurrentMP();
            UpdateMana(+5);
        }

        if (horizontalInput != 0 || verticalInput != 0)
        {
            animator.SetBool("isWalking", true);
        }
        if (horizontalInput == 0 && verticalInput == 0)
        {
            animator.SetBool("isWalking", false);
        }

        if (enemiesKilled == 10 && bossBoarKilled == true)
        {
            SceneManager.LoadScene("MainHub");
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            animator.SetBool("isJumping", true);
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Crouching is the goal here
        if (Input.GetKeyDown("c"))
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
        }
        if (Input.GetKeyUp("c"))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // Starts charging fireball
        if (Input.GetKey(KeyCode.Mouse0) && chargeTime < 3)
        {
            isCharging = true;
            if (isCharging == true)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
            }
        }

        //fireball attack
        if (Input.GetButtonDown("Fire1") && currentMana > 0)
        {
            FireballAttack();
            chargeTime = 0;
            UpdateMana(-1);
        }
        else if (Input.GetButtonUp("Fire1") && chargeTime >= 2 && currentMana > 4)
        {
            ReleaseCharge();
            UpdateMana(-5);
        }

        //Testing Laser
        /*if(Input.GetButtonDown("Fire2"))
        {
            LaserAttack();
        }*/
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    public void FireballAttack()
    {
    
        //Rigidbody rb = fireball.GetComponent<Rigidbody>();

        //rb.velocity = Camera.main.transform.forward * projectileSpeed;

        //new fireball shooting
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
    }

    void ReleaseCharge()
    {
        //Instantiate(chargedFireball, projectileSpawn);
        //rigidbody rb2 = chargedFireball.GetComponent<Rigidbody>();

        //rb.velocity = Camera.main.transform.forward * projectileSpeed;

        //new fireballattack
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

    //Laser Testing
    /*public void LaserAttack()
{
    GameObject Laser = Instantiate(LaserObject, LaserSpawn) as GameObject;
}*/


    void OnTriggerEnter(Collider other)
    {
        
        //Send player to different scenes
        if (other.CompareTag("LevelOne"))
        {
            Debug.Log("Send Me to Level One!");
            SceneManager.LoadScene("Level1Terrain");
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

    private IEnumerator RegenMana()
    {
        yield return new WaitForSeconds(2);

        while(currentMana < maxMana)
        {
            currentMana += maxMana / 100;

            yield return regenTick;

        }
        regen = null;
    }

}