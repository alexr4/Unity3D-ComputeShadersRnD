using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSBaseController : MonoBehaviour
{
    public ComputeShader shader;
    RenderTexture tex;
    int kernelHandle;
    // Start is called before the first frame update
    void Start()
    {
        
        RunShader();
    }

    // Update is called once per frame
    void Update()
    {
        shader.SetFloat("time", (float)Time.time);
        shader.Dispatch(kernelHandle, 256/8, 256/8, 1);
    }


    void RunShader()
    {
        kernelHandle = shader.FindKernel("CSMain");

        tex = new RenderTexture(256,256,24);//8*8*32*32 → total amount of pixels
        tex.enableRandomWrite = true;
        tex.Create();

        shader.SetTexture(kernelHandle, "Result", tex);
    }

     void OnGUI(){
        int textureSize = 256;
        GUI.DrawTexture(new Rect(0, 0, textureSize, textureSize), tex);
    }
}
