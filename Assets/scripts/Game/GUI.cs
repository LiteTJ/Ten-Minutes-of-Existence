using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI : MonoBehaviour
{
    public GameObject[] gemKeys;
    public GameObject canvas;
    public GameObject messagePosition;
    public GameObject timeoutCanvas;
    public GameObject pausedCanvas;

    public GameController gameController;
    public GameObject textContainerPrefab;
    public TextMeshProUGUI timeLeft;

    private Color32 timeoutColour = new Color32(192, 22, 22, 255);

    private Color32[] keyColours =
    {
        //Hex colour picker from https://www.hexcolortool.com/
        new Color32(189, 86, 86, 255),
        new Color32(211, 162, 69, 255),
        new Color32(79, 211, 69, 255),
        new Color32(69, 136, 211, 255),
        new Color32(128, 69, 211, 255)
    };

    private GameObject CreateText(string _text)
    {
        GameObject textContainer = Instantiate(textContainerPrefab, messagePosition.transform.position, Quaternion.identity, canvas.transform);

        GameObject textObject = textContainer.transform.GetChild(0).gameObject;
        TextMeshProUGUI myText = textObject.GetComponent<TextMeshProUGUI>();

        myText.text = _text;

        return textContainer;
    }

    private void UpdateTextColour()
    {
        if(gameController.GetTimeOut())
        {
            timeLeft.color = timeoutColour;
        } else
        {
            timeLeft.color = Color.white;
        }
    }

    private void UpdateTimeLeft()
    {
        int time = (int)Mathf.Floor(gameController.GetTimeLeft());
        string minuteTime = Convert.ToString(Mathf.Floor(time / 60));
        string secondTime = Convert.ToString(time % 60);

        if (secondTime.Length < 2)
        {
            secondTime = "0" + secondTime;
        }

        timeLeft.text = "TIME LEFT - " + minuteTime + ":" + secondTime;

        UpdateTextColour();
    }

    private void UpdateGemstoneIcons()
    {
        bool[] collection = gameController.level.gemstoneCollected;

        for(int i = 0; i < collection.Length; i++)
        {
            if(collection[i] == true)
            {
                Image img = gemKeys[i].GetComponent<Image>();
                img.color = keyColours[i];
            }
        }
    }

    private void Update()
    {
        UpdateTimeLeft();
        UpdateGemstoneIcons();
    }

    public IEnumerator ShowMessage(string msg, float duration)
    {
        GameObject textContainer = CreateText(msg);

        yield return new WaitForSeconds(duration);

        textContainer.GetComponent<Animator>().SetTrigger("FadeOut");
    }

    public IEnumerator ShowTimeoutCanvas(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        timeoutCanvas.SetActive(true);
    }

    public void ShowPausedCanvas()
    {
        pausedCanvas.SetActive(true);
    }

    public void HidePausedCanvas()
    {
        pausedCanvas.SetActive(false);
    }
}
