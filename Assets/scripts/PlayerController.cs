using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Outlet
    public static PlayerController instance;
    
    Rigidbody2D player_rigidBody;
    Rigidbody2D iron_rigidBody;
    public Transform aimPivot;
    public GameObject projectilePrefab;
    SpriteRenderer sprite;
    Animator animator;

    public float maxEnergy = 1f;
    public float currentEnergy;
    public EnergyBar energyBar;  
    

    // State Tracking

    public int jumpsLeft;
    public float timesleft;
    public bool isPaused = false; // for menu
    
    void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player_rigidBody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        iron_rigidBody = GameObject.Find("iron").GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        currentEnergy = maxEnergy;
        energyBar.SetMaxEnergy(maxEnergy);

    }

    void FixedUpdate()
    {
        animator.SetFloat("Speed", player_rigidBody.velocity.magnitude);
        if (player_rigidBody.velocity.magnitude > 0)
        {
            animator.speed = player_rigidBody.velocity.magnitude / 3f;
        }
        else
        {
            animator.speed = 1f;
        }
    }

    // Update is called once per frame
    void Update()

    {
        if (isPaused)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            MenuController.instance.Show();
        }
        // move left, 4f based on the frame rate on my laptop
        if (Input.GetKey(KeyCode.A))
        {
            player_rigidBody.AddForce(Vector2.left * 4f);
            sprite.flipX = true;
        }

        // move right

        if (Input.GetKey(KeyCode.D))
        {
            player_rigidBody.AddForce(Vector2.right * 4f);
            sprite.flipX = false;
        }

        // jump
        if (Input.GetKey(KeyCode.Space))
        
        {
            if(jumpsLeft > 0)
            {
                jumpsLeft--;
                player_rigidBody.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
            }
        }
        animator.SetInteger("JumpsLeft", jumpsLeft);

        // rocket jump
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

        // move pivot

        //Vector3 mousePosition = Input.mousePosition;
        //Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        //Vector3 dicrectionFromPlayerToMouse = mousePositionInWorld - transform.position;

        //float radiansToMouse = Mathf.Atan2(dicrectionFromPlayerToMouse.y, dicrectionFromPlayerToMouse.x);
        //float angleToMouse = radiansToMouse * 180f / Mathf.PI;

        //aimPivot.rotation = Quaternion.Euler(0, 0, angleToMouse);


       

        // Shoot

        //if (Input.GetMouseButtonDown(0))
        //{
        //    GameObject newProjectile = Instantiate(projectilePrefab);
        //    newProjectile.transform.position = transform.position;
        //    newProjectile.transform.rotation = aimPivot.rotation;
        //}
        // spown an iron
        
        
            
        

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
                currentEnergy -= Time.deltaTime;
                energyBar.SetEnergy(currentEnergy);
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
                    jumpsLeft = 1;
                    timesleft = 1;
                    currentEnergy = maxEnergy;
                    energyBar.SetEnergy(currentEnergy);
                }
            }
        }

        
    }
}
