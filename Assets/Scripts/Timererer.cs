using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timererer : MonoBehaviour
{
    public float time;
    private TextMeshProUGUI texter;
    public bool stopped;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        texter = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped)
            return;
        time += Time.deltaTime;
        texter.text = (((float)(int)(time*100))/100).ToString();
    }
}
