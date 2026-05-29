using System.Collections;
using UnityEngine;

public class ZeroBossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform slashPoint;
    private Animator anim;

    [Header("Stats")]
    private float chaseRange = 10f;
    private float attackRange = 3.0f;
    public float chaseSpeed = 3f;
    public float dashSpeed = 8f;

    [Header("Combat Hitbox")]
    public float attackRadius = 1.2f;
    public float zsaberDamage = 15f;

    [Header("Ghost Trail")]
    public GameObject ghostPrefab;
    public float ghostSpawnRate = 0.05f;
    public Color ghostColor = new Color(1f, 0.2f, 0.2f, 0.5f);
    private float ghostTimer = 0f;

    private SpriteRenderer sr;

    private bool isActing = false;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player == null || isActing) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        FlipTowardsPlayer();

        if (distanceToPlayer <= attackRange)
        {
            StartCoroutine(ZeroSlashCombo());
        }
        else if (distanceToPlayer <= chaseRange)
        {
            StartCoroutine(RandomApproachPattern());
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    private IEnumerator RandomApproachPattern()
    {
        isActing = true;

        int action = Random.Range(0, 3);

        if (action == 0) 
        {
            anim.SetBool("isRunning", false);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.2f));
        }
        else if (action == 1)
        {
            anim.SetBool("isRunning", true);
            float timer = 0f;

            while (timer < 1.5f && Vector2.Distance(transform.position, player.position) > attackRange)
            {
                FlipTowardsPlayer();
                Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);

                timer += Time.deltaTime;
                yield return null;
            }
            anim.SetBool("isRunning", false);
        }
        else if (action == 2) // DASH - falta mejorar la logica y la estela al hacer dash
        {
            anim.SetBool("isDashing", true);
            anim.SetBool("isRunning", false);

            float timer = 0f;
            int direction = player.position.x > transform.position.x ? 1 : -1;

            while (timer < 0.4f && Vector2.Distance(transform.position, player.position) > attackRange)
            {
                transform.position += new Vector3(direction * dashSpeed * Time.deltaTime, 0, 0);

                ghostTimer -= Time.deltaTime;
                if (ghostTimer <= 0)
                {
                    SpawnGhost();
                    ghostTimer = ghostSpawnRate;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            anim.SetBool("isDashing", false);
        }

        isActing = false;
    }

    // HITBOX DEL ZSABER
    private IEnumerator ZeroSlashCombo()
    {
        isActing = true;
        anim.SetBool("isRunning", false);

        // Slash 1
        anim.SetInteger("ComboStep", 1);
        yield return new WaitForSeconds(0.2f);
        DealSaberDamage(); // Aplica la hitbox
        yield return new WaitForSeconds(0.2f);

        // Slash 2
        anim.SetInteger("ComboStep", 2);
        yield return new WaitForSeconds(0.2f);
        DealSaberDamage();
        yield return new WaitForSeconds(0.2f);

        // Slash 3
        anim.SetInteger("ComboStep", 3);
        yield return new WaitForSeconds(0.2f);
        DealSaberDamage();
        yield return new WaitForSeconds(0.2f);

        // interrupcion del combo
        anim.SetInteger("ComboStep", 0);
        yield return new WaitForSeconds(0.5f);

        isActing = false;
    }

    private void DealSaberDamage()
    {
        if (slashPoint == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(slashPoint.position, attackRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerMovement pm = hit.GetComponent<PlayerMovement>();
                if (pm != null)
                {
                    float knockbackDir = (hit.transform.position.x > transform.position.x) ? 1f : -1f;

                    pm.TakeDamageBoss((int)zsaberDamage, knockbackDir);
                }
            }
        }
    }

    void FlipTowardsPlayer()
    {
        Vector3 currentScale = transform.localScale;
        float baseScaleX = Mathf.Abs(currentScale.x);

        if (player.position.x > transform.position.x)
        {
            currentScale.x = baseScaleX;
        }
        else if (player.position.x < transform.position.x)
        {
            currentScale.x = -baseScaleX;
        }

        transform.localScale = currentScale;
    }

    void SpawnGhost()
    {
        if (ghostPrefab == null || sr == null) return;

        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();

        if (ghostSR != null)
        {
            ghostSR.sprite = sr.sprite;
            ghost.transform.localScale = transform.localScale;
            ghostSR.color = ghostColor;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (slashPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(slashPoint.position, attackRadius);
        }
    }
}