using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T05CSController : MonoBehaviour
{
    struct VecMatPair{
        public Vector3 point;
        public Matrix4x4 matrix;
    }
 
    public ComputeShader shader;
     ComputeBuffer buffer;
    VecMatPair[] data;
    int kernel;
    // Start is called before the first frame update
    void Start()
    {
        
        RunShader();
    }

    // Update is called once per frame
    void Update()
    {
        shader.Dispatch(kernel, data.Length, 1, 1);
        Output();
    }


    void RunShader()
    {
        data = new VecMatPair[5];
        for(int i=0; i<data.Length; i++){
            Matrix4x4 mat = Matrix4x4.identity;
            Vector3 point = Random.insideUnitSphere;
            VecMatPair vmp = new VecMatPair();
            vmp.point = point;
            vmp.matrix = mat;
            data[i] = vmp;
        }

        buffer = new ComputeBuffer(data.Length, (3 + 4 * 4) * sizeof(float));
        buffer.SetData(data);

        kernel = shader.FindKernel("Multiply");
        shader.SetBuffer(kernel, "dataBuffer", buffer);
    }

    void Output(){
        VecMatPair[] output = new VecMatPair[5];
        buffer.GetData(output);

        for(int i=0; i<output.Length; i++){
            Debug.Log(i+": "+output[i].point);
        }
    }
    
    void OnDestroy(){
        //Delet buffer after simulation
        buffer.Dispose();
    }
}
