using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MouseTest : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TestMouseUI();
        }
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

}
