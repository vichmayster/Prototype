using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovment : MonoBehaviour
{
    public delegate void PlayerAttack();
    public static event PlayerAttack HitTheEnemy;
    public static event PlayerAttack HitTheEnemy2;
    public static event PlayerAttack HitTheEnemy3;
    public delegate void EnemySpawner();
    public static event EnemySpawner enemySpawn;

    Animator animator;
    Rigidbody2D rb;
    float input;
    bool directionRight = true;
    bool isAttacking;
    bool inEnemyRange = false;
    bool gotHit = false;
    bool isDodging = false;
    bool isDead = false;
    int facingDiraction = 1;
    int hp = 100;

    bool weapon1 = true;
    bool weapon2 = false;
    bool weapon3 = false;

    bool isAvailbleWeapon2 = true;
    bool isAvailbleWeapon3 = true;

    [Header("Movment")]
    [SerializeField] int jumpForce;
    [SerializeField] Vector2 dodgeForce;
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
        EnemyScript.hitThePlayer += GotHit;
        speed = charecterProperties.speed;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttack();
        HandleAttacked();
        HandleCollision();
        HandleInput();
        HandleMovment();
        HandleFlip();
        HandleAnimations();
    }

   
    private void HandleAttacked()
    {
        if (onTarget && !inEnemyRange)
        {
            EnemyScript.hitThePlayer -= GotHit;
            EnemyScript.hitThePlayer += GotHit;
            inEnemyRange = true;
            Debug.Log("subscribe");
        }
        else if (!onTarget && inEnemyRange)
        {

            EnemyScript.hitThePlayer -= GotHit;
            inEnemyRange = false;
            Debug.Log("unsabscribe");
        }
        else
            return;
    }

    void GotHit()
    {
        hp -= 10;
        gotHit = true;
        if (hp < 1)
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        Debug.Log("Player Got Hit, hp: " + hp);
        StartCoroutine(ResetGotHit());
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponToUse(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(isAvailbleWeapon2)
                WeaponToUse(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (isAvailbleWeapon3)
                WeaponToUse(3);
        }


    }

    private void WeaponToUse(int slot)
    {
        switch (slot)
        {
            case 1:
                weapon1 = true;
                weapon2 = false;
                weapon3 = false;
                break;
            case 2:
                weapon1 = false;
                weapon2 = true;
                weapon3 = false;
                break;
            case 3:
                weapon1 = false;
                weapon2 = false;
                weapon3 = true;
                break;
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
        yield return new WaitForSeconds(0.3f);
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

        if (isDodging)
            return;

        rb.velocity = new Vector2(input * speed, rb.velocity.y);
    }

    private void HandleFlip()
    {
        if (rb.velocity.x < 0 && directionRight || rb.velocity.x > 0 && !directionRight )
            if(!isDodging)
                Flip();

    }
    private void HandleAttack()
    {
        if (Input.GetKey(KeyCode.F) && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(ResetAttack());
            if (onTarget)
                InvokeHitTheEnemy();   
        }


    }

    private void InvokeHitTheEnemy()
    {
       // Debug.Log("inside InvokeHitTheEnemy");
        if (HitTheEnemy != null && weapon1)
        {
            HitTheEnemy.Invoke();
           // Debug.Log("Invoke Hit The Enemy");
        }
        else if (HitTheEnemy2 != null && weapon2)
            HitTheEnemy2.Invoke();

        else if (HitTheEnemy3 != null && weapon3)
            HitTheEnemy3.Invoke();

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
        animator.SetBool("isAttacking_1", weapon1);
        animator.SetBool("isAttacking_2", weapon2);
        animator.SetBool("isAttacking_3", weapon3);
        animator.SetBool("gotHit", gotHit);
        animator.SetBool("isDead", isDead);

    }

   private IEnumerator ResetGotHit()
    {
        yield return new WaitForSeconds(0.5f);
        gotHit = false;
    }
}

   