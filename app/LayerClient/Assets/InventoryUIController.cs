using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Concurrent;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

public class InventoryUIController : MonoBehaviour
{

    public GameObject inventoryGB;
    public GameObject pagePrefab;
    public GameObject itemPrefab;
    private int columnLimit = 4;
    public Dictionary<int, GameObject> pageDict = new();
    public ObservableConcurrentDictionary<string, GameObject> inventoryUIDict = new();
    public List<GameObject> inventoryUIList = new();
    public Sprite imgSptite;



    // Start is called before the first frame update
    void Start()
    {
        //inventoryDict.CollectionChanged += Test;
        //inventoryDict.PropertyChanged+= Test2;
        imgSptite = Resources.Load<Sprite>("Test-item");
    }



    // Update is called once per frame
    void Update()
    {

    }

    //public void Test(object sender, NotifyCollectionChangedEventArgs e)
    //{
    //    Debug.Log(e.OldStartingIndex);
    //    foreach(string key in inventoryDict.Keys)
    //    {
    //        Debug.Log(key);
    //    }
    //}

    //public void Test2(object sender, PropertyChangedEventArgs e)
    //{
    //    Debug.Log(e.PropertyName);
    //}

    public void InitPageGB(Inventory inventory)
    {

        var itemCount = inventory.inventoryDict.Count;

        int pageSize = itemCount / columnLimit;

        int restSize = itemCount % columnLimit;

        if (restSize != 0)
        {
            pageSize += 1;
        }



        int pageIndex = 0;
        for (int i = 0; i < itemCount; i += columnLimit)
        {
            GameObject pageGB = Instantiate(pagePrefab, new Vector3(inventoryGB.transform.position.x + Screen.width * pageIndex, inventoryGB.transform.position.y, inventoryGB.transform.position.z), Quaternion.identity);
            pageGB.transform.SetParent(inventoryGB.transform);
            pageDict.Add(pageIndex, pageGB);
            pageIndex++;


            List<InventoryItem> pageItems = inventory.inventoryList.Skip(i).Take(columnLimit).ToList();

            foreach (InventoryItem invenItem in pageItems)
            {
                GameObject pageItemGB = Instantiate(itemPrefab);

                pageItemGB.transform.SetParent(pageGB.transform);

                GameObject gb = pageItemGB.transform.Find("Amount").gameObject;

                pageItemGB.transform.Find("Image").gameObject.GetComponent<Image>().sprite = ItemDictionary.itemsDict[invenItem.itemTypeId].iconSprite;

                gb.GetComponent<TextMeshProUGUI>().text = invenItem.amount.ToString();

                inventoryUIDict.Add(invenItem.itemTypeId, pageItemGB);
                inventoryUIList.Add(pageItemGB);

            }



        }

        inventoryGB.GetComponent<PageSwipeController>().totalPageCount = pageSize;

    }

    public void UpdateUIInventory(InventoryItem inventoryItem)
    {
        if (inventoryUIDict.ContainsKey(inventoryItem.itemTypeId))
        {
            if (inventoryItem.amount == 0)
            {
                inventoryUIDict.Remove(inventoryItem.itemTypeId);
                inventoryUIList.FirstOrDefault(obj => obj == inventoryUIDict[inventoryItem.itemTypeId]);

                DestroyInventoryItem(inventoryItem);
            }
            else
            {

                inventoryUIDict[inventoryItem.itemTypeId].transform.Find("Amount").gameObject.GetComponent<TextMeshProUGUI>().text = inventoryItem.amount.ToString();

            }
        }
        else
        {

            GameObject inventoryItemGB = GenerateInventoryItemGB(inventoryItem);
            inventoryUIDict.Add(inventoryItem.itemTypeId, inventoryItemGB);
            inventoryUIList.Add(inventoryItemGB);
            //GenerateInventoryItemGB(inventoryItem);

            //Init the gb and added into page gb
            //inventoryUIDict.Add(inventoryItem.itemId, inventoryItem);
            RefreshUIInventory();
        }

    }

    //private void DeleteItem(GameObject itemGB) {
    //    inventoryUIDict.FirstOrDefault(obj => obj.Value == itemGB);
    //    inventoryUIList.Remove(itemGB.itemId);
    //}

    private void DestroyInventoryItem(InventoryItem inventoryItem)
    {
        //GameObject tempInventoryItem = inventoryDict[inventoryItem.itemId];
        Destroy(inventoryUIDict[inventoryItem.itemTypeId]);
        inventoryUIDict.Remove(inventoryItem.itemTypeId);

    }

    private GameObject GenerateInventoryItemGB(InventoryItem inventoryItem)
    {
        GameObject pageItemGB = Instantiate(itemPrefab);

        GameObject gb = pageItemGB.transform.Find("Amount").gameObject;

        pageItemGB.transform.Find("Image").gameObject.GetComponent<Image>().sprite = ItemDictionary.itemsDict[inventoryItem.itemTypeId].iconSprite;

        gb.GetComponent<TextMeshProUGUI>().text = inventoryItem.amount.ToString();

        return pageItemGB;

    }

    public void RefreshUIInventory()
    {
        int itemCount = inventoryUIDict.Count();

        int pageSize = itemCount / columnLimit;

        int restSize = itemCount % columnLimit;

        if (restSize != 0)
        {
            pageSize += 1;
        }


        int pageIndex = 0;
        for (int i = 0; i < itemCount; i += columnLimit)
        {
            if (pageDict.TryGetValue(pageIndex, out GameObject pageGB))
            {

                List<GameObject> pageItems = inventoryUIList.Skip(i).Take(columnLimit).ToList();

                foreach (GameObject inventoryItemGB in pageItems)
                {
                    if (inventoryItemGB.transform.IsChildOf(pageGB.transform))
                    {

                        continue;
                    }
                    else
                    {

                        inventoryItemGB.transform.SetParent(pageGB.transform);
                    }

                }
            }
            else
            {
                //add new page instance gb
                GameObject newPageGB = Instantiate(pagePrefab, new Vector3(inventoryGB.transform.position.x + Screen.width * pageIndex, inventoryGB.transform.position.y, inventoryGB.transform.position.z), Quaternion.identity);
                newPageGB.transform.SetParent(inventoryGB.transform);

                List<GameObject> pageItems = inventoryUIList.Skip(i).Take(columnLimit).ToList();


                foreach (GameObject inventoryItemGB in pageItems)
                {
                    inventoryItemGB.transform.SetParent(newPageGB.transform);


                }
            }
            pageIndex++;
        }
        inventoryGB.GetComponent<PageSwipeController>().totalPageCount = pageSize;





    }
    enum DictionaryAction
    {
        Add, Remove, Update
    }
    private void UpdateUIInventoryDictAndLst(string action, InventoryItem inventoryItem)
    {

    }




}

