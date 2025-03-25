using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    public Transform turretRotationPoint;
    public float targetRange = 3f;
    private Transform target;
    public LayerMask enemyMask;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float timeUntilFire;



    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTurret();
        
        if (!CheckTargetIsInRange())
        {
            target = null;
        } else
        {
            timeUntilFire += Time.deltaTime;
            if(timeUntilFire >= fireRate)
            {
                Fire();
                timeUntilFire = 0f;
            }
        }
    }


    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetTarget(target);
    }


    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetRange, (Vector2)transform.position, 0f, enemyMask);


        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTurret()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, 180f * Time.deltaTime);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(transform.position, target.position) <= targetRange;
    }




    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetRange);
    }

}
