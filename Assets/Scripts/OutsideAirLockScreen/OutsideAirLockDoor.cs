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
        ScreenManager.Instance.DisableButtons();
        StartCoroutine(this.openDoor());
    }

    IEnumerator openDoor(){
        animator.SetTrigger("TrOpenAirLock");
        yield return new WaitForSeconds(4);
        yield return StartCoroutine(AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().PlayAnimation("maximizing"));
        AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().unlockAstronaut();
    }
}
