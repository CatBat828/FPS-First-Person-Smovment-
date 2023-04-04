using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBlob : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.tag == "Player")
            {
                print("finish.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
    }

}
