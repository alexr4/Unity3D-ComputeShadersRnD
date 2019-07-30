using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class AStarCreatorRenderer : MonoBehaviour {
    public Material material;
    Matrix4x4[][] transformList;
    Mesh mesh;

    const int instanceMax = 1023;
    const int wantedInstances = 256 * 256; 
    const float starSize = 1.0f;

    void Start(){
        transformList = new Matrix4x4[wantedInstances / instanceMax][];
        MeshFilter mf = this.gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        mesh = new Mesh();
        mf.mesh = mesh;

        //create quad
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-starSize, -starSize, 0);
        vertices[1] = new Vector3(starSize, -starSize, 0);
        vertices[2] = new Vector3(-starSize, starSize, 0);
        vertices[3] = new Vector3(starSize, starSize, 0);

        mesh.vertices = vertices;

        int[] triangles = new int[2 * 3];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
 
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        mesh.triangles = triangles;

        //We can only render 1023 instance at once
        //So we split the 256 * 256 object into sets of size 1023
        for(int set=0; set<wantedInstances/instanceMax; set++){
            int instance = instanceMax;
            if(set == (wantedInstances / instanceMax) - 1){
                instance = wantedInstances % instanceMax;
            }
            transformList[set] = new Matrix4x4[instance];

            for(int i=0; i<instance; i++){
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetTRS(Vector3.zero, Quaternion.Euler(Vector3.zero), Vector3.one);
                transformList[set][i] = matrix;
            }
        }
    }

    void Update(){
          for(int set=0; set<wantedInstances/instanceMax; set++){
            int instance = instanceMax;
            if(set == (wantedInstances / instanceMax) - 1){
                instance = wantedInstances % instanceMax;
            }

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mpb.SetInt("offset", set * instanceMax);
            if(set < wantedInstances/instanceMax/2){
                mpb.SetColor("color", new Color(0.45f, 0.5f, 0.75f, 0.5f));
            }else{
                mpb.SetColor("color", new Color(0.9f, 0.4f, 0.5f, 0.5f));
            }

            Graphics.DrawMeshInstanced(mesh, 0, material, transformList[set], instance, mpb);
          }
    }
}