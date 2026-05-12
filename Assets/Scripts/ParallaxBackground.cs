using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackground : MonoBehaviour
{
    private float length;
    private float startpos;

    [Header("Configuration")]
    public GameObject cam;

    [Tooltip("0 = Se mueve igual que la camara. 1 = Se queda quieto con el escenario")]
    public float parallaxFactor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxFactor));
        float dist = (cam.transform.position.x * parallaxFactor);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length)
        {
            startpos += length;
        }
        else if (temp < startpos - length)
        {
            startpos -= length;
        }
    }
}
