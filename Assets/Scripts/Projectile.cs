using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    private void Update()
    {
        MoveProjectile();
    }
    private void MoveProjectile() {
        transform.Translate(-Vector3.right * Time.deltaTime * moveSpeed);
    }
}
