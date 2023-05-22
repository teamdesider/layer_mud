using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    public AudioSource audioSource = null;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic() {
        Debug.Log("play music");
        audioSource.enabled = true;
        audioSource.Play();
    }


    //TODO this is not setting, may be deprecated
    public void SelfDestroy() {
        Destroy(gameObject.transform.parent.gameObject);
    }

    //TODO End of disappear for token now, others are not set
    public void PendingDestroy()
    {
        Invoke("SetGameObjectInactive",1f);
        //gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void SetGameObjectInactive() {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
