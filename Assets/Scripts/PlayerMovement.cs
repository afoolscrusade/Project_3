using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;

    public AudioSource audioSource;
    public AudioClip footstepsNormal;
    public AudioClip footstepsFlesh;

    private Vector3 scaleChange;
    private GameObject plr;

    void Start()
    {
  
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

    //Jump 
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        /*if(Input.GetKeyDown("escape"))
        {
            Application.Quit();
            Debug.Log("Quit");
        }*/

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

    
}
