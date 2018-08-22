﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SphereScreen : MonoBehaviour
{
    private const float radius = 0.5f;

    [SerializeField]
    private int div = 4;

    [SerializeField]
    private float scaleH = 1.7f;
    [SerializeField]
    private float scaleV = 1.5f;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (div < 1)
        {
            div = 1;
        }
        if (div > 64)
        {
            div = 64;
        }
        EditorApplication.delayCall += CreateMesh;
    }
#endif
    private void OnEnable()
    {
        CreateMesh();
    }
    private void CalcPosUV(int div, Vector3[] vtx, Vector2[] uv)
    {
        int idx = 0;
        var ih = 1.0f / (div * 4);
        var iv = 1.0f / (div * 2);
        float opt1x = scaleH;
        float opt1y = scaleV;
        float opt2 = 2.0f;
        float opt3x = 1.0f / (0.5f / Mathf.Tan(Mathf.Atan(0.5f * opt2) * opt1x));
        float opt3y = 1.0f / (0.5f / Mathf.Tan(Mathf.Atan(0.5f * opt2) * opt1y));
        float xx = 1.0f / opt2;//0.5f / Mathf.Tan(Mathf.Atan(0.5f * opt3) / opt1);
        for (int v = 0; v <= div * 2; v++)
        {
            for(int h = 0; h <= div * 4; h++)
            {
                var tu = h * ih;
                var tv = v * iv;
                var vt = (tv - 0.5f) * 2.0f * Mathf.PI * 0.5f;
                var ht = (tu - 0.5f) * 2.0f * Mathf.PI;
                var y = Mathf.Sin(vt);
                var l = Mathf.Cos(vt);
                var x = Mathf.Sin(ht) * l;
                var z = Mathf.Cos(ht) * l;
                vtx[idx] = new Vector3(x, y, z);
#if true
                var tux = Mathf.Tan(Mathf.Atan((tu - 0.5f) * opt3x) / opt1x) * xx + 0.5f;
                var tvx = Mathf.Tan(Mathf.Atan((tv - 0.5f) * opt3y) / opt1y) * xx + 0.5f;
                //Debug.Log(tu + "," + tv + " : " + tux + "," + tvx);
                tu = tux;
                tv = tvx;
#endif
                uv[idx] = new Vector2(tu, tv);
                idx++;
            }
        }
        //Debug.Log(iv.ToString() + "/" + vtx.Length.ToString());
    }
    private void CalcIndex(int div, int[] tri, Vector3[] vtx)
    {
        int it = 0;
        int iv00 = 0;
        int iv10 = div * 4 + 1;
        for (int v = 0; v < div * 2; v++)
        {
            for (int h = 0; h < div * 4; h++)
            {
                tri[it++] = iv10 + 0;
                tri[it++] = iv00 + 1;
                tri[it++] = iv00 + 0;

                tri[it++] = iv10 + 1;
                tri[it++] = iv00 + 1;
                tri[it++] = iv10 + 0;
                iv00++;
                iv10++;
            }
            iv00++;
            iv10++;
        }
        //Debug.Log(it.ToString() + "/" + tri.Length.ToString());
        //Debug.Log(iv10.ToString() + "/" + vtx.Length.ToString());
    }
    private void CreateMesh()
    {
        if (this == null)
        {
            return;
        }
        var mesh = new Mesh();
        mesh.hideFlags = HideFlags.DontSave;
        int nv = (div * 4 + 1) * (div * 2 + 1);
        int nt = div * 4 * div * 2 * 2;

        var vtx = new Vector3[nv];
        var uv = new Vector2[nv];
        var tri = new int[nt * 3];
        CalcPosUV(div, vtx, uv);
        CalcIndex(div, tri, vtx);
        mesh.name = "SphereScreen";
        mesh.vertices = vtx;
        mesh.uv = uv;
        mesh.triangles = tri;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        var filter = GetComponent<MeshFilter>();
        if (filter != null)
        {
            filter.sharedMesh = mesh;
        }
    }
}
