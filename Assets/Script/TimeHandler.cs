using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    [SerializeField]
    private float _countdown = 10f;
    [SerializeField]
    public Slider timeSlide;

    // Public property to encapsulate the countdown value.
    // Setting this value will update the slider and (re)enable the timer.
    public float Countdown
    {
        get => _countdown;
        set
        {
            _countdown = Mathf.Max(0f, value);
            if (timeSlide != null)
            {
                timeSlide.maxValue = _countdown;
                timeSlide.value = _countdown;
            }
        }
    }
    public delegate void OnTimeUp();
    public event OnTimeUp TimeUpEvent;
    void Awake()
    {
        // Try to auto-bind the Slider if it wasn't assigned in the Inspector
        if (timeSlide == null)
        {
            timeSlide = GetComponent<Slider>();
            if (timeSlide == null)
                timeSlide = GetComponentInChildren<Slider>();

            if (timeSlide == null)
                Debug.LogWarning($"[TimeHandler] No Slider assigned or found. Timer will run but UI won't update. ({name})");
        }
        enabled = false;
    }

    void Update()
    {
        TickCountdown();
    }
    private void TickCountdown()
    {
        if (_countdown <= 0f) return;

        _countdown -= Time.deltaTime;
        _countdown = Mathf.Max(0f, _countdown);

        if (timeSlide != null)
            timeSlide.value = _countdown;

        if (_countdown <= 0f)
        {
            enabled = false;
            TimeUpEvent?.Invoke();
        }
    }
    public void ResetTimer(float seconds)
    {
        Debug.Log("Resetting timer");
        if (timeSlide != null)
            timeSlide.minValue = 0f;
        Countdown = seconds;
    }
}
