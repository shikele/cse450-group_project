using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Outlet

    Rigidbody2D player_rigidBody;
    Rigidbody2D iron_rigidBody;
    public Transform aimPivot;
    public GameObject projectilePrefab;

    
    
    

    //State Tracking

    public int jumpsleft;
    public float timesleft;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player_rigidBody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        iron_rigidBody = GameObject.Find("iron").GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()

    {
        //move left, 4f based on the frame rate on my laptop
        if (Input.GetKey(KeyCode.A))
        {
            player_rigidBody.AddForce(Vector2.left * 4f);
        }

        // move right

        if (Input.GetKey(KeyCode.D))
        {
            player_rigidBody.AddForce(Vector2.right * 4f);
        }

        //move pivot

        Vector3 mousePosition = Input.mousePosition;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 dicrectionFromPlayerToMouse = mousePositionInWorld - transform.position;

        float radiansToMouse = Mathf.Atan2(dicrectionFromPlayerToMouse.y, dicrectionFromPlayerToMouse.x);
        float angleToMouse = radiansToMouse * 180f / Mathf.PI;

        aimPivot.rotation = Quaternion.Euler(0, 0, angleToMouse);


       

        // Shoot

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newProjectile = Instantiate(projectilePrefab);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.rotation = aimPivot.rotation;
        }
        // spown an iron
        
        
            
            
        

       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(jumpsleft > 0)
            {
                jumpsleft--;
                player_rigidBody.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            }
        }

        //rocket jump
        if (Input.GetKey(KeyCode.C))
        {
            if (timesleft > 0)
            {
                var iron = GameObject.Find("iron");
                if (iron)
                {
                    var pos = GameObject.Find("iron").transform.position;
                    Vector3 dicrectionFromPlayerToIron = pos - transform.position;
                    dicrectionFromPlayerToIron.z = 0;
                    player_rigidBody.AddForce(dicrectionFromPlayerToIron.normalized * 4f);
                    timesleft -= Time.deltaTime;
                }
                
                
                
            }
            
        }

        if (Input.GetKey(KeyCode.X))
        {

            var iron = GameObject.Find("iron");
            if (iron) {
                var pos = GameObject.Find("iron").transform.position;
                Vector3 dicrectionFromPlayerToIron = transform.position - pos;
                dicrectionFromPlayerToIron.z = 0;
                if (Mathf.Abs(dicrectionFromPlayerToIron.x) <= 0.8){
                    dicrectionFromPlayerToIron.x = 0;
                }
                iron_rigidBody.AddForce(dicrectionFromPlayerToIron.normalized * 1.5f);
            }
                


        }

        if (Input.GetKey(KeyCode.W))
        {
            if (timesleft > 0)
            {
                player_rigidBody.AddForce(Vector2.up * 4f);
                timesleft -= Time.deltaTime;
            }

        }


    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.up, 0.7f);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    jumpsleft = 2;
                    timesleft = 2;
                }
            }
        }

        
    }
}
