using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowfield3DController : MonoBehaviour{
    [Header("FlowField Params")]
    public ComputeShader computeShader;
    private ComputeBuffer computeBuffer;
    private int kernel;
    private int sizeGroup = 8;
    private int widthGroup = 16;
    public int cubSize = 512;

    [Range(0.5f, 1.5f)]
    public float noiseScale = 1.0f;
    [Range(0.0f, 1.0f)]
    public float timeScale = 0.25f;
    private Vector3 crd; //cols, rows, depths
    private float[] data;

    [Header("Debug")]
    [Tooltip("Defines the cell offset for gizmo drawing. This will only parts of the cells but increase Editor performances")]
    public int GizmosOffset = 2;
    [Tooltip("Define the anchor point of the cube as a normalized xyz vector")]
    public Vector3 anchor;

    void Start(){
        initComputeBuffer();
    }

    void LateUpdate(){
        computeShader.SetFloat("noiseScale", noiseScale);
        computeShader.SetFloat("time", Time.time * timeScale);
        computeShader.Dispatch(kernel, widthGroup, widthGroup, widthGroup);
        computeBuffer.GetData(data);
    }

     void OnDestroy(){
         computeBuffer.Release();
         computeBuffer.Dispose();
     }

     void OnDrawGizmosSelected(){
         Gizmos.color = Color.red;
         if(data != null && data.Length > 0){
             for(int i=0; i<crd.x*crd.y*crd.z; i+=GizmosOffset){
                Vector3 pos = i1DToi3D(i, crd);
                Vector3 dir = new Vector3(data[i * 3 + 0],
                                            data[i * 3 + 1],
                                            data[i * 3 + 2]);
                pos += transform.position;
                pos -= Vector3.Scale(crd, anchor);
                Debug.DrawLine(pos, pos + dir.normalized * 0.5f, Color.red);
            //  Gizmos.DrawWireSphere(pos, 0.025f);
             }
         }
     }

     private void initComputeBuffer(){
         //Init Compute Buffer for a float3 array
        widthGroup = cubSize / sizeGroup;
        computeBuffer = new ComputeBuffer(sizeGroup * sizeGroup * sizeGroup *  widthGroup * widthGroup * widthGroup, 3 * sizeof(float));
        crd = new Vector3(sizeGroup * widthGroup, sizeGroup * widthGroup, sizeGroup * widthGroup);
        //Init Compute Shader, get kernel and bind variables
        kernel = computeShader.FindKernel("CSMain");
        computeShader.SetVector("crd", crd);
        computeShader.SetFloat("time", Time.time * timeScale);
        computeShader.SetFloat("noiseScale", noiseScale);
        computeShader.SetBuffer(kernel, "Result", computeBuffer); //Bind the buffer to the shader by defining it as the buffer used for the Result StructBuffer
        computeShader.Dispatch(kernel, widthGroup, widthGroup, widthGroup);

        data = new float[sizeGroup * widthGroup * sizeGroup * widthGroup * sizeGroup * widthGroup * 3];
        computeBuffer.GetData(data);

        Debug.Log("Compute Shader use to compute "+(sizeGroup * widthGroup * sizeGroup * widthGroup * sizeGroup * widthGroup)+
                 " datas into "+(sizeGroup*sizeGroup*sizeGroup)+" threads of "+(widthGroup)+" groups"+
                 " which gives us "+(data.Length/3)+" datas for our flow field on a cube of "+(sizeGroup * widthGroup)+"×"+(sizeGroup * widthGroup)+"×"+(sizeGroup * widthGroup));
     }

     public Vector3 i1DToi3D(int index){
         return i1DToi3D(index, crd);
     }

     private Vector3 i1DToi3D(int index, Vector3 crd){
        int z = index / (int)(crd.x * crd.y);
        index -= (int)(z * crd.x * crd.y);
        int y = index / (int)crd.x;
        int x = index % (int)crd.x;
        return new Vector3(x, y, z);
    }

    private int i3DToi1D(Vector3 xyz, Vector3 crd){
        return (int)((xyz.z * crd.x * crd.y) + (xyz.y * crd.x) + xyz.x);
    }
    public int i3DToi1D(Vector3 xyz){
        return i3DToi1D(xyz, crd);
    }
}