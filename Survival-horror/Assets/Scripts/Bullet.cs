using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float bulletSpeed = 100f;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, 5f);
    }

    public void AddForce(Vector3 direction)
    {
        rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyStats>().GetDamage(damage);

            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
