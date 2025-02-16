using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public bool gettingKnockedBack { get; private set; }

    [SerializeField] private float knockBackTime = 0.2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockBackThrust)
    {
        // Debug.Log("KnockBack Applied");
        gettingKnockedBack = true;
        Vector2 direction = (transform.position - damageSource.position).normalized;
        rb.linearVelocity = direction * knockBackThrust;
        StartCoroutine(KnockRoutine());
        // Debug.Log("KnockBack Done");
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.linearVelocity = Vector2.zero;
        gettingKnockedBack = false;
    }
}
