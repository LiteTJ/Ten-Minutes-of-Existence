using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameController : MonoBehaviour
{
    public GameObject startIndicator;
    public GameObject transitionController;

    public GUI gui;
    public MyCam myCam;
    public Clock clock;
    public Player player;
    public Level level;
    public Light2D globalLight;

    [HideInInspector] public bool isPaused;

    private readonly float timeAllowed = 60 * 10f;
    private float startTime;
    private float lightIntensity = 0.36f;
    private bool messageShown;
    private bool timeoutShown;

    private float GetTimePassed()
    {
        return Time.time - startTime;
    }

    public float GetTimeLeft()
    {
        float timeLeft = timeAllowed - GetTimePassed();

        if(timeLeft < 0)
        {
            return 0f;
        } else
        {
            return timeLeft;
        }
    }

    public bool GetTimeOut()
    {
        return GetTimeLeft() == 0f;
    }

    private int GetTimeLeftInMinutes()
    {
        return (int) Mathf.Ceil(GetTimeLeft() / 60);
    }

    private Vector2 GetMousePos()
    {
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mousePos3D.x, mousePos3D.y);

        return mousePos;
    }

    public void Pause()
    {
        isPaused = true;
        gui.ShowPausedCanvas();
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        isPaused = false;
        gui.HidePausedCanvas();
        Time.timeScale = 1f;
    }

    private void Start()
    {
        player.CreateBodySegments();
        level.ResetGemstones();
        level.CreateEnemies();
        player.Respawn(startIndicator);
        globalLight.intensity = lightIntensity;
        messageShown = false;
        timeoutShown = false;

        startTime = Time.time;
    }

    private void UpdatePlayer()
    {
        player.FlickerLight();

        //We add 1 because the head does not count as a body segment
        if (GetTimeLeftInMinutes() < player.GetBodyCount() + 1 && GetTimeLeftInMinutes() != 0f)
        {
            player.RemoveBodySegment();
        }

        if (player.GetState().Equals("DEAD"))
        {
            player.Respawn(startIndicator);
        }

        if(player.GetWithinGateArea())
        {
            if(level.AllGemstonesCollected())
            {
                Animator animator = level.gate.GetComponent<Animator>();
                animator.SetTrigger("OpenGate");
            } else
            {
                if(!messageShown)
                {
                    StartCoroutine(gui.ShowMessage("Not all gems are collected.", 4f));

                    messageShown = true;
                }
            }
        }

        if(player.GetWithinEndArea())
        {
            Debug.Log("Player reaches the end");

            transitionController.GetComponent<Animator>().SetTrigger("Finish");
        }
    }

    private void UpdateGemstones()
    {
        GameObject gem = player.GetNewlyCollectedGemstone();
        if (gem != null)
        {
            for (int i = 0; i < level.gemstones.Length; i++)
            {
                //Find the correct gem reference
                if (ReferenceEquals(gem, level.gemstones[i]))
                {
                    //Verify this gem has not been collected yet
                    if (!level.gemstoneCollected[i])
                    {
                        level.gemstoneCollected[i] = true;

                        //Gem can fade out
                        Animator animator = gem.GetComponent<Animator>();
                        animator.SetTrigger("FadeOut");
                    }
                }
            }

            player.ResetNewlyCollectedGemstone();
        }
    }

    private void Update()
    {
        if(GetTimeOut())
        {
            //Time runs out
            if(!timeoutShown)
            {
                StartCoroutine(gui.ShowTimeoutCanvas(2.6f));
                timeoutShown = true;
            }
        } else
        {
            //Update clock
            clock.UpdateHands(GetTimeLeft());

            //Update player
            UpdatePlayer();

            //Update gemstones
            UpdateGemstones();

            //Other updates
            if(Input.GetKey(KeyCode.Space) && !isPaused)
            {
                Pause();
            }
        }
    }

    private void FixedUpdate()
    {
        if(!GetTimeOut())
        {
            //Update player
            player.MovePlayer(GetMousePos());
            player.UpdateBodySegmentsPos();

            //Update enemies
            level.UpdateEnemies(player.gameObject);

            //Update camera
            myCam.UpdatePosition();
        }

    }
}
