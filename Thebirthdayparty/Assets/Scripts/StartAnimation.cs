using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.Rendering;

public class StartAnimation : MonoBehaviour
{
    new Animation animation;
    new Collider collider;
    Volume volume;
    AnimationClip clip;
    void Start()
    {   
        
        collider = GetComponent<Collider>();
        animation = gameObject.AddComponent<Animation>();
        clip = Resources.Load("StartAnim/StartAnimation") as AnimationClip;
        animation.clip = clip;
        animation.AddClip(clip,clip.name);

        PlayerController.Instance.interactable = false;
        collider.enabled = false;
        InteractHintManager.Instance.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(PlayStartAnimation());
    }
    IEnumerator PlayStartAnimation(){
        yield return null;
        animation.Play();
        Invoke(nameof(ResetControl),clip.length);
        yield return null;
    }
    void ResetControl(){
        PlayerController.Instance.interactable = true;
        collider.enabled = true;
        InteractHintManager.Instance.gameObject.SetActive(true);
        GameSaveManager.Instance.currentSave.firstPlayed = false;
        GameSaveManager.Instance.SaveGame();
        Destroy(this);
    }
}
