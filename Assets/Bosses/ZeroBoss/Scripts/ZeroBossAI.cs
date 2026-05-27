using System.Collections;
using UnityEngine;

public class ZeroBossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("Stats")]
    private float speed = 4f;
    private float chaseRange = 10f;
    private float attackRange = 1.5f;

    private bool isAttacking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Buscar a X por su tag "Player"
        }   
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || isAttacking) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        FlipTowardsPlayer();

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            Chase();
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    void Chase()
    {
        anim.SetBool("isRunning", true);

        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void Attack()
    {
        isAttacking = true;
        anim.SetBool("isRunning", false);
        anim.SetTrigger("attack");
        
        StartCoroutine(ZeroSlashCombo());
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void FlipTowardsPlayer()
    {
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    private IEnumerator ZeroSlashCombo()
    {
        anim.SetInteger("ComboStep", 1);
        yield return new WaitForSeconds(0.4f);

        anim.SetInteger("ComboStep", 2);
        yield return new WaitForSeconds(0.4f);

        anim.SetInteger("ComboStep", 3);
        yield return new WaitForSeconds(0.4f);

        anim.SetInteger("ComboStep", 0);
        yield return new WaitForSeconds(0.3f);

        ResetAttack();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
