using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackground : MonoBehaviour
{
    private float length;
    private float startposX;
    private float startposY;

    [Header("Configuration")]
    public GameObject cam;

    [Tooltip("0 = Se mueve igual que la camara. 1 = Se queda quieto con el escenario")]
    public float parallaxFactorX;

    [Tooltip("0 = Se mueve igual que la camara. 1 = Se queda quieto con el escenario")]
    public float parallaxFactorY = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startposX = transform.position.x;
        length = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x;

        startposY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float tempX = (cam.transform.position.x * (1 - parallaxFactorX));
        float distX = (cam.transform.position.x * parallaxFactorX);
        float distY = (cam.transform.position.y * parallaxFactorY);

        transform.position = new Vector3(startposX + distX, startposY + distY, transform.position.z);

        if (tempX > startposX + length)
        {
            startposX += length;
        }
        else if (tempX < startposX - length)
        {
            startposX -= length;
        }
    }
}
