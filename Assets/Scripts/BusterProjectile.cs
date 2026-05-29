using UnityEngine;

public class BusterProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;
    private Vector2 moveDirection; 
    private float damage = 4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public void Setup(bool facingLeft)
    {
        moveDirection = facingLeft ? Vector2.left : Vector2.right;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.flipX = facingLeft;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            BossHealth boss = collision.GetComponent<BossHealth>();

            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            Destroy(gameObject);
            
        } else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
