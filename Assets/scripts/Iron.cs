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
}
