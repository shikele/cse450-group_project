using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBounds : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            PlayerController.instance.GetComponent<CapsuleCollider2D>().isTrigger = true;
            PlayerController.instance.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 100f);
            PlayerController.instance.death = true;
        }
    }
}
