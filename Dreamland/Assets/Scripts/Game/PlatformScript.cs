using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

    public SpriteRenderer[] spriteRenderers;

    public void Init(Sprite sprite)
    {
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprite;
        }
    }
}
