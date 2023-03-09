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
    public GameObject body;
    public GameObject blaster;
    public GameObject bulletSpawnPoint;
    public GameObject bullet;
    Vector2 blasterPositionDefault;
    Quaternion blasterRotationDefault;
    public float walkSpeed;
    public float jumpSpeed;
    public float walkSpeedMinimizer;
    float walkSpeedMinimizerTemp;
    bool grounded;
    Direction lastDirection = Direction.right;
    Direction generalDirection = Direction.right;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        blasterPositionDefault = blaster.transform.localPosition;
        blasterRotationDefault = blaster.transform.rotation;
        walkSpeedMinimizerTemp = walkSpeedMinimizer;
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
                    body.transform.localScale = new Vector3(1, 1, 1);
                    blaster.transform.localPosition = new Vector2(blasterPositionDefault.x * 1, blasterPositionDefault.y);
                    blaster.transform.localScale = new Vector3(1, 1, 1);
                    break;

                case Direction.left:
                    body.transform.localScale = new Vector3(-1, 1, 1);
                    blaster.transform.localPosition = new Vector2(blasterPositionDefault.x * -1, blasterPositionDefault.y);
                    blaster.transform.localScale = new Vector3(-1, -1, 1);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            rigidbody.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.transform.position, new Quaternion(0, 0, 0, 0));
            newBullet.transform.localScale = new Vector3(newBullet.transform.localScale.x * blaster.transform.localScale.x,
                                                        newBullet.transform.localScale.y * blaster.transform.localScale.y,
                                                        newBullet.transform.localScale.z * blaster.transform.localScale.z);

            Bullet tempBullet = newBullet.GetComponent<Bullet>();
            switch (generalDirection)
            {
                case Direction.up:
                    newBullet.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
                    tempBullet.bulletDirection = transform.up;
                    break;

                case Direction.down:
                    newBullet.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
                    tempBullet.bulletDirection = -transform.up;
                    break;

                case Direction.right:
                    newBullet.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                    tempBullet.bulletDirection = transform.right;
                    break;

                case Direction.left:
                    newBullet.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                    tempBullet.bulletDirection = -transform.right;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal") * walkSpeed * walkSpeedMinimizerTemp, 0), ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        grounded = true;
        walkSpeedMinimizerTemp = 1;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
        walkSpeedMinimizerTemp = walkSpeedMinimizer;
    }
}
