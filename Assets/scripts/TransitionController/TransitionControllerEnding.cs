using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionControllerEnding : MonoBehaviour
{
    public void OnFadeOut()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("ReturnMenu");
        }
    }
}
