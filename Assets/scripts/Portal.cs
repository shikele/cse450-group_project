using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && SceneManager.GetActiveScene().name == "level1")
        {
            SceneManager.LoadScene("level2");
        }
        if (other.gameObject.GetComponent<PlayerController>() && SceneManager.GetActiveScene().name == "level2")
        {
            SceneManager.LoadScene("level3");
        }

    }
}
