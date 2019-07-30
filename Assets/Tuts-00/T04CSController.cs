using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T04CSController : MonoBehaviour
{
    public ComputeShader cs;
    private RenderTexture rt;
    // Start is called before the first frame update
    void Start()
    {
        rt = new RenderTexture(64, 64, 0);  
        rt.enableRandomWrite = true;
        rt.Create();

        cs.SetFloat("width", (float)rt.width);
        cs.SetFloat("height", (float)rt.height);
        cs.SetTexture(0, "Result", rt);
        cs.Dispatch(0, rt.width / 8, rt.height/8, 1); 
        /*
        To speed up the calculation, we want to use one thread per pixel. 
        For the above example, our texture is 64 * 64 = 4.096 pixels. 
        We will use threads groups as we learned before. 
        To calculate the number of groups needed, we can divide our texture width and height by the number of threads per group in each dimension.
        The dispatch function looks like shader.Dispatch(0, texture.width / group.x , texture.height / group.y, 1);
         */
    }

    void OnGUI(){
        int textureSize = 256;
        GUI.DrawTexture(new Rect(0, 0, textureSize, textureSize), rt);
    }

    // Update is called once per frame
    void OnDisable()
    {
        rt.Release();
    }
}
