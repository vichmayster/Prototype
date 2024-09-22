using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovment : MonoBehaviour
{
    public delegate void PlayerAttack();
    public static event PlayerAttack HitTheEnemy;

    Animator animator;
    Rigidbody2D rb;
    float input;
    bool directionRight = true;
    bool isAttacking;
    int facingDiraction = 1;

    [Header("Movment")]
    [SerializeField] int jumpForce;
    int speed;

    [Header("Collision with ground")]
    [SerializeField] float distanceToGround;
    [SerializeField] LayerMask ground;
    bool isGrounded;
    bool onTarget;

    [Header("Attack properties")]
    [SerializeField] float attackDistance;

    [Header("ScriptableObjects")]
    [SerializeField] CharecterProperties charecterProperties;

    [Header("Wall interactions")]
    [SerializeField] private float wallJumpDuration = 0.6f;
    [SerializeField] private Vector2 wallJunpForce;
    private bool isWallJumping;
    bool wallDetected = false;
    float wallDistance = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        speed = charecterProperties.speed;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        HandleAttack();
        HandleCollision();
        HandleInput();
        HandleMovment();
        HandleAnimations();
        HandleFlip();
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


    }

    private void WallJump()
    {
        rb.velocity = new Vector2(wallJunpForce.x * -facingDiraction, wallJunpForce.y);
        StartCoroutine(WallJumpRoutine());
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
            onTarget = Physics2D.Raycast(transform.position, Vector2.right, attackDistance, LayerMask.GetMask("Enemy"));
        else
            onTarget = Physics2D.Raycast(transform.position, Vector2.left, attackDistance, LayerMask.GetMask("Enemy"));
       
        if (directionRight)
            wallDetected = Physics2D.Raycast(transform.position, Vector2.right, wallDistance, LayerMask.GetMask("Wall"));
        else
            wallDetected = Physics2D.Raycast(transform.position, Vector2.left, wallDistance, LayerMask.GetMask("Wall"));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - distanceToGround));
        if (directionRight)
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + attackDistance, transform.position.y));
        else
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - attackDistance, transform.position.y));
    }

    private void HandleMovment()
    {
        if (isWallJumping)
            return;

        rb.velocity = new Vector2(input * speed, rb.velocity.y);
    }

    private void HandleFlip()
    {
        if (rb.velocity.x < 0 && directionRight || rb.velocity.x > 0 && !directionRight)
            Flip();

    }
    private void HandleAttack()
    {
        if (Input.GetKey(KeyCode.F) && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(ResetAttack());
            if (onTarget)
            {
                Debug.Log("on target");
                InvokeHitTheEnemy();
            }
        }


    }

    private void InvokeHitTheEnemy()
    {
        Debug.Log("inside InvokeHitTheEnemy");
        if (HitTheEnemy != null)
        {
            HitTheEnemy.Invoke();
            Debug.Log("Invoke Hit The Enemy");
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        directionRight = !directionRight;
        facingDiraction *= -1;
    }
    private void HandleAnimations()
    {
        animator.SetBool("isMoving", rb.velocity.x != 0);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isAttacking", isAttacking);

    }
}

   