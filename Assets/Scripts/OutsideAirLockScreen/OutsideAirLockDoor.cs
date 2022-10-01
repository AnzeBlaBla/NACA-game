using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideAirLockDoor : MonoBehaviour
{
    private Animator animator;

   void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start(){
        this.openDoor();
        ScreenManager.Instance.DisableButtons();
    }

    void openDoor(){
        animator.SetTrigger("TrOpenAirLock");
    }
}
