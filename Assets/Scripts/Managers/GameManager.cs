using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //--------------------------------------------------------
    // Game variables

    public static int Level = 0;
    public static int lives = 3;

    public enum GameState { Init, Game, Dead, Scores }
    public static GameState gameState;

    private GameObject robot;

    private GameGUINavigation gui;

    static public int score;

   
   

    public float SpeedPerLevel;

    //-------------------------------------------------------------------
    // singleton implementation
    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    //-------------------------------------------------------------------
    // function definitions

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }


    }

    void Start()
    {
        gameState = GameState.Init;
    }

    void OnLevelWasLoaded()
    {
        if (Level == 0) lives = 3;

        Debug.Log("Level " + Level + " Loaded!");

        ResetVariables();




        robot.GetComponent<PlayerController>().speed += Level * SpeedPerLevel / 2;
    }

    private void ResetVariables()
    {
        
     
        PlayerController.killstreak = 0;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void ResetScene()
    {
        ;

        robot.transform.position = new Vector3(15f, 11f, 0f);


        robot.GetComponent<PlayerController>().ResetDestination();


        gameState = GameState.Init;
        gui.H_ShowReadyScreen();

    }







    public void LoseLife()
    {
        lives--;
        gameState = GameState.Dead;

        // update UI too
        UIScript ui = GameObject.FindObjectOfType<UIScript>();
        Destroy(ui.lives[ui.lives.Count - 1]);
        ui.lives.RemoveAt(ui.lives.Count - 1);
    }

    public static void DestroySelf()
    {

        score = 0;
        Level = 0;
        lives = 3;
        Destroy(GameObject.Find("Game Manager"));
    }
}
