using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField]
    GameObject deathVFX;

    [SerializeField]
    float health = 20;

    [SerializeField]
    private bool vulnerable = false;

    [SerializeField]
    int enemyID;

    private LevelLoader levelLoad;

    GameObject currentTarget;

    Vector2 currentTargetPos;

    float movementSpeed = 1f;

    Animator animator;

    DefenderSpawner defenderSpawner;

    LevelController levelControll;

    private void Awake()
    {
        levelControll = FindObjectOfType<LevelController>();

        if(levelControll)
            levelControll.AddAttackers();
    }

    void Start()
    {
        health *= PlayerPrefsController.GetDifficulty();
        defenderSpawner = FindObjectOfType<DefenderSpawner>();
        levelLoad = FindObjectOfType<LevelLoader>();
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        if(levelControll)
            levelControll.RemoveAttackers();
    }

    void Update()
    {
        Movement();
        
        if(!defenderSpawner.IsOccupied(currentTargetPos))
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void Movement()
    {
        transform.Translate(Vector2.left * movementSpeed * Time.deltaTime);
    }

    private void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    private void SetVulnerability()
    {
        vulnerable = true;
    }

    private void Attack()
    {
        animator.SetBool("isAttacking", true);
    }

    private void StrikeTarget()
    {
        if (!currentTarget)
            return;
        DefenderResource health = currentTarget.GetComponent<DefenderResource>();

        if(health)
        {
            health.DamageHealth(GetComponent<DamageDealer>().GetDamage());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherGameobject = other.gameObject;
        DamageDealer damage = otherGameobject.GetComponent<DamageDealer>();
        DefenderResource defender = otherGameobject.GetComponent<DefenderResource>();


        if (damage && vulnerable)
        {
            health -= damage.GetDamage() * 1.01f;

            if (health <= 0)
            {
                GameObject newVFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
                newVFX.transform.parent = levelLoad.VFXContainer();
                Destroy(newVFX, 1f);
                Destroy(this.gameObject);
            }
        }

        if(otherGameobject.CompareTag("GraveStone") && enemyID == 1)
        {
            animator.SetTrigger("JumpTrigger");
        }
        else if (defender)
        {
            currentTarget = otherGameobject;
            currentTargetPos = currentTarget.transform.position;
            Attack();
        }
    }
}
