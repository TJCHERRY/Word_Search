using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Data;
using TMPro;

public class WordPlacement : MonoBehaviour
{
    //fields
    private List<string> wordList;
    private readonly uint gridSize=12;
    private List<TMP_InputField> inputFields;
    private char[,] wordGrid;
    private List<Vector2Int> startIndex;

    public GameObject inputFieldHolder;

    public enum Direction {Left,Down,Up,Right};

    //properties
    public char[,] WordGrid { get => wordGrid;}
    public List<Vector2Int> StartIndex { get => startIndex; set => startIndex = value; }
    public List<TMP_InputField> InputFields { get => inputFields; set => inputFields = value; }


    private void OnEnable()
    {
        GameManager.Instance.OnRestartPressed += OnReset;
    }
    // Start is called before the first frame update
    void Start()
    {
        wordGrid = new char[gridSize, gridSize];
        StartIndex = new List<Vector2Int>();
        wordList = new List<string>();
        inputFields = new List<TMP_InputField>();
        
        Debug.Log(wordGrid.Length);



    }


    // Update is called once per frame
    void Update()
    {

       
    }

    /// <summary>
    /// fit words in 2dArray grid
    /// </summary>
    /// <param name="_gridSize"></param>
    public void PlaceWord(int _gridSize)
    {
        StoreWordsInList();
        for (int n = 0; n < wordList.Count; n++)
        {
            bool wordPlaced = false;
           
                for(int i = 0; i < _gridSize; i++)
                {
                    for(int j = 0; j < _gridSize; j++)
                    {
                        StartIndex.Add(new Vector2Int(i, j));
                    }
                }


            while(StartIndex.Count>0)
            {
                int randomIndex = Mathf.CeilToInt(UnityEngine.Random.Range(-1f, StartIndex.Count - 1));
                Vector2Int temp = StartIndex[randomIndex];



                foreach (int currentDir in Enum.GetValues(typeof(Direction)))
                {
                    if(IsPlaceAvailableInGivenDirection(currentDir, wordList[n], temp.x, temp.y))
                    {
                        wordPlaced = true;
                        Debug.Log(wordList[n] + " || " + temp);
                        Data.WordProperties tempWordProperty = new Data.WordProperties(wordList[n],temp,(Direction)currentDir);
                        GameManager.Instance.WordPropertyList.Add(tempWordProperty);
                        break;
                        
                    }
                    else
                    {
                        if(currentDir== Enum.GetValues(typeof(Direction)).Length-1)
                            StartIndex.RemoveAt(randomIndex);
                        
                    }

                }

                if (wordPlaced)
                    break;


            }

               
           
        }
        
    }
    /// <summary>
    /// checks if there are available spaces to fit the word in the required direciton
    /// </summary>
    /// <param name="currentDirection"></param>
    /// <param name="currentWord"></param>
    /// <param name="placementIndex_X"></param>
    /// <param name="placementIndex_Y"></param>
    /// <returns></returns>
    public bool IsPlaceAvailableInGivenDirection(int currentDirection,string currentWord,int placementIndex_X, int placementIndex_Y)
    {
        bool placeAvailable=true;
        switch (currentDirection)
        {

            case (int)Direction.Right:

                for(int i = 0,j = placementIndex_Y; i < currentWord.Length; i++, j++)
                {
                    if (j >= gridSize)
                        return false;
                        

                    if(wordGrid[placementIndex_X,j]!= '\0')
                    {
                        if (wordGrid[placementIndex_X, j] != currentWord[i])
                        {
                            placeAvailable = false;
                            break;
                        }
                    }

                }

                if (placeAvailable)
                {
                    for(int i=0,j=placementIndex_Y; i < currentWord.Length; i++, j++)
                    {
                        wordGrid[placementIndex_X, j] = currentWord[i];
                        Debug.Log(wordGrid[placementIndex_X, j] +","+"("+placementIndex_X+","+j+")");

                    }
                    return true;
                }

                break;
            case (int)Direction.Left:
                for (int i = 0, j = placementIndex_Y; i < currentWord.Length; i++, j--)
                {
                    if (j <= 0)
                        return false;


                    if (wordGrid[placementIndex_X, j] != '\0')
                    {
                        if (wordGrid[placementIndex_X, j] != currentWord[i])
                        {
                            placeAvailable = false;
                            break;
                        }
                    }

                }
                if (placeAvailable)
                {
                    for (int i = 0, j = placementIndex_Y; i < currentWord.Length; i++, j--)
                    {
                        wordGrid[placementIndex_X, j] = currentWord[i];
                        Debug.Log(wordGrid[placementIndex_X, j] + "," + "(" + placementIndex_X + "," + j + ")");

                    }
                    return true;
                }
                break;

            case (int)Direction.Down:
                for (int i = 0, j = placementIndex_X; i < currentWord.Length; i++,j++)
                {
                    if (j >= gridSize)
                        return false;

                    if (wordGrid[j, placementIndex_Y] != '\0')
                    {
                        if(wordGrid[j,placementIndex_Y] != currentWord[i])
                        {
                            placeAvailable = false;
                            break;
                        }
                    }

                }

                if (placeAvailable)
                {
                    for (int i = 0, j = placementIndex_X; i < currentWord.Length; i++, j++)
                    {
                        wordGrid[j,placementIndex_Y] = currentWord[i];
                        Debug.Log(wordGrid[j, placementIndex_Y] + "," + "(" + j + "," + placementIndex_Y + ")");

                    }
                    return true;
                }
                break;

            case (int)Direction.Up:
                    
                for(int i = 0, j = placementIndex_X; i < currentWord.Length; i++, j--)
                {
                    if (j <= 0)
                        return false;

                    if(wordGrid[j,placementIndex_Y]!= '\0')
                    {
                        if(wordGrid[j,placementIndex_Y] != currentWord[i])
                        {
                            placeAvailable = false;
                            break;
                        }
                    }
                }
                if (placeAvailable)
                {
                    for (int i = 0, j = placementIndex_X; i < currentWord.Length; i++, j--)
                    {
                        wordGrid[j, placementIndex_Y] = currentWord[i];
                        Debug.Log(wordGrid[j, placementIndex_Y] + "," + "(" + j + "," + placementIndex_Y + ")");

                    }
                    return true;
                }
                break;


        }

        return placeAvailable;
    }

    /// <summary>
    /// store words from input fields into the word list
    /// </summary>
    public void StoreWordsInList()
    {
        List<string> _wordsFromInput= new List<string>();
        inputFields.AddRange(inputFieldHolder.transform.GetComponentsInChildren<TMP_InputField>());

        foreach(TMP_InputField i in inputFields)
        {
            if (!string.IsNullOrEmpty(i.text))
                wordList.Add(i.text);
        }

    }

    public void OnReset()
    {
       
        foreach(TMP_InputField tmp in inputFields)
        {
            tmp.text = string.Empty;
        }
        wordList.Clear();
        startIndex.Clear();
        inputFields.Clear();
        Array.Clear(wordGrid,0,wordGrid.Length );
    }

    public void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRestartPressed -= OnReset;
        }
    }

}
