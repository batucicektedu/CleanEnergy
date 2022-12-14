using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTiling : MonoBehaviour
{
    private Material mat;
    public float speed;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        mat.mainTextureOffset -= new Vector2(0, Time.deltaTime * speed);
    }
}
