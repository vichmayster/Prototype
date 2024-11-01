using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class Enemy2Script : MonoBehaviour
{
    public delegate void EnemyAttack();
    public static event EnemyAttack hitThePlayer;

    public delegate void LevelPoints();
    public static event LevelPoints mushroomPoints;

    public delegate void CounterAttack();
    public static event CounterAttack counterAttackEvent;


    Animator animator;
    int speed;
    int hp;
    int randomNumber;
    int facingDirection = 1;
    bool gotHit;
    bool missed;
    bool dead;
    bool freezed = false;
    bool facingRight = true;
    bool playerSpotted = false;
    bool alert = false;
    bool attack = false;
    bool counterAttack = false;
    bool canAttack = true;
    bool inRange = false;
    bool inPlayersRange = false;

    bool vulnerable;
    bool vulnerableLeft;
    bool vulnerableRight;

    CapsuleCollider2D capsuleCollider;

    [SerializeField] int attackDistance;

    //outside source
    int success_hit;
    int success_hit_w2;
    int success_hit_w3;
    int hit_damage;
    int hit_damage_w2;
    int hit_damage_w3;

    [Header("Player Enemy Interactions")]
    [SerializeField] int detectionRang = 10;

    [Header("Object Destruction Duration")]
    [SerializeField] int objectDestructionDuration = 10;

    [Header("Reset Attack Duration")]
    [SerializeField] float resetAttackDuration = 0.4f;

    Rigidbody2D rb;

    [Header("ScriptableObjects")]
    [SerializeField] CharecterProperties charecterProperties;

    [Header("Text Messages")]
    [SerializeField] GameObject message;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        speed = charecterProperties.speed;
        hp = charecterProperties.health;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SwitchSide());
        HandleLevelUpdate();
        LevelSystemScript.updateLevelEvent += HandleLevelUpdate;

        //outside source
       

    }


    void Update()
    {
        HandleAnimations();
        HandleAttack();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandlePlayerAttack();
        HandlePlayerDetection();

    }
    private void HandleAnimations()
    {
        animator.SetBool("gotHit", gotHit);
        animator.SetBool("missed", missed);
        animator.SetBool("dead", dead);
        animator.SetFloat("xVelocity", rb.velocity.x);
        animator.SetBool("attack", attack);
        animator.SetBool("counterAttack", counterAttack);
    }
    private void HandleAttack()
    {
        if (attack && canAttack)
        {
            StartCoroutine(Attack());
        }
    }
    private void HandleMovement()
    {
        if (!freezed)
        {
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        }
    }
    private void HandlePlayerAttack()
    {
        if (vulnerable && !inPlayersRange)
        {
            PlayerCombatScript.HitTheEnemy -= HandleAttacked;
            PlayerCombatScript.HitTheEnemy += HandleAttacked;

            PlayerCombatScript.HitTheEnemy2 -= HandleAttacked2;
            PlayerCombatScript.HitTheEnemy2 += HandleAttacked2;

            PlayerCombatScript.HitTheEnemy3 -= HandleAttacked3;
            PlayerCombatScript.HitTheEnemy3 += HandleAttacked3;

            inPlayersRange = true;
        }
        else if (!vulnerable && inPlayersRange)
        {
            PlayerCombatScript.HitTheEnemy -= HandleAttacked;

            PlayerCombatScript.HitTheEnemy2 -= HandleAttacked2;

            inPlayersRange = false;
        }
        else
            return;
    }

    // Handle Attacked 1, 2, 3: the logic after players attack with weapons 1, 2, or 3
    private void HandleAttacked()
    {
        if (Random.Range(1, 101) < success_hit)
        {
            gotHit = true;
            StartCoroutine(Freez(1));
            hp -= hit_damage;
            if (hp <= 0)
            {
                dead = true;
                PlayerCombatScript.HitTheEnemy -= HandleAttacked;
                InvokeLevelPoints();
                StartCoroutine(EnemyDeath());
            }
        }
        else
        {
            missed = true;
            StartCoroutine(Freez(1));
            StartCoroutine(SpawnMissedText());
        }

        StartCoroutine(ResetAttacked());
    }
    private void HandleAttacked2()
    {
        counterAttack = true;
        StartCoroutine(CounterAttackCorutine());
        StartCoroutine(resetCounterAttack());
    }
    private void HandleAttacked3()
    {
        if (Random.Range(1, 101) < success_hit_w3)
        {
            gotHit = true;
            StartCoroutine(Freez(1));
            hp -= hit_damage_w3;
            if (hp <= 0)
            {
                dead = true;
                PlayerCombatScript.HitTheEnemy -= HandleAttacked;
                InvokeLevelPoints();
                StartCoroutine(EnemyDeath());
            }
        }
        else
        {
            missed = true;
            StartCoroutine(Freez(1));
            StartCoroutine(SpawnMissedText());
        }

        StartCoroutine(ResetAttacked());
    }
    private void HandlePlayerDetection()
    {
        if (facingRight)
        {
            playerSpotted = Physics2D.Raycast(transform.position, Vector2.right, detectionRang, LayerMask.GetMask("Player"));
            //Debug.Log("Player spotted " + playerSpotted);
            inRange = Physics2D.Raycast(transform.position, Vector2.right, attackDistance, LayerMask.GetMask("Player"));
        }
        else
        {
            playerSpotted = Physics2D.Raycast(transform.position, Vector2.left, detectionRang, LayerMask.GetMask("Player"));
            //Debug.Log("Player spotted " + playerSpotted);
            inRange = Physics2D.Raycast(transform.position, Vector2.left, attackDistance, LayerMask.GetMask("Player"));
        }

        if (facingRight && canAttack)
        {
            attack = Physics2D.Raycast(transform.position, Vector2.right, attackDistance, LayerMask.GetMask("Player"));
            // Debug.Log("Player attacked " + attack);
        }
        else if (!facingRight && canAttack)
        {
            attack = Physics2D.Raycast(transform.position, Vector2.left, attackDistance, LayerMask.GetMask("Player"));
            //Debug.Log("Player attacked " + attack);
        }

        if (playerSpotted)
            alert = true;

        vulnerableLeft = Physics2D.Raycast(transform.position, Vector2.left, attackDistance, LayerMask.GetMask("Player"));
        vulnerableRight = Physics2D.Raycast(transform.position, Vector2.right, attackDistance, LayerMask.GetMask("Player"));
        vulnerable = vulnerableLeft || vulnerableRight;
    }
    private void InvokeLevelPoints()
    {
        Debug.Log("before Invoke Level Points");
        if (mushroomPoints != null)
        {
            Debug.Log("Send Invoke Level Points");
            mushroomPoints.Invoke();
        }
    }
    private void InvokeHitThePlayer()
    {

        if (hitThePlayer != null)
        {
            if (!gotHit)
                hitThePlayer.Invoke();


        }
    }
    private void OnDrawGizmos()
    {

        if (facingRight)
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + detectionRang, transform.position.y));
        else
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - detectionRang, transform.position.y));
    }
    private void HandleFreez()
    {
        rb.velocity = Vector2.zero;
    }
    private void HandleLevelUpdate()
    {
        success_hit = LevelSystemScript.mushroomSuccessHitW1;
        success_hit_w2 = LevelSystemScript.mushroomSuccessHitW2;
        success_hit_w3 = LevelSystemScript.mushroomSuccessHitW3;
        hit_damage = LevelSystemScript.mushroomHitDamageW2;
        hit_damage_w2 = LevelSystemScript.mushroomHitDamageW2;
        hit_damage_w3 = LevelSystemScript.mushroomHitDamageW3;
    }
    private IEnumerator EnemyDeath()
    {
        if (capsuleCollider != null)
        {
            Destroy(capsuleCollider);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        yield return new WaitForSeconds(objectDestructionDuration);
        Destroy(gameObject);
    }
    private IEnumerator CounterAttackCorutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (counterAttackEvent != null)
            counterAttackEvent.Invoke();
    }
    private IEnumerator resetCounterAttack()
    {
        yield return new WaitForSeconds(resetAttackDuration);
        counterAttack = false;
    }
    private IEnumerator Attack()
    {

        canAttack = false;
        StartCoroutine(Freez(2));
        yield return new WaitForSeconds(0.4f);
        attack = false;

        yield return new WaitForSeconds(1);
        InvokeHitThePlayer();


        yield return new WaitForSeconds(2);
        canAttack = true;


    }
    private IEnumerator ResetAttacked()
    {
        yield return new WaitForSeconds(resetAttackDuration);
        gotHit = false;
        missed = false;
    }
    private IEnumerator Freez(float seconds)
    {
        freezed = true;
        HandleFreez();
        yield return new WaitForSeconds(seconds);
        freezed = false;
        HandleMovement();
    }
    private IEnumerator SwitchSide()
    {
        while (true)
        {
            if (!playerSpotted && !alert)
            {
                yield return new WaitForSeconds(5);

                StartCoroutine(Freez(1));
                yield return new WaitForSeconds(1);

                facingDirection *= -1;
                transform.Rotate(0, 180, 0);
                facingRight = !facingRight;
            }
            else if (!playerSpotted && alert)
            {
                freezed = true;
                HandleFreez();
                facingDirection *= -1;
                transform.Rotate(0, 180, 0);
                facingRight = !facingRight;
                yield return new WaitForSeconds(2);

                if (playerSpotted)
                {
                    freezed = false;
                    HandleMovement();
                }
                else
                {
                    facingDirection *= -1;
                    transform.Rotate(0, 180, 0);
                    facingRight = !facingRight;
                    yield return new WaitForSeconds(2);
                    if (playerSpotted)
                    {
                        freezed = false;
                        HandleMovement();
                    }
                    else
                    {
                        alert = false;
                        freezed = false;
                        facingDirection *= -1;
                        transform.Rotate(0, 180, 0);
                        facingRight = !facingRight;
                        HandleMovement();
                    }
                }
            }
            else
            {
                yield return null;
            }
            // Debug.Log("switch");
        }
    }
    private IEnumerator SpawnMissedText()
    {
        Vector3 spawnLocation = transform.position;
        GameObject text = Instantiate(message, spawnLocation, Quaternion.identity);
        yield return new WaitForSeconds(3);
        Destroy(text);
    }
}
