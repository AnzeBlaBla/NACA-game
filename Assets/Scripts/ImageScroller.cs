using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageScroller : MonoBehaviour
{
    Material thisMaterial;
    public Vector2 textureOffset = new Vector2(0.1f, 0.1f);
    public float randomOffset = 0.05f;

    private void Awake()
    {
        // get material from plane
        thisMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {

        float x = Random.Range(-randomOffset, randomOffset);
        float y = Random.Range(-randomOffset, randomOffset);
        
        thisMaterial.mainTextureOffset += new Vector2(
            (textureOffset.x + x) / 100f,
            (textureOffset.y + y) / 100f
        );
    }
}
