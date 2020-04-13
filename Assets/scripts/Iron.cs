using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : MonoBehaviour
{
    Rigidbody2D ironRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        ironRigidBody = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D other)
    {
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
            checkGround(upHits, up);
            checkGround(downHits, down);
            checkGround(rightHits, right);
            checkGround(leftHits, left);
        }
    }

    void checkGround(RaycastHit2D[] hits, Vector3 checkDir)
    {
        hits = Physics2D.RaycastAll(transform.position, checkDir, 2f);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                PlayerController.instance.jumpOnBox = true;
            }
        }
    }
}
