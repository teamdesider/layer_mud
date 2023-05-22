using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItemController : MonoBehaviour
{
    public GameObject animGB = null;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animGB = gameObject.transform.GetChild(0).gameObject;
        animator = animGB.GetComponent<Animator>();
    }

    public void SetDisappear()
    {
        animator.SetTrigger("disappear");
    }

    public void RevertDisappear()
    {
        animator.ResetTrigger("disappear");
    }
}
