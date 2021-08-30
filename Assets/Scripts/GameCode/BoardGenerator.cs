using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class BoardGenerator : MonoBehaviour
{
    //public fields
    //public Button generateBoardButton;
    public GameObject GridBlockPrefab;
    public UnityAction OnBoardGenerate;
    public TextMeshProUGUI wordCheck;
    public WordPlacement wordPlacement;
    public GameObject gridBlockPanel;
    public bool canSelectWord;

    //private fields
    private List<TextMeshProUGUI> blockLetterText;
    private List<GameObject> gridBlock;
    

    //properties
    public List<GameObject> GridBlock { get => gridBlock; set => gridBlock = value; }


    // Start is called before the first frame update

    private void Awake()
    {
        if(wordPlacement==null)
            wordPlacement = GetComponent<WordPlacement>();

        blockLetterText = new List<TextMeshProUGUI>();
        gridBlock= new List<GameObject>();
    }
    void OnEnable()
    {
        GameManager.Instance.OnGenerateBoard += GenerateBoard;
        GameManager.Instance.OnRestartPressed += OnReset;
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.Instance.currentState == GameManager.GameState.PanelB)
           // GenerateBoard();
    }

    public void GenerateBoard()
    {
        int count = 0;
        for(int i=0;i< wordPlacement.WordGrid.Length; i++)
        {
            GameObject go = Instantiate(GridBlockPrefab) as GameObject;
            AddEventTriggerComponent(go);
            MouseDownTriggerSetup(go);
            EntryTriggerSetup(go);
            MouseUpTriggerSetup(go);

           
            TextMeshProUGUI letter = go.GetComponentInChildren<TextMeshProUGUI>();
            blockLetterText.Add(letter);
            go.transform.SetParent(gridBlockPanel.transform);
            go.transform.localScale = new Vector3(1f, 1f, 1f);
            gridBlock.Add(go);
            //generateBoardButton.interactable = false;
        }

        foreach(char c in wordPlacement.WordGrid)
        {
            if (c != '\0')
                blockLetterText[count].text = c.ToString();
            else
            {
                blockLetterText[count].text = char.ToString((char)Random.Range(065, 090));
            }
                
            count++;
        }


    }

 /// <summary>
 /// Adds event trigger component to the grid buttons
 /// </summary>
 /// <param name="go"></param>
    public void AddEventTriggerComponent(GameObject go)
    {
        if (!go.GetComponent<EventTrigger>())
        {
            go.AddComponent <EventTrigger>();
        }
    }

    public void EntryTriggerSetup(GameObject go)
    {

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { DisplaySelectedWord(go); });
        go.GetComponent<EventTrigger>().triggers.Add(entry);
    }

    public void MouseDownTriggerSetup(GameObject go)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { SelectWord(go,true); });
        entry.callback.AddListener((eventData) => { DisplaySelectedWord(go); });
        go.GetComponent<EventTrigger>().triggers.Add(entry);
    }

    public void MouseUpTriggerSetup(GameObject go)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { SelectWord(go,false); });
        entry.callback.AddListener((eventData) => { wordCheck.text = string.Empty; });
        entry.callback.AddListener((eventData) => { GameManager.Instance.OnWordFound.Invoke(); });
        go.GetComponent<EventTrigger>().triggers.Add(entry);
    }

    private void DisplaySelectedWord(GameObject go)
    {
        if (canSelectWord)
        {
            string letter = go.GetComponentInChildren<TextMeshProUGUI>().GetParsedText();
            if (wordCheck.text.Length <=15)
            {
                wordCheck.text += letter;
            }
            
        }
    }

    

    private void SelectWord(GameObject go, bool _canSelect)
    {
        canSelectWord = _canSelect;
    }

    public void Test()
    {
        Debug.Log("Generate Board");
    }

    public void OnReset()
    {
        foreach (GameObject go in gridBlock)
        {
            Destroy(go);
        }

        foreach(TextMeshProUGUI tmp in blockLetterText)
        {
            tmp.text = string.Empty;
        }

        blockLetterText.Clear();

    }


    public void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRestartPressed -= OnReset;
            GameManager.Instance.OnGenerateBoard -= GenerateBoard;
        }
    }


}
