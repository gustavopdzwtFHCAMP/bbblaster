using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    none,
    up,
    down,
    right,
    left
}

public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public GameObject blaster;
    public GameObject bulletSpawnPoint;
    public GameObject bullet;
    Vector2 blasterPositionDefault;
    Quaternion blasterRotationDefault;
    public float speed;
    Direction lastDirection = Direction.right;
    Direction generalDirection = Direction.right;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        blasterPositionDefault = blaster.transform.localPosition;
        blasterRotationDefault = blaster.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") > 0)
        {
            generalDirection = Direction.up;

            if (blaster.transform.localScale.x == 1 && blaster.transform.localScale.y == 1)
            {
                blaster.transform.localRotation = new Quaternion(0, 0, 0, blaster.transform.rotation.w);
            }
            else
            {
                blaster.transform.localRotation = new Quaternion(0, 0, 180, blaster.transform.rotation.w);
            }

            blaster.transform.localPosition = new Vector2(0, 0.75f);
            blaster.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                generalDirection = Direction.right;
                lastDirection = Direction.right;
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                generalDirection = Direction.left;
                lastDirection = Direction.left;
            }

            if(generalDirection == Direction.up)
            {
                generalDirection = lastDirection;
            }

            blaster.transform.localRotation = blasterRotationDefault;

            switch (lastDirection)
            {
                case Direction.right:
                    blaster.transform.localPosition = new Vector2(blasterPositionDefault.x * 1, blasterPositionDefault.y);
                    blaster.transform.localScale = new Vector3(1, 1, 1);
                    break;

                case Direction.left:
                    blaster.transform.localPosition = new Vector2(blasterPositionDefault.x * -1, blasterPositionDefault.y);
                    blaster.transform.localScale = new Vector3(-1, -1, 1);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.transform.position, new Quaternion(0, 0, 0, 0));
            newBullet.transform.localScale = new Vector3(newBullet.transform.localScale.x * blaster.transform.localScale.x,
                                                        newBullet.transform.localScale.y * blaster.transform.localScale.y,
                                                        newBullet.transform.localScale.z * blaster.transform.localScale.z);

            Bullet tempBullet = newBullet.GetComponent<Bullet>();
            switch (generalDirection)
            {
                case Direction.up:
                    tempBullet.bulletDirection = transform.up;
                    break;

                case Direction.down:
                    tempBullet.bulletDirection = -transform.up;
                    break;

                case Direction.right:
                    tempBullet.bulletDirection = transform.right;
                    break;

                case Direction.left:
                    tempBullet.bulletDirection = -transform.right;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed, 0), ForceMode2D.Force);
    }
}
