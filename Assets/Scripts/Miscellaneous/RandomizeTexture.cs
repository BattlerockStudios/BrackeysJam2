using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RandomizeTexture : MonoBehaviour
{
    public Texture2D[] textureArray;

    private Renderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_renderer.material.SetTexture("_MainTex", textureArray[Random.Range(0, textureArray.Length)]);
    }
}
