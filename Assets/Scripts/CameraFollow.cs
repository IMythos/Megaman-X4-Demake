using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera follow")]
    public Transform target;

    [Header("Camera Settings")]
    public float smoothSpeed = 4f;
    public float lookAheadDistance = 3f;
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);

    [Header("Lock Y Position")]
    [Tooltip("Activa para mantener la cámara a una altura fija en el eje Y, ignorando el movimiento vertical del objetivo.")]
    public bool lockY = true;
    public float fixedPositionY;

    private float currentOffsetX;
    private SpriteRenderer targetSprite;

    void Start()
    {
        if (target != null)
        {
            targetSprite = target.GetComponent<SpriteRenderer>();
        }

        fixedPositionY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null || targetSprite == null) return;

        float targetOffsetX = targetSprite.flipX ? -lookAheadDistance : lookAheadDistance;
        currentOffsetX = Mathf.Lerp(currentOffsetX, targetOffsetX, smoothSpeed * Time.deltaTime);

        Vector3 desiredPosition = target.position + offset + new Vector3(currentOffsetX, 0, 0);

        if (lockY)
        {
            desiredPosition.y = fixedPositionY;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
