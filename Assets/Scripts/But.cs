using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class But : MonoBehaviour
{
    private float t=0f; //inpt
    void Update()
    {
        t += Time.deltaTime;
        if (t >= 10f) // unite
        {
            SceneManager.LoadScene(1); // sgas
        }
    }
    public void Clicker()
    { // unuse
        Debug.Log("DEVRT ROCKS GET OTU THE CKAG");
    }
}
