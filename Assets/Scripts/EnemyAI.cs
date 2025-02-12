using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming
    }
    private State state;
    private EnemyPathFinding enemyPathFinding;
    private Animator animator;
    private void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        animator = GetComponent<Animator>();
        state = State.Roaming;
    }
    private void Start()
    {
        StartCoroutine(RoamingRoutine());
    }
    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            bool moving = Random.value > 0.3f;
            animator.SetBool("IsMoving", moving);
            if (moving)
            {
                // Vector2 roamPosition = (Vector2)transform.position + GetRoamingPosition()*3f;
                Vector2 roamPosition = GetRoamingPosition();
                FlipSprite(roamPosition.x - transform.position.x);
                enemyPathFinding.MoveTo(roamPosition);
                Debug.Log("Here");
            }
            else
            {
                enemyPathFinding.Stop();
                Debug.Log("There");
            }
            Debug.Log("Should move: " + moving);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    private void FlipSprite(float directionX)
    {
        if ((directionX > 0 && transform.localScale.x < 0) || (directionX < 0 && transform.localScale.x > 0))
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

}
