using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureUpdater : MonoBehaviour
{
    ImageSelector imageSelector;

    // Start is called before the first frame update
    void Start()
    {
        var obj = GameObject.Find("ImageSelector");
        imageSelector = obj.GetComponent<ImageSelector>();
        imageSelector.OnLoaded = updateTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateTexture(Texture2D tex)
    {
		var renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = tex;
        Debug.Log("テクスチャ更新反映完了");
    }
}
