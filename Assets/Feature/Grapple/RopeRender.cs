using System.Collections.Generic;
using UnityEngine;

public static class RopeRender
{ 
    public static void DrawRope(List<Vector3> points, float thickness, float uvScale, Color color, Mesh mesh)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<int> indices = new List<int>();

        float startUV = 0;
        float endUV = 0;

        for (int i = 0; i < points.Count - 1; i++)
        {
            var pointA = points[i];
            var pointB = points[i + 1];
            
            startUV = endUV;
            endUV += uvScale * Vector3.Distance(pointA, pointB);

            float angle = Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x);
            float offsetX = thickness / 2 * Mathf.Sin(angle);
            float offsetY = thickness / 2 * Mathf.Cos(angle);

            vertices.Add(new Vector3(pointA.x + offsetX, pointA.y - offsetY, 0));
            vertices.Add(new Vector3(pointB.x - offsetX, pointB.y + offsetY, 0));
            vertices.Add(new Vector3(pointB.x + offsetX, pointB.y - offsetY, 0)); 
            vertices.Add(new Vector3(pointB.x - offsetX, pointB.y + offsetY, 0));
            vertices.Add(new Vector3(pointA.x + offsetX, pointA.y - offsetY, 0));
            vertices.Add(new Vector3(pointA.x - offsetX, pointA.y + offsetY, 0));
            
//                uv.Add(new Vector2(0,1));
//                uv.Add(new Vector2(1,0));
//                uv.Add(new Vector2(1,1));
//                uv.Add(new Vector2(1,0));
//                uv.Add(new Vector2(0,1));
//                uv.Add(new Vector2(0,0));
            uv.Add(new Vector2(startUV,1));
            uv.Add(new Vector2(endUV,0));
            uv.Add(new Vector2(endUV,1));
            uv.Add(new Vector2(endUV,0));
            uv.Add(new Vector2(startUV,1));
            uv.Add(new Vector2(startUV,0));

            int indexOffset(int value) => (ushort)(value + (i * 6));
            indices.Add(indexOffset(0));
            indices.Add(indexOffset(1));
            indices.Add(indexOffset(2));
            indices.Add(indexOffset(3));
            indices.Add(indexOffset(4));
            indices.Add(indexOffset(5));

        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.uv = uv.ToArray();
    }

}
