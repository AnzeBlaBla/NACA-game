using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomScreenManager : MonoBehaviour
{
    public GameObject curtain;

    void Start()
    {
        var _animation = curtain.GetComponent<Animation>();

    var clip = new AnimationClip();
    var curve = AnimationCurve.Linear(0, -1056, 20, 0);
    clip.SetCurve("Curtain", typeof(Transform), "anchoredPosition.x", curve);

    curve = AnimationCurve.Linear(0, 0, 20, 0);
    clip.SetCurve("Curtain", typeof(Transform), "anchoredPosition.y", curve);

    clip.name = "Curtain"; // set name
    clip.legacy = true; // change to legacy
    
    _animation.clip = clip; // set default clip
    _animation.AddClip(clip, clip.name); // add clip to animation component

    _animation.Play("Curtain"); // play animation
    }
}
