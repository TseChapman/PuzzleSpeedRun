using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[ExecuteInEditMode]
public class CustomCubeUV : MonoBehaviour
{

    [Serializable]
    public class FaceUV
    {
        public Vector2 uv0;
        public Vector2 uv1;
        public Vector2 uv2;
        public Vector2 uv3;
    }

    public FaceUV face0;
    public FaceUV face1;
    public FaceUV face2;
    public FaceUV face3;
    public FaceUV face4;
    public FaceUV face5;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void Awake()
    {
        updateFaceUVs();
    }

    private void setFaceUV(int i, FaceUV face)
    {
        i *= 4;
        GetComponent<MeshFilter>().mesh.uv[i + 0] = face.uv0;
        GetComponent<MeshFilter>().mesh.uv[i + 1] = face.uv1;
        GetComponent<MeshFilter>().mesh.uv[i + 2] = face.uv2;
        GetComponent<MeshFilter>().mesh.uv[i + 3] = face.uv3;
    }
   
    private void updateFaceUVs()
    {
        setFaceUV(0, face0);
    }

    void Update()
    {
        //if (EditorApplication.isPlayingOrWillChangePlaymode)
        //{
        //    return;
        //}
        updateFaceUVs();
    }

}
