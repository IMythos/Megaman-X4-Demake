using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public void OnAnimationFinished()
    {
        Destroy(gameObject);
    }
}
