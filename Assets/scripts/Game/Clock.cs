using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject HourHand;
    public GameObject MinuteHand;
    public GameObject SecondHand;

    public void UpdateHands(float timeLeft)
    {
        float minuteAngle = timeLeft * 360 / 60 / 60;

        float secondAngle = minuteAngle * 60;
        secondAngle = Mathf.Floor(secondAngle / 6) * 6;

        float hourAngle = minuteAngle / 60;

        SecondHand.transform.eulerAngles = new Vector3(0, 0, secondAngle);
        MinuteHand.transform.eulerAngles = new Vector3(0, 0, minuteAngle);
        HourHand.transform.eulerAngles = new Vector3(0, 0, hourAngle);
    }
}
