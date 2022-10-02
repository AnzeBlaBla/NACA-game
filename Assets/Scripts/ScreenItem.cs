using UnityEngine;

[CreateAssetMenu(fileName = "Screen", menuName = "NACA/Screen", order = 1)]
public class ScreenItem : ScriptableObject
{
    public string sceneName;
    public string displayName;

    public Sprite backgroundImage;

}