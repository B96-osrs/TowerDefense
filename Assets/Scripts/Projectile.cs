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
        //Debug.Log("Projectile triggered with: " + other.gameObject.name);
        if(!target) return;
        target.GetComponent<Enemy>().takeDamage((int)damage);
        Destroy(gameObject);

            
    }
}
