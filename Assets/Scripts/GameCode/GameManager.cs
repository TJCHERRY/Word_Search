using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class GameManager : MonoBehaviour
{
    public enum GameState { PanelA, PanelB,GameOver };
    [HideInInspector]
    public GameState currentState;
    private static GameManager instance=null;
    private List<Data.WordProperties> wordPropertyList;
    public bool hasWon;
    public Action OnGameStart;
    public Action OnGenerateBoard;
    public Action OnWordFound;
    public Action OnGameOver;
    public Action OnOptionsPressed;
    public Action OnRestartPressed;
    [HideInInspector]
    public int wordCount= int.MaxValue;


   
    public static GameManager Instance {

        get
        {
            if (GameManager.instance == null)
            {

                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "GameManager";
                    instance=go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
                
            }
            return instance;
        }
            
            
    }

    public List<WordProperties> WordPropertyList { get => wordPropertyList; set => wordPropertyList = value; }
    private void OnEnable()
    {
        OnRestartPressed += OnReset;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
            Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        wordPropertyList = new List<WordProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wordCount <= 0)
        {
            hasWon = true;
            OnGameOver.Invoke();
        }
    }

    public void OnReset()
    {
        hasWon = false;
        wordCount = int.MaxValue;
        wordPropertyList.Clear();
    }
 
    private void OnDisble()
    {
        OnRestartPressed -= OnReset;
    }




}
