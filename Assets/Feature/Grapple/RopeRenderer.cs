using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    public LayerMask hitLayer;
    public float lineThickness = 0.2f;
    public float uvWorldSpaceScale = 100f;
    public float tensionScale = 1;
    private Mesh mesh;
    private List<Vector3> lines = new List<Vector3>();
    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        //lineRenderer.SetPositions();
    }

    private void OnEnable()
    {
        mesh.Clear();
    }

    public void AddPoint(Vector3 point)
    {
        lines.Add(point);
    }

    public void MovePoint(int index, Vector3 newPosition)
    {
        if (lines.Count -1 >= index)
        {
            lines[index] = newPosition;
        }
        else
        {
            Debug.LogError("Attempt to access point that don't exist!");
        }
    }

    public void RepaintRope()
    {
        RopeRender.DrawRope(lines,lineThickness,uvWorldSpaceScale * tensionScale,Color.cyan, mesh);
    }

    public void ClearRope()
    {
        lines.Clear();
        RopeRender.DrawRope(lines,lineThickness,uvWorldSpaceScale * tensionScale,Color.cyan, mesh);
    }
    
}
