using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionControllerTutorial : MonoBehaviour
{
    public void OnFadeOut()
    {
        SceneManager.LoadScene(0);
    }
}
