using UnityEngine;

public class BusterProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;
    private Vector2 moveDirection; 
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
}
