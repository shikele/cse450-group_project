using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : MonoBehaviour
{
    // State tracking
    bool onWater = false;
    bool direction = false;
    int dirCount = 50;

    Rigidbody2D ironRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        ironRigidBody = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print(onWater);
        if (onWater)
        {

            ironRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            ironRigidBody.SetRotation(0);
            ironRigidBody.drag = 5;
            if (direction)
            {
                if(dirCount < 10)
                {
                    ironRigidBody.AddForce(transform.up * 0.8f, ForceMode2D.Impulse);
                    direction = false;
                    if(dirCount == 0)
                    {
                        print("Oscillate");
                        dirCount = 45;
                    }
                    
                }
                
                dirCount--;
            }
            else
            {
                if (dirCount < 10 )
                {
                    ironRigidBody.AddForce(-transform.up * 0.8f, ForceMode2D.Impulse);
                    direction = true;
                    if (dirCount == 0)
                    {
                        print("Oscillate");
                        dirCount = 45;
                    }
                }

                dirCount--;
            }
        }
        else
        {
            ironRigidBody.drag = 0;
            ironRigidBody.constraints = RigidbodyConstraints2D.None;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        //print(other.gameObject.layer);
        PlayerController.instance.jumpOnBox = false;
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Vector3 up = transform.up;
            Vector3 down = -transform.up;
            Vector3 right = transform.right;
            Vector3 left = -transform.right;
            RaycastHit2D[] upHits = Physics2D.RaycastAll(transform.position, up, 2f);
            RaycastHit2D[] downHits = Physics2D.RaycastAll(transform.position, down, 2f);
            RaycastHit2D[] rightHits = Physics2D.RaycastAll(transform.position, right, 2f);
            RaycastHit2D[] leftHits = Physics2D.RaycastAll(transform.position, left, 2f);
            checkGround(upHits, up, 2f);
            checkGround(downHits, down, 2f);
            checkGround(rightHits, right, 2f);
            checkGround(leftHits, left, 2f);
        }
        if (other.gameObject.layer == 12)
        {
            Vector3 up = transform.up;
            Vector3 down = -transform.up;
            Vector3 right = transform.right;
            Vector3 left = -transform.right;
            float l = 1;
            RaycastHit2D[] upHits = Physics2D.RaycastAll(transform.position, up, l);
            RaycastHit2D[] downHits = Physics2D.RaycastAll(transform.position, down, l);
            RaycastHit2D[] rightHits = Physics2D.RaycastAll(transform.position, right, l);
            RaycastHit2D[] leftHits = Physics2D.RaycastAll(transform.position, left, l);
            checkGround(upHits, up, l);
            checkGround(downHits, down, l);
            checkGround(rightHits, right, l);
            checkGround(leftHits, left, l);
        }
    }

    void checkGround(RaycastHit2D[] hits, Vector3 checkDir, float checkLength)
    {
        hits = Physics2D.RaycastAll(transform.position, checkDir, checkLength);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                onWater = false;
                PlayerController.instance.jumpOnBox = true;
            }
            if (hit.collider.gameObject.layer == 12)
            {
                onWater = true;
                PlayerController.instance.jumpOnBox = true;
            }
        }
    }
}
