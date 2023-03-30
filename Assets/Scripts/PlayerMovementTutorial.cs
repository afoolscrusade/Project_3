using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
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

        //fireball attack
        if (Input.GetButtonDown("Fire1"))
        {
            FireballAttack();
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
        GameObject fireball = Instantiate(projectile, projectileSpawn) as GameObject;
        Rigidbody rb = fireball.GetComponent<Rigidbody>();

        rb.velocity = Camera.main.transform.forward * projectileSpeed;
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {

        yield return new WaitForSeconds(AttackCooldown);

    }

    //Laser Testing
    /*public void LaserAttack()
{
    GameObject Laser = Instantiate(LaserObject, LaserSpawn) as GameObject;
}*/
}