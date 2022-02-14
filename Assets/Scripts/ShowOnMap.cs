using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnMap : MonoBehaviour
{
    public Material originMaterial;
    Material createMaterial;
    float[] mPoints;
    int mHitCount;

    //public float xp;
    //public float yp;

    // Start is called before the first frame update
    void Awake()
    {
        mPoints = new float[32 * 3];
        GetComponent<MeshRenderer>().material = new Material(originMaterial);
        createMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createCircle(Color color, float xPosShader, float yPosShader, float size)
    {


        mPoints[mHitCount * 3] = xPosShader;
        mPoints[mHitCount * 3 + 1] = yPosShader;
        mPoints[mHitCount * 3 + 2] = size;

        mHitCount++;
        mHitCount %= 32;

        createMaterial.SetVector("_ColorSelect", new Vector4(color.r, color.g, color.b, color.a));
        createMaterial.SetFloatArray("_Hits", mPoints);
        createMaterial.SetFloat("_Diameter", size);
        createMaterial.SetInt("_HitCount", mHitCount);
    }



}
