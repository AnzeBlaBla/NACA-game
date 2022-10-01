using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomScreenManager : MonoBehaviour
{
    //Add curtain to the scene
    public GameObject curtain;
    private bool curtainOpen = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Function to close the curtain with sliding animation
    public void CloseCurtain()
    {
        //Set the animation trigger to close the curtain
        this.curtainOpen = false;
        this.curtain.GetComponent<Animator>().SetTrigger("TrClose");
    }

    public void OpenCurtain()
    {
        //Set the animation trigger to close the curtain
        this.curtainOpen = true;
        this.curtain.GetComponent<Animator>().SetTrigger("TrOpen");
    }

    public void OpenCloseCurtain()
    {
        //Set the animation trigger to close the curtain
        if(this.curtainOpen){
            this.CloseCurtain();
        }else{
            this.OpenCurtain();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
