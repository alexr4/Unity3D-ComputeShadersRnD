using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowfield3DController : MonoBehaviour{
    public ComputeShader computeShader;
    private ComputeBuffer computeBuffer;
    private int kernel;
    private int sizeGroup = 8;
    private int widthGroup = 16;
    private float[] data;

    void Start(){
        computeBuffer = new ComputeBuffer(sizeGroup * sizeGroup * sizeGroup *  widthGroup * widthGroup * widthGroup, 3 * sizeof(float));
        kernel = computeShader.FindKernel("CSMain");
        computeShader.SetVector("crd", new Vector3(sizeGroup * widthGroup, sizeGroup * widthGroup, sizeGroup * widthGroup));
        computeShader.SetFloat("time", Time.time);
        computeShader.SetBuffer(kernel, "Result", computeBuffer); //Bind the buffer to the shader by defining it as the buffer used for the Result StructBuffer
        computeShader.Dispatch(kernel, widthGroup, widthGroup, widthGroup);

        data = new float[sizeGroup * widthGroup * sizeGroup * widthGroup * sizeGroup * widthGroup * 3];
        computeBuffer.GetData(data);

        Debug.Log("Compute Shader use to compute "+(sizeGroup * widthGroup * sizeGroup * widthGroup * sizeGroup * widthGroup)+
                 " datas into "+(sizeGroup*sizeGroup*sizeGroup)+" threads of "+(widthGroup)+" groups"+
                 " which gives us "+(data.Length/3)+" datas for our flow field on a cube of "+(sizeGroup * widthGroup)+"×"+(sizeGroup * widthGroup)+"×"+(sizeGroup * widthGroup));
        // for(int i=0; i<data.Length; i+=3){
        //     Debug.Log(i+
        //               " x: "+data[i + 0]+
        //               " y: "+data[i + 1]+
        //               " z: "+data[i + 2]); 
        // }
    }

    void LateUpdate(){
        computeShader.SetFloat("time", Time.time);
        computeShader.Dispatch(kernel, widthGroup, widthGroup, widthGroup);
        computeBuffer.GetData(data);
        // Debug.Log(" x: "+data[1 + 0]+
        //           " y: "+data[1 + 1]+
        //           " z: "+data[1 + 2]); 
    }

     void OnDestroy(){
         computeBuffer.Release();
         computeBuffer.Dispose();
     }
}