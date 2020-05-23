using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialGUI : MonoBehaviour
{
    public TutorialController tutorialController;

    public GameObject canvas;
    public GameObject textContainerPrefab;
    public GameObject leftIndicator;
    public GameObject rightIndicator;
    public GameObject coverLeft;
    public GameObject coverRight;

    private GameObject currentText;
    private int noSlides = 14;
    private int slideIndex;
    private bool canSwitchSlides;

    private void FocusLeft()
    {
        coverLeft.SetActive(false);
        coverRight.SetActive(true);
    }

    private void FocusRight()
    {
        coverLeft.SetActive(true);
        coverRight.SetActive(false);
    }

    private GameObject CreateText(string _text, string pos)
    {
        Vector3 posReference;

        if(pos.Equals("LEFT"))
        {
            posReference = leftIndicator.transform.position;
        } else
        {
            posReference = rightIndicator.transform.position;
        }

        GameObject textContainer = Instantiate(textContainerPrefab, posReference, Quaternion.identity, canvas.transform);

        GameObject textObject = textContainer.transform.GetChild(0).gameObject;
        TextMeshProUGUI myText = textObject.GetComponent<TextMeshProUGUI>();

        myText.text = _text;

        return textContainer;
    }

    private void SlideOne() { currentText = CreateText("This is you." + "\n" + "(Space to continue)", "LEFT"); }
    private void SlideTwo() { currentText = CreateText("Use mouse to navigate.", "LEFT"); }
    private void SlideThree() { currentText = CreateText("In this world, you will encounter many creatures.", "LEFT"); }
    private void SlideFour() { currentText = CreateText("These floating creatures are harmless.", "RIGHT"); }
    private void SlideFive() { currentText = CreateText("These, however, are harmful.", "RIGHT"); }
    private void SlideSix() { currentText = CreateText("When you contact them, you will respawn at the start position.", "RIGHT"); }
    private void SlideSeven() { currentText = CreateText("The starfish are also harmless.", "RIGHT"); }
    private void SlideEight() { currentText = CreateText("These creatures will explode when you get close to them. Watch out!", "RIGHT"); }
    private void SlideNine() { currentText = CreateText("You have only 10 minutes to live.", "LEFT"); }
    private void SlideTen() { currentText = CreateText("Within that time, you must collect 5 gems.", "LEFT"); }
    private void SlideEleven() { currentText = CreateText("This unlocks the gate at the very top-right corner...", "LEFT"); }
    private void SlideTwelve() { currentText = CreateText("...unlocking the passage to enlightenment.", "LEFT"); }
    private void SlideThirteen() { currentText = CreateText("That is your final destination.", "LEFT"); }
    private void SlideFourteen() { currentText = CreateText("Good luck.", "LEFT"); }

    private IEnumerator ShowSlide(int index)
    {
        canSwitchSlides = false;

        if (index == 1) SlideOne();
        if (index == 2) SlideTwo();
        if (index == 3) SlideThree();
        if (index == 4) SlideFour();
        if (index == 5) SlideFive();
        if (index == 6) SlideSix();
        if (index == 7) SlideSeven();
        if (index == 8) SlideEight();
        if (index == 9) SlideNine();
        if (index == 10) SlideTen();
        if (index == 11) SlideEleven();
        if (index == 12) SlideTwelve();
        if (index == 13) SlideThirteen();
        if (index == 14) SlideFourteen();

        if(index == 1)
        {
            FocusLeft();
        }

        //Create example enemies and show/hide regions
        if(index == 4)
        {
            tutorialController.CreateFloaterEnemies();
            FocusRight();
        }

        if(index == 5)
        {
            tutorialController.testLevel.DestroyAllEnemies();
            tutorialController.CreateFloaterEnemiesHarmful();
        }

        if(index == 7)
        {
            tutorialController.testLevel.DestroyAllEnemies();
            tutorialController.CreateStars();
        }

        if (index == 8)
        {
            tutorialController.testLevel.DestroyAllEnemies();
            tutorialController.CreateBombEnemies();
        }

        if (index == 9)
        {
            FocusLeft();
            tutorialController.testLevel.DestroyAllEnemies();
        }

        //Estimates the time taken for text to fade in
        yield return new WaitForSeconds(1f);

        canSwitchSlides = true;
    }

    private IEnumerator NextSlide()
    {
        canSwitchSlides = false;

        if (currentText != null)
        {
            currentText.GetComponent<Animator>().SetTrigger("FadeOut");
        }

        yield return new WaitForSeconds(1.1f);

        slideIndex++;

        if(slideIndex > noSlides)
        {
            tutorialController.EndTutorial();
        } else
        {
            StartCoroutine(ShowSlide(slideIndex));
        }

    }

    private void Start()
    {
        slideIndex = 1;
        StartCoroutine(ShowSlide(slideIndex));
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space) && canSwitchSlides)
        {
            StartCoroutine(NextSlide());
        }
    }
}
