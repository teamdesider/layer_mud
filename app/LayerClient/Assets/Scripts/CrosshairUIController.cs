using UnityEngine;

public class CrosshairUIController : MonoBehaviour
{

    public bool locked;
    public float changeSize;
    private RectTransform rectile;
    //[Range(50f, 250)]
    //public float size;
    private float currentSize;
    public float speed;
    public float lockedSize;
    public float restingSize;
    public GameObject crossHairGB;
    public Camera arCamera;
    public Vector3 crosshairOrigin;
    // Start is called before the first frame update
    void Start()
    {
        rectile = crossHairGB.GetComponent<RectTransform>();
        locked = false;
        currentSize = restingSize;

        float screenHeight = Screen.height;
        float targetY = screenHeight / 3 *2;
        Vector3 newPosition = rectile.position;
        newPosition.y = targetY;
        rectile.position = newPosition;
 
    }

    // Update is called once per frame
    void Update()
    {

        if (locked)
        {
            currentSize = Mathf.Lerp(currentSize, lockedSize, Time.deltaTime * speed);

        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);

        }

        rectile.sizeDelta = new Vector2(currentSize, currentSize);
    }

    public void ToggleLocked()
    {
        locked = !locked;
    }

    public void updateLocked(bool status)
    {
        locked = status;
    }

    public Vector3 GetCrossHairCenterPos()
    {
        return rectile.position;
    }

}
