using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public float Movespeed = 5f;
    public bool isMoving;
    public Vector2 input;
    private Vector2 lastMove = Vector2.down;
    private Animator animator;
    public LayerMask solidObjectLayer;
    public LayerMask GrassLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component not found on Player. Animations will not play.");
        }
        else
        {
            string names = "";
            foreach (var p in animator.parameters) names += p.name + ", ";
            Debug.Log("Animator parameters: " + names);
        }
    }
    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0; // prevent diagonal movement
            if (input != Vector2.zero)
            {
                if (animator != null)
                {
                    animator.SetFloat("MoveX", input.x);
                    animator.SetFloat("MoveY", input.y);

                }

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("isMoving", isMoving);
    }
    IEnumerator Move(Vector3 targetPos)
    {
        // guard against zero/negative speed which would cause an infinite loop
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Movespeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        CheckForEncounter();
    }
    private bool IsWalkable(Vector3 targetPos)
    {
        // if there's any collider at target position on the solid layer, it's not walkable
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectLayer);
        if (hit != null)
        {
            Debug.Log("Blocked by: " + hit.name + " at " + targetPos);
            return false;
        }

        return true;
    }
    private void CheckForEncounter()
    {
        // check if player is on grass tile
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.2f, GrassLayer);
        if (hit != null)
        {
            Debug.Log("On grass: " + hit.name);
            // 10% chance to encounter
            if (Random.Range(1, 101) <= 10)
            {
                Debug.Log("Encounter a wild Pokemon!");
            }
        }
    }
}