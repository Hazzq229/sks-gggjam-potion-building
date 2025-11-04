using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public float countdown = 10f;
    public Slider timeslide;
    public delegate void OnTimeUp();
public event OnTimeUp TimeUpEvent;

    // Start is called before the first frame update
    void Start()
    {
        timeslide.maxValue = countdown;
        timeslide.minValue = 0;
        timeslide.value = countdown;
    }

    // Update is called once per frame
    void Update()
    {
        coutdownting();
    }
    public void coutdownting()
    {
        countdown -= Time.deltaTime;

        timeslide.value = countdown;
        if (countdown <= 0)
    {
        countdown = 0;
        TimeUpEvent?.Invoke();
        enabled = false;
    }
    }
}
