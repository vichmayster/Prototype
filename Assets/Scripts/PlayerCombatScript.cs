using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour
{
    public delegate void PlayerAttack();
    public static event PlayerAttack HitTheEnemy;
    public static event PlayerAttack HitTheEnemy2;
    public static event PlayerAttack HitTheEnemy3;

    public int hp;


    Animator animator;
    Rigidbody2D rb;

    bool inEnemyRange = false;
    bool isDead = false;
    bool gotHit = false;
    bool isAttacking;
    bool onTarget;
    bool directionRight = true;
    bool vulnerableLeft = false;
    bool vulnerableRight = false;
    bool vulnerableL = false;

    float input;
    int facingDiraction = 1;

    //weapons
    bool weapon1 = true;
    bool weapon2 = false;
    bool weapon3 = false;

    [Header("Available Weapons")]
    [SerializeField] bool isAvailbleWeapon2 = true;
    [SerializeField] bool isAvailbleWeapon3 = true;

    [Header("Player's damege")]
    [SerializeField] int damege = 10;
    [SerializeField] float resetGotHitDuration = 0.5f;

    [Header("Attack properties")]
    [SerializeField] float attackDistance;
    [SerializeField] float attackDuration = 0.4f;

    [Header("ScriptableObjects")]
    [SerializeField] CharecterProperties charecterProperties;

    // Start is called before the first frame update
    void Start()
    {
        hp = charecterProperties.health;
        EnemyScript.hitThePlayer += GotHit;
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
        HandleAttack();
        HandleCollision();
        HandleFlip();
        
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

    private void HandleAnimations()
    {
       
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isAttacking_1", weapon1);
        animator.SetBool("isAttacking_2", weapon2);
        animator.SetBool("isAttacking_3", weapon3);
        animator.SetBool("gotHit", gotHit);
        animator.SetBool("isDead", isDead);

    }

  

    private void HandleCollision()
    {
        bool wasInRange = vulnerableL;

        if (directionRight)
            onTarget = Physics2D.Raycast(transform.position, Vector2.right, attackDistance, LayerMask.GetMask("Enemy"));
        else
            onTarget = Physics2D.Raycast(transform.position, Vector2.left, attackDistance, LayerMask.GetMask("Enemy"));

        float detectionDistance = attackDistance * 1.5f;
        vulnerableLeft = Physics2D.Raycast(transform.position, Vector2.left, detectionDistance, LayerMask.GetMask("Enemy"));
        vulnerableRight = Physics2D.Raycast(transform.position, Vector2.right, detectionDistance, LayerMask.GetMask("Enemy"));

        vulnerableL = vulnerableLeft || vulnerableRight;

        // Clear state when entering/exiting range
        if (vulnerableL && !wasInRange)
        {
            inEnemyRange = true;
            GameStateManager.Instance.GoToCombat();
            // Resubscribe to combat events
            EnemyScript.hitThePlayer -= GotHit;
            EnemyScript.hitThePlayer += GotHit;
            Debug.Log("Entering Combat Range");
        }
        else if (!vulnerableL && wasInRange)
        {
            inEnemyRange = false;
            EnemyScript.hitThePlayer -= GotHit;
            GameStateManager.Instance.GoToPlaying();
            Debug.Log("Exiting Combat Range");
        }
    }

    private void HandleInput()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponToUse(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isAvailbleWeapon2)
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

    private void OnDrawGizmos()
    {
       
        if (directionRight)
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + attackDistance, transform.position.y));
        else
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - attackDistance, transform.position.y));
    }

    private void HandleFlip()
    {
        facingDiraction = PlayerMovement.GetDirection();
        directionRight = PlayerMovement.GetBoolDiraction();
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

    void GotHit()
    {
        hp -= damege;
        gotHit = true;
        if (hp < 1)
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            GameStateManager.Instance.GoToGameOver();
            return;
        }
        Debug.Log("Player Got Hit, hp: " + hp);
        StartCoroutine(ResetGotHit());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    private IEnumerator ResetGotHit()
    {
        yield return new WaitForSeconds(resetGotHitDuration);
        gotHit = false;
    }
}
