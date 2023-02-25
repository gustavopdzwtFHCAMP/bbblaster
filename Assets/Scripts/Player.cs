using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public GameObject blaster;
    public GameObject bulletSpawnPoint;
    public GameObject bullet;
    public float bulletSpeed;
    Vector2 blasterPosition;
    Quaternion blasterRotation;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        blasterPosition = blaster.transform.localPosition;
        blasterRotation = blaster.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") > 0)
        {
            blaster.transform.localPosition = new Vector2(0, 0.75f);
            if(blaster.GetComponent<SpriteRenderer>().flipX == true && blaster.GetComponent<SpriteRenderer>().flipY == true)
            {
                blaster.transform.localRotation = new Quaternion(0, 0, 180, blaster.transform.rotation.w);
            }
            else
            {
                blaster.transform.localRotation = new Quaternion(0, 0, 0, blaster.transform.rotation.w);
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                blaster.transform.localRotation = blasterRotation;
                blaster.GetComponent<SpriteRenderer>().flipX = true;
                blaster.GetComponent<SpriteRenderer>().flipY = true;
                blaster.transform.localPosition = new Vector2(blasterPosition.x * -1, blasterPosition.y);
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                blaster.transform.localRotation = blasterRotation;
                blaster.GetComponent<SpriteRenderer>().flipX = false;
                blaster.GetComponent<SpriteRenderer>().flipY = false;
                blaster.transform.localPosition = new Vector2(blasterPosition.x * 1, blasterPosition.y);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.transform.position, new Quaternion(0, 0, 0, 0));
            newBullet.GetComponent<Rigidbody2D>().AddForce(blaster.transform.forward * bulletSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed, 0), ForceMode2D.Force);
    }
}
