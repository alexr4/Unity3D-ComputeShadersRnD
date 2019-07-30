using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T01CSController : MonoBehaviour
{
    public ComputeShader cs;
    // Start is called before the first frame update
    void Start()
    {
        int[] arrayOfInt = new int[]{0, 1, 2, 3, 4, 5, 6, 7}; //create a simple 1D inte Array

        // Create ah Buffer with arrayOfInt.Length element which a size of int for each one
        ComputeBuffer buffer = new ComputeBuffer(8 * 8, sizeof(int)); //2D kernel
        buffer.SetData(arrayOfInt); // Bind the array to the buffer

        int kernel = cs.FindKernel("CSMain"); //Return the kernel (function) which need to be executed
        cs.SetBuffer(kernel, "Result", buffer); //Bind the buffer to the shader by defining it as the buffer used for the Result StructBuffer
        cs.Dispatch(kernel, 1, 1, 1); //Execute the kernel and defines the numpber of groupe. Hardware limit is 1024 and you need to use group for more datas

        int[] data = new int[8 * 8];//Create an array to retreive the data 
        buffer.GetData(data);// Retreive the data

        for(int i=0; i<data.Length; i++){
            Debug.Log(data[i]); //Debug
        }

        buffer.Release();//Release the buffer for next frame
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
