using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PageSwipeController : MonoBehaviour, IDragHandler, IEndDragHandler
{

    private Vector2 panelLocation;
    //public GameObject pagePrefab;
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;
    private int currentChild=0;
    public int totalPageCount = 5;
    //public Dictionary<int,GameObject> pageDict = new();
    public GameObject inventoryGameObject;

    // Start is called before the first frame update
    void Start()
    {
        panelLocation = inventoryGameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        inventoryGameObject.transform.position = panelLocation - new Vector2(difference, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {

        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;

        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if (percentage > 0 && currentChild< totalPageCount - 1)
            {
                newLocation += new Vector3(-Screen.width, 0, 0);
                currentChild++;
            }else if (percentage < 0 && currentChild>0)
            {
                newLocation += new Vector3(Screen.width, 0, 0);
                currentChild--;
            }
            StartCoroutine(SmoothMove(inventoryGameObject.transform.position,newLocation,easing));
            panelLocation = newLocation;

        }
        else
        {
            StartCoroutine(SmoothMove(inventoryGameObject.transform.position, panelLocation, easing));

        }

        IEnumerator SmoothMove(Vector3 startpos,Vector3 endpos,float seconds)
        {
            float t = 0f;
            while (t <= 1.0)
            {
                t += Time.deltaTime / seconds;
                inventoryGameObject.transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
        }

    }

}
