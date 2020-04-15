using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathBound : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (other.gameObject.name == "iron")
        {
            Vector3 pos = new Vector3(3.41f, 1.46f, 0f);
            other.transform.position = pos;
            other.transform.rotation = Quaternion.identity;
        }
    }
}
