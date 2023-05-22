using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlayerController : MonoBehaviour
{
    public GameObject crosshairGB;
    public Camera arCamera;
    public GameStateController gameStateController;
    private readonly int collectSpendEnergy = 1;
    public CrosshairController crosshairController;


    // Start is called before the first frame update
    void Start()
    {
        //crosshairCenter = crosshairGB.transform.position;
        //Debug.Log(arCamera);
    }

    // Update is called once per frame
    void Update()
    {
        LockHandler();

    }


    private void LockHandler()
    {

        Ray ray = arCamera.ScreenPointToRay(crosshairController.GetCrossHairScreenPosition());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Collectable") || hit.collider.gameObject.CompareTag("Token"))
            {
                crosshairController.UpdateLocked(true);

            }

        }
        else
        {
            crosshairController.UpdateLocked(false);

        }
    }

    // Get the legal touch position, false is illegal, true for legal
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;

                bool isOverUI = touchPosition.IsPointOverUIGB();

                return !isOverUI;
            }
        }

        touchPosition = default;

        return false;
    }

    private bool UpdateEnergy(int updateEnergy)
    {
        gameStateController.UpdateEnergy(-updateEnergy);
        return true;
        return false;
    }

    bool UpdateToken()
    {
        gameStateController.UpdateToken(1);
        return true;
    }

    bool UpdateInventory(GameObject collectGB)
    {

        string itemTypeId = collectGB.GetComponent<ItemAttributeScript>().itemTypeId;
        gameStateController.UpdateInventory(itemTypeId, InventoryActions.collect);
        return true;
    }

    bool ProcessAfterCollected(GameObject gb)
    {
        //Destroy(gb);
        CollectableItemController collectItemController = gb.GetComponent<CollectableItemController>();
        collectItemController.SetDisappear();
        return true;


    }

    bool PendingHandler(GameObject gb)
    {
        gameStateController.AddNewPending(gb);
        return true;
        return false;
    }

    void PendingCollect(GameObject gb)
    {
        CollectableItemController collectItemController = gb.GetComponent<CollectableItemController>();
        if (collectItemController.isActiveAndEnabled)
        {
            collectItemController.SetDisappear();

        }
    }



}
