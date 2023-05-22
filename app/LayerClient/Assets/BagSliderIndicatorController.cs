using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class BagSliderIndicatorController : MonoBehaviour
{
    // Unity Engine Interface
    public GameObject sliderIndicator;
    public GameObject sliderIndicatorNode;
    private List<Button> sliderNodes = new();
    public GameObject inventoryGB;
    private readonly int nodeLimit = 2;
    private Color changedColor = new(0.78f,0.78f,0.78f,1f);


    // Start is called before the first frame update
    void Start()
    {
        EventTrigger eventTrigger = inventoryGB.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.EndDrag
        };
        entry.callback.AddListener((eventData) => {
            PointerEventData pointerEventData = eventData as PointerEventData;
            if (pointerEventData != null)
            {
                int direction = (pointerEventData.pressPosition.x - pointerEventData.position.x) <0 ? 0:1;

                SliderNodeHandler(direction);
            }

        });

        eventTrigger.triggers.Add(entry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSlideIndicator(int totalPageCount) {
        AddSliderNodes(totalPageCount);
    }

    private void AddSliderNodes(int totalPageCount)
    {
        //if (totalPageCount > 1)
        //{
        //    if (sliderNodes.Count==nodeLimit)
        //    {
        //        return;
        //    }

            for (int idx = sliderNodes.Count; idx < nodeLimit; idx++) {
                GameObject sliderNode = Instantiate(sliderIndicatorNode);
                sliderNode.transform.SetParent(sliderIndicator.transform);
                sliderNodes.Add(sliderNode.GetComponent<Button>());
            }
        //}
        //else
        //{
        //    sliderNodes.Add(Instantiate(sliderIndicatorNode).GetComponent<Button>());
        //}
    }

    private void DelSliderNode(int totalPageCount)
    {
        if (totalPageCount<nodeLimit) {
            switch (totalPageCount) {
                case 0:
                    sliderNodes.Clear();
                    break;
                case 1:
                    sliderNodes.RemoveAt(sliderNodes.Count - 1);
                    break;
            }
        }
    }

    public void UpdateSlideIndicator(int totalPageCount) {
        int sliderCount = sliderNodes.Count;
        switch (sliderCount)
        {
            case 0:
                AddSliderNodes(totalPageCount);
                break;
            case 1:
                if (totalPageCount > sliderCount)
                {
                    AddSliderNodes(totalPageCount);
                }
                else
                {
                    DelSliderNode(totalPageCount);
                }
                break;
            case 2:
                DelSliderNode(totalPageCount);
                break;
        }
    }


    public void SliderNodeHandler(int direction)
    {
        if (sliderNodes.Count == 0) {
            return;
        }
        Button sliderNodeBtn = sliderNodes[direction];
        ColorBlock colors = sliderNodeBtn.colors;
        Color defaultColor = colors.normalColor;
        float fadeDuration = colors.fadeDuration;

        StartCoroutine(ColorLerpCoroutine(defaultColor, fadeDuration, sliderNodeBtn));
    }

    private IEnumerator ColorLerpCoroutine(Color defaultColor, float fadeDuration,Button button)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            ColorBlock btncolors = button.colors;
            btncolors.normalColor = Color.Lerp(defaultColor, changedColor, t);
            button.colors = btncolors;
            yield return null;
        }

        // �ָ���ť��Ĭ����ɫ����ɫ����ʱ��
        yield return new WaitForSeconds(fadeDuration);
        ColorBlock colors = button.colors;
        colors.normalColor = defaultColor;
        button.colors = colors;
    }
}
