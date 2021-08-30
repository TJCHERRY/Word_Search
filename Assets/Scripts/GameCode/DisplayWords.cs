using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWords : MonoBehaviour
{
    public GameObject wordPrefab;
    public WordPlacement wordGridPlacement;
    public BoardGenerator boardGenerator;

    // Start is called before the first frame update
    private void OnEnable()
    {
        GameManager.Instance.OnWordFound += CompareWords;
        GameManager.Instance.OnRestartPressed += OnReset;
        DisplayWordList();
        GameManager.Instance.wordCount = transform.childCount;
    }
    void Start()
    {
        

    }


    /// <summary>
    /// displays List of words on the right side
    /// </summary>
    public void DisplayWordList()
    {
        for(int i = 0; i < GameManager.Instance.WordPropertyList.Count; i++)
        {
            GameObject go = Instantiate(wordPrefab) as GameObject;
            go.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.Instance.WordPropertyList[i].word;
            go.transform.SetParent(this.transform);

        }
        
    }

    public  void CompareWords()
    {
        if (boardGenerator.wordCheck != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<TMPro.TextMeshProUGUI>()?.GetParsedText() == boardGenerator.wordCheck.GetParsedText())
                {
                    GameManager.Instance.wordCount--;
                    transform.GetChild(i).GetComponent<TMPro.TextMeshProUGUI>().color = Color.grey;
                    transform.GetChild(i).GetComponent<TMPro.TextMeshProUGUI>().alpha = 0.5f;
                    transform.GetChild(i).GetComponent<TMPro.TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Strikethrough;

                }
            }
        }
    }

    public void OnReset()
    {
        foreach(Transform go in transform)
        {
            Destroy(go.gameObject);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnWordFound -= CompareWords;
            GameManager.Instance.OnRestartPressed -= OnReset;
        }
    }


}
