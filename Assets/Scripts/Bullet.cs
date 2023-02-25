using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public float bulletSpeed;
    public Vector3 bulletDirection;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.AddForce(bulletDirection * bulletSpeed, ForceMode2D.Impulse);
    }
}
