using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Tilemaps;

public class Turret : MonoBehaviour
{
    public float fireRate = 1f;
    public float targetRange = 3f;

    private float timeUntilFire;

    public Transform turretRotationPoint;
    private Transform target;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public LayerMask enemyMask;
    private Tilemap tilemap;


    private void Awake()
    {
        if (tilemap == null)
        {
            tilemap = GameObject.FindAnyObjectByType<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogError("No Tilemap found (Turret.cs)");
            }
        }
    }



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
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= fireRate)
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetRange, enemyMask);
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
        // Check if the target is within range, specifically the tile center on which the enemy is
        return Vector2.Distance(transform.position, target.position) <= targetRange;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetRange);
    }

    public bool isInRange(Vector3Int tilePosition)
    {
        //A regular Vector2.Distance has rounding errors and did not color the tiles correctly
        Vector3 turretCenter = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
        Vector3 tileCenter = tilemap.GetCellCenterWorld(tilePosition);

        float distanceSquared = (turretCenter - tileCenter).sqrMagnitude;
        float rangeSquared = targetRange * targetRange;
        //Debug.Log("Turret Center: " + turretCenter + " | Tile Center: " + tileCenter + " | Distance: " + Mathf.Sqrt(distanceSquared));
        return distanceSquared <= rangeSquared;
    }


}
