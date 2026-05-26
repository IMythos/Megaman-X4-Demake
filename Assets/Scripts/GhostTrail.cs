using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostTrail : MonoBehaviour
{
    [Header("Ajustes visuales")]
    public float lifetime = 0.3f;
    public Color trailColor = new Color(0.1f, 0.3f, 0.8f, 0.8f);

    private SpriteRenderer sr;
    private float fadeSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr.material != null)
        {
            sr.material.shader = Shader.Find("GUI/Text Shader");
        }

        sr.color = trailColor;
        fadeSpeed = trailColor.a / lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        Color currentColor = sr.color;
        currentColor.a -= fadeSpeed * Time.deltaTime;
        sr.color = currentColor;

        if (currentColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
