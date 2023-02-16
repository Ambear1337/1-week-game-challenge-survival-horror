using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleEquipped : EquippedItem
{
    [SerializeField] Transform weaponEnd;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject pickupItem;

    [SerializeField] float reloadTime = 5f;

    [SerializeField] LayerMask enemyMask;

    bool canShoot = true;

    private void Update()
    {
        Ray ray = new Ray(transform.parent.position, transform.parent.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            transform.LookAt(hit.point);
        }
    }

    public override void Use()
    {
        if (canShoot)
        {
            Debug.Log("Bullet should be spawned!");
            
            canShoot = false;
            GameObject bullet = Instantiate(bulletPrefab, weaponEnd.position, Quaternion.identity, null);
            bullet.GetComponent<Bullet>().AddForce(weaponEnd.position - transform.position);
            StartCoroutine(ReloadCoroutine());
        }
    }

    public override void Drop()
    {
        Instantiate(pickupItem, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }
}
