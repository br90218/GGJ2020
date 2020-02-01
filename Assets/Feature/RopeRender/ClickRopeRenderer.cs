using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRopeRenderer : MonoBehaviour
{
    public LayerMask hitLayer;
    public float lineThickness = 0.2f;
    public float uvWorldSpaceScale = 100f;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit; 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit,2000,hitLayer))
            {
                lines.Add(hit.point);
                RopeRender.DrawRope(lines,lineThickness,uvWorldSpaceScale,Color.cyan, mesh);
            }
        }
    }

    private void OnValidate()
    {
        if (mesh == null)
        {
            mesh = GetComponent<MeshFilter>().mesh;
        }
        
        RopeRender.DrawRope(lines,lineThickness,uvWorldSpaceScale,Color.cyan, mesh);
    }
}
