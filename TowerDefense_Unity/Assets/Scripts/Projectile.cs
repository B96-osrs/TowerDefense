using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    public Rigidbody2D rb;
    public float speed = 10f;
    public float damage = 50f;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!target) return;
        Debug.Log(target);
        if(target.GetComponent<Enemy_BFS>() != null)
        {
            target.GetComponent<Enemy_BFS>().takeDamage((int)damage);
            Debug.Log("Hit enemy");
        } else if (target.GetComponent<Enemy_Left_Only>() != null)
        {
            target.GetComponent<Enemy_Left_Only>().takeDamage((int)damage);
            Debug.Log("Hit enemy left only");
        }
        else if (target.GetComponent<Enemy_Right_Only>() != null)
        {
            target.GetComponent<Enemy_Right_Only>().takeDamage((int)damage);
            Debug.Log("Hit enemy right only");
        }
        else if (target.GetComponent<Enemy_Random>() != null)
        {
            target.GetComponent<Enemy_Random>().takeDamage((int)damage);
            Debug.Log("Hit eneme Random");
        }
        else if (target.GetComponent<Enemy_A_Star>() != null)
        {
            target.GetComponent<Enemy_A_Star>().takeDamage((int)damage);
            Debug.Log("Hit enemy AStar");
        }
        else if (target.GetComponent<Enemy_AI>() != null)
        {
            target.GetComponent<Enemy_AI>().takeDamage((int)damage);
            Debug.Log("Hit Enemy_AI");
        }
        Destroy(gameObject);
    }
}
