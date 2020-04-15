using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Outlet
    public static PlayerController instance;

    Rigidbody2D player_rigidBody;
    Rigidbody2D iron_rigidBody;
    Collider2D iron_collider;
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
    private float maxSpeed = 10f;
    private int rocket_flag = 0;
    private int box_flag = 0;
    private int jump_flag = 0;
    public bool jumpOnBox = false;
    public bool death = false;


    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        death = false;
        player_rigidBody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        iron_rigidBody = GameObject.Find("iron").GetComponent<Rigidbody2D>();
        iron_collider = GameObject.Find("iron").GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        currentEnergy = maxEnergy;
        energyBar.SetMaxEnergy(maxEnergy);

    }




    void FixedUpdate()

    {
        animator.SetFloat("Speed", player_rigidBody.velocity.magnitude);
        animator.SetBool("Death", death);
        if (player_rigidBody.velocity.magnitude > 0)
        {
            animator.speed = player_rigidBody.velocity.magnitude / 3f;
        }
        else
        {
            animator.speed = 1f;
        }
        if (isPaused)
        {
            return;
        }
        if (player_rigidBody.velocity.magnitude > maxSpeed)
        {
            Vector2.ClampMagnitude(player_rigidBody.velocity, maxSpeed);
        }
        //print(player_rigidBody.velocity.magnitude.ToString());
        if (Input.GetKey(KeyCode.Escape))
        {
            MenuController.instance.Show();
        }
        // move left, 4f based on the frame rate on my laptop
        if (Input.GetKey(KeyCode.A))
        {
            player_rigidBody.AddForce(Vector2.left * 20f);
            sprite.flipX = true;
        }

        // move right

        if (Input.GetKey(KeyCode.D))
        {
            player_rigidBody.AddForce(Vector2.right * 20f);
            sprite.flipX = false;
        }

        // jump
        if (Input.GetKey(KeyCode.Space) && jump_flag == 1)

        {
            if (jumpsLeft > 0)
            {
                jumpsLeft--;
                player_rigidBody.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
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
                    player_rigidBody.AddForce(dicrectionFromPlayerToIron.normalized * 20f);
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

        resetIron();

        

        if (Input.GetKey(KeyCode.F))
        {
            var iron = GameObject.Find("iron");
            if (iron)
            {
                var pos = GameObject.Find("iron").transform.position;
                Vector3 dicrectionFromPlayerToIron = transform.position - pos;
                dicrectionFromPlayerToIron.z = 0;
                if (Mathf.Abs(dicrectionFromPlayerToIron.x) <= 0.8)
                {
                    dicrectionFromPlayerToIron.x = 0;
                }
                float dist = Vector3.Distance(pos, transform.position);
                if (dist >= 1.2)
                {
                    iron_rigidBody.AddForce(dicrectionFromPlayerToIron.normalized * 10f);
                    iron_rigidBody.constraints = RigidbodyConstraints2D.None;
                }
                else
                {
                    iron_rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
                }
            }



        }

        if (Input.GetKey(KeyCode.W))
        {
            if (timesleft > 0)
            {
                player_rigidBody.AddForce(Vector2.up * 10f);
                timesleft -= Time.deltaTime;
                currentEnergy -= Time.deltaTime;
                energyBar.SetEnergy(currentEnergy);
            }

        }

        if (Input.GetKey(KeyCode.G))
        {
            print(rocket_flag.ToString());
            if (rocket_flag == 1 && box_flag == 1)
            {
                Vector2 v = new Vector2(15f, 200f);
                iron_rigidBody.AddForce(v);
            }

        }
        if (player_rigidBody.velocity.magnitude > maxSpeed)
        {
            player_rigidBody.velocity = player_rigidBody.velocity.normalized * maxSpeed;
        }

        if (GameObject.Find("canon"))
        {
            if (iron_collider.IsTouching(GameObject.Find("canon").GetComponent<Collider2D>()))
            {
                box_flag = 1;
            }
            else
            {
                box_flag = 0;
            }
        }
        
        

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.up, 0.7f);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Button"))
            {
                jump_flag = 1;
            }
            else
            {
                jump_flag = 0;
            }


        }



    }

    private void resetIron()
    {
        var iron = GameObject.Find("iron");
        if (iron)
        {
            var pos = GameObject.Find("iron").transform.position;
            float dist = Vector3.Distance(pos, transform.position);
            if (dist >= 0.8)
            {
                iron_rigidBody.constraints = RigidbodyConstraints2D.None;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
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
                    jump_flag = 1;
                }



            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumpOnBox)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Button"))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.up, 0.7f);

                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Button"))
                    {
                        jumpsLeft = 1;
                        timesleft = 1;
                        currentEnergy = maxEnergy;
                        energyBar.SetEnergy(currentEnergy);
                        jump_flag = 1;
                        rocket_flag = 1;
                    }


                }

            }
        }
        
    }
}
