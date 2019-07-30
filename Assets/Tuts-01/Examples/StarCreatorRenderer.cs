using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCreatorRenderer : MonoBehaviour {
    public Material material;

    Matrix4x4[][] transformList;

    Mesh mesh;

    const int instance_max = 1023;
    const int wanted_instances = 256 * 256;

    const float star_size = 1.0f;

    // Use this for initialization
    void Start () {
        transformList = new Matrix4x4[wanted_instances / instance_max][];

        MeshFilter mf = GetComponent<MeshFilter>();

        mesh = new Mesh();
        mf.mesh = mesh;

        // Create a basic quad.
        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-star_size, -star_size, 0);
        vertices[1] = new Vector3(star_size, -star_size, 0);
        vertices[2] = new Vector3(-star_size, star_size, 0);
        vertices[3] = new Vector3(star_size, star_size, 0);

        mesh.vertices = vertices;

        int[] tri = new int[6];

        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;

        mesh.triangles = tri;

        // Only 1023 objects can be rendered as instanced.
        // So we split the 256*256 objects into sets of size 1023.
        for (int set = 0; set < wanted_instances / instance_max; set++)
        {
            int instances = instance_max;
            if (set == (wanted_instances / instance_max) - 1)
            {
                instances = wanted_instances % instance_max;
            }

            transformList[set] = new Matrix4x4[instances];

            for (int i = 0; i < instances; i++)
            {
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetTRS(Vector3.zero, Quaternion.Euler(Vector3.zero), Vector3.one);
                transformList[set][i] = matrix;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int set = 0; set < wanted_instances / instance_max; set++)
        {
            int instances = instance_max;
            if (set == (wanted_instances / instance_max) - 1)
            {
                instances = wanted_instances % instance_max;
            }

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mpb.SetInt("offset", set * instance_max);
            if (set < wanted_instances / instance_max / 2)
                mpb.SetColor("color", new Color(0.45f, 0.5f, 0.75f, 0.5f));
            else
                mpb.SetColor("color", new Color(0.9f, 0.4f, 0.5f, 0.5f));
            Graphics.DrawMeshInstanced(mesh, 0, material, transformList[set], instances, mpb);
        }
    }
}
