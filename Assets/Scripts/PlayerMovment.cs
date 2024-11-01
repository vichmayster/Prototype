using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovment : MonoBehaviour
{
    
    public delegate void EnemySpawner();
    public static event EnemySpawner enemySpawn;

    Animator animator;
    Rigidbody2D rb;

    float input;
    static bool directionRight = true;

    float horizontalVelocity;
    float velocityThreshold;

    [Header("Dodging")]
    bool isDodging = false;
    [SerializeField] private float dodgeDuration = 0.3f;

    static int facingDiraction = 1;
   

    [Header("Movment")]
    [SerializeField] int jumpForce;
    [SerializeField] Vector2 dodgeForce;
    int speed;

    [Header("Collision")]
    [SerializeField] float distanceToGround;
    [SerializeField] LayerMask ground;
    bool isGrounded;
    bool onTarget;


    [Header("ScriptableObjects")]
    [SerializeField] CharecterProperties charecterProperties;

    [Header("Wall interactions")]
    [SerializeField] private float wallJumpDuration = 0.6f;
    [SerializeField] private Vector2 wallJunpForce;
    [SerializeField] float wallDistance = 0.5f;
    private bool isWallJumping;
    bool wallDetected = false;


    // Start is called before the first frame update
    void Start()
    {
        //EnemyScript.hitThePlayer += GotHit;
        speed = charecterProperties.speed;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        HandleCollision();
        HandleMovment();
        HandleFlip();
        
    }

    public static int GetDirection()
    {
        return facingDiraction;
    }

    public static bool GetBoolDiraction()
    {
        return directionRight;
    }
   
   

    private void HandleEnemySpawner()
    {
        //Debug.Log("Enemy Spawner triggered");
        if (enemySpawn != null)
        {
            enemySpawn.Invoke();
           // Debug.Log("Invoke Spawner");

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("EnemyTrigger"))
        {
           // Debug.Log("Player inside the trigger");
            HandleEnemySpawner();
        }
    }

    private void HandleInput()
    {
        input = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && wallDetected)
        {
            WallJump();
        }
        if(Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            DodgeBack();
        }
        


    }


    private void DodgeBack()
    {
        rb.velocity = new Vector2(dodgeForce.x * -facingDiraction, dodgeForce.y);
        StartCoroutine(dodgeRoutine());
    }

    private void WallJump()
    {
        rb.velocity = new Vector2(wallJunpForce.x * -facingDiraction, wallJunpForce.y);
        StartCoroutine(WallJumpRoutine());
    }
    
    private IEnumerator dodgeRoutine()
    {
        isDodging = true;
        yield return new WaitForSeconds(dodgeDuration);
        isDodging = false;

    }



    private IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpDuration);

        isWallJumping = false;
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, ground);
        if (directionRight)
            wallDetected = Physics2D.Raycast(transform.position, Vector2.right, wallDistance, LayerMask.GetMask("Wall"));
        else
            wallDetected = Physics2D.Raycast(transform.position, Vector2.left, wallDistance, LayerMask.GetMask("Wall"));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - distanceToGround));
    }

    private void HandleMovment()
    {
        if (isWallJumping)
            return;

        if (isDodging)
            return;

        rb.velocity = new Vector2(input * speed, rb.velocity.y);
        horizontalVelocity = Mathf.Abs(rb.velocity.x);
        velocityThreshold = 0.01f;
    }

    private void HandleFlip()
    {
        if (rb.velocity.x < 0 && directionRight || rb.velocity.x > 0 && !directionRight )
            if(!isDodging)
                Flip();

    }
   
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        directionRight = !directionRight;
        facingDiraction *= -1;
    }
    private void HandleAnimations()
    {
        animator.SetBool("isMoving", horizontalVelocity > velocityThreshold);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded);
       

    }

}

   