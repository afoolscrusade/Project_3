using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    //player movement
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    //jump
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    public bool isGrounded;
    public bool isJump;

    //audio
    public AudioSource audioSource;
    public AudioClip footstepsNormal;
    public AudioClip footstepsFlesh;

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
    

    void Start()
    {
        CanAttack = true;
    }

    void Update()
    {
        //Crouching is the goal here
        if(Input.GetKeyDown("c"))
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
        }
        if(Input.GetKeyUp("c"))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        //fireball attack
        if(Input.GetButtonDown("Fire1"))
        {
            FireballAttack();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);


        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    //interact with objects
        if(Input.GetKeyDown("e"))
        {
            
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player is dead");
            SceneManager.LoadScene("LoseScreen");
        }
    }

    void Jump()
    {
 
        if(isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJump = true;
        }

        else if(isJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJump = false;
        }

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
}
