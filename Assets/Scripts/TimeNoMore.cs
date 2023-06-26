using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeNoMore : MonoBehaviour
{
    private float t = 0f; //time
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject deverf = GameObject.FindGameObjectsWithTag("Finish")[0];
        deverf.GetComponent<Timererer>().stopped = true;

        t += Time.deltaTime;
        if (t >= 10f) // time check
        {
            Application.Quit();
        }
    }
}
