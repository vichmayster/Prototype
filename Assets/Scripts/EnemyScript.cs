using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    int speed;
    int hp;
    int randomNumber;
    bool gotHit;
    bool missed;
    bool dead;

    [Header("ScriptableObjects")]
    [SerializeField] CharecterProperties charecterProperties;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        speed = charecterProperties.speed;
        hp = charecterProperties.health;
        PlayerMovment.HitTheEnemy += HandleAttacked;
    }
    void Update()
    {
        HandleAnimations();
    }
    
    void HandleAttacked()
    {
        if (Random.Range(1,101) < 50)
        {
            gotHit = true;
            hp -= 25;
            if (hp < 0)
            {
                dead = true;
                PlayerMovment.HitTheEnemy -= HandleAttacked;
            }
        }
        else 
            missed = true;

        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.4f);
        gotHit = false;
        missed = false;
    }

    private void HandleAnimations()
    {
        animator.SetBool("gotHit", gotHit);
        animator.SetBool("missed", missed);
        animator.SetBool("dead", dead);
    }

    // Update is called once per frame
}
