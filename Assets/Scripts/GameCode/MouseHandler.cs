using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MouseHandler : MonoBehaviour,IPointerEnterHandler,IDragHandler,IEndDragHandler,IBeginDragHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{

    public LayerMask buttonLayer;
    private static List<GameObject> letterBlocks;
    private static bool isDragging=false;

    private BoardGenerator bg;

    public static List<GameObject> LetterBlocks { get => letterBlocks; set => letterBlocks = value; }

    // Start is called before the first frame update
    void Start()
    {
        letterBlocks = new List<GameObject>();
        if (bg == null)
            bg = FindObjectOfType<BoardGenerator>();

    }

    // Update is called once per frame
    void Update()
    {

       
    }

    


    private bool TestMouseUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult r in results)
        {
            if (r.gameObject.name.Contains("GridBlock"))
            {
                Debug.Log(r.gameObject.name);
                return true;
            }

        }

        return false;

    }
   

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        /*if (isDragging)
        {
            
            GameObject tmp = eventData.selectedObject;
            letterBlocks.Add(tmp);
            

        }*/

        if (isDragging)
        {
            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, results);
            foreach (RaycastResult r in results)
            {
                if (r.gameObject.name.Contains("GridBlock"))
                {
                    //Debug.Log(r.gameObject.GetComponentInChildren<TextMeshProUGUI>().GetParsedText());
                    letterBlocks.Add(r.gameObject);
                }

            }
        }

    }


    public void OnDrag(PointerEventData eventData)
    {
       
       

    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
    }

   
    public void OnPointerDown(PointerEventData eventData)
    {
        if(letterBlocks.Count>0)
                letterBlocks.Clear();

        isDragging = true;
       
        if (isDragging)
        {
            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, results);
            foreach (RaycastResult r in results)
            {
                if (r.gameObject.name.Contains("GridBlock"))
                {
                    //Debug.Log(r.gameObject.GetComponentInChildren<TextMeshProUGUI>().GetParsedText());
                    letterBlocks.Add(r.gameObject);
                }

            }
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        isDragging = false;
        string word=string.Empty;


        Debug.Log(letterBlocks.Count);

        if (letterBlocks.Count > 0)
        {
            for(int i=0;i<letterBlocks.Count;i++)
            {
                word += letterBlocks[i].GetComponentInChildren<TextMeshProUGUI>()?.GetParsedText();
            }

            bg.wordCheck.text = word;
            
        }

        letterBlocks.Clear();



    }
}


