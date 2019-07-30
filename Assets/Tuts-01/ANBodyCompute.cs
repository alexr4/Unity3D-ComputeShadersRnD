using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
public class ANBodyCompute : MonoBehaviour {
    public ComputeShader shader;
    public static ComputeBuffer posBuffer;
    public static ComputeBuffer velBuffer;
    private int sizeGroup = 8;
    public int widthGroup;

    void Start(){
        posBuffer = new ComputeBuffer(sizeGroup * sizeGroup * widthGroup * widthGroup, 3 * sizeof(float), ComputeBufferType.Default);
        velBuffer = new ComputeBuffer(sizeGroup * sizeGroup * widthGroup * widthGroup, 3 * sizeof(float), ComputeBufferType.Default);

        //this global buffers apply to every shader with these buffer defined. They are set into the GPU
        shader.SetInt("widthGroup", widthGroup); //Bind value to shader
        Shader.SetGlobalBuffer(Shader.PropertyToID("position"), posBuffer);
        Shader.SetGlobalBuffer(Shader.PropertyToID("velocity"), velBuffer);

        float[] posData = new float[sizeGroup * sizeGroup * widthGroup * widthGroup * 3];
        float[] velData = new float[sizeGroup * sizeGroup * widthGroup * widthGroup * 3];

        for(int i=0; i<sizeGroup * sizeGroup * widthGroup * widthGroup; i++){
            float randx = (0.5f - (i%256) / 256.0f) * 16.0f;
            float randy = (0.5f - (i/256) / 256.0f) * 16.0f;
            float randz = UnityEngine.Random.Range(-1f, 1f);//(0.5f + (i%256) / 256.0f) * 16.0f;

            posData[i * 3 + 0] = randx;
            posData[i * 3 + 1] = randy;
            posData[i * 3 + 2] = randz; //this is a 2D simulation for now

            // If position was a vector from origin (0,0), this would turn it 90 degrees - e.g. create circular motion.
            velData[i * 3 + 0] = randy * 0.01f;
            velData[i * 3 + 1] = -randx * 0.01f;
            velData[i * 3 + 2] = -randz * 0.01f;
        }

        Debug.Log(posBuffer.count + " " + posData.Length);
        //Set the buffer at start
        posBuffer.SetData(posData);
        velBuffer.SetData(velData);
    }

    void LateUpdate(){
        //Update the kernel for 256 threads
        shader.Dispatch(shader.FindKernel("CSMain"), widthGroup, widthGroup, 1);
    }

    void OnDestroy(){
        //Delet buffer after simulation
        posBuffer.Dispose();
        velBuffer.Dispose();
    }
}