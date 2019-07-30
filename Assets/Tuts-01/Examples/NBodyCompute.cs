using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NBodyCompute : MonoBehaviour {

    public ComputeShader shader;

    public static ComputeBuffer pos_buf;
    public static ComputeBuffer vel_buf;

    void Start () {
        pos_buf = new ComputeBuffer(256 * 256, 3 * sizeof(float), ComputeBufferType.Default);
        vel_buf = new ComputeBuffer(256 * 256, 3 * sizeof(float), ComputeBufferType.Default);

        // These global buffers apply to every shader with these buffers defined.
        Shader.SetGlobalBuffer(Shader.PropertyToID("position"), pos_buf);
        Shader.SetGlobalBuffer(Shader.PropertyToID("velocity"), vel_buf);

        float[] pos_data = new float[256 * 256 * 3];
        float[] vel_data = new float[256 * 256 * 3];

        for (int i = 0; i < 256 * 256; i++)
        {
            float rand_x = (0.5f - (i%256)/256.0f) * 16.0f;
            float rand_y = (0.5f - (i/256)/256.0f) * 16.0f;

            pos_data[i * 3 + 0] = rand_x;
            pos_data[i * 3 + 1] = rand_y;
            // We are 2D so set the z axis to zero.
            pos_data[i * 3 + 2] = 0.0f;

            // If position was a vector from origin (0,0), this would turn it 90 degrees - e.g. create circular motion.
            vel_data[i * 3] = rand_y * 0.01f;
            vel_data[i * 3 + 1] = -rand_x * 0.01f;
            vel_data[i * 3 + 2] = 0.0f;
        }

        pos_buf.SetData(pos_data);
        vel_buf.SetData(vel_data);
    }
	
	// Update is called once per frame
	void Update () {
        shader.Dispatch(shader.FindKernel("CSMain"), 256, 1, 1);
    }

    void OnDestroy()
    {
        pos_buf.Dispose();
        vel_buf.Dispose();
    }
}
