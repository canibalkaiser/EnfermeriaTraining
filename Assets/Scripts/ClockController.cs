using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ClockController : MonoBehaviour
{
    [Header("Required Components")]
    public TextMeshProUGUI textHourMinute;
    public TextMeshProUGUI textSeconds;
    public TextMeshProUGUI textDayMonthYear;

    private void Start()
    {
        InvokeRepeating(nameof(GetTimeEverySecond), 1, 1);
    }

    private void GetTimeEverySecond()
    {
        textHourMinute.text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
        textSeconds.text = DateTime.Now.Second.ToString();
        textDayMonthYear.text = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
    }

}
