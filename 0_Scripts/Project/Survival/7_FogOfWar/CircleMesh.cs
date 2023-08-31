using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMesh : MonoBehaviour
{

    public int polygon = 3;
    public float size = 1.0f;
    public Vector3 offset = new Vector3(0, 0, 0);

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void OnValidate()
    {

        if (mesh == null) return;

        if (size > 0 || offset.magnitude > 0 || polygon >= 3)
        {

            setMeshData(size, polygon);
            createProceduralMesh();
        }
    }

    private void Awake()
    {

        mesh = new Mesh();
        mesh.name = "ViewMesh";
        GetComponent<MeshFilter>().mesh = mesh;
        // mesh = GetComponent<MeshFilter>().mesh;

        setMeshData(size, polygon);
        createProceduralMesh();
    }

    void setMeshData(float _size, int _polygon)
    {

        vertices = new Vector3[polygon + 1];

        vertices[0] = new Vector3(0, 0, 0) + offset;

        for (int i = 1; i <= polygon; i++)
        {

            float angle = -i * (Mathf.PI * 2.0f) / polygon;

            vertices[i] = (new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * size) + offset;
        }

        triangles = new int[3 * polygon];
        for (int i = 0; i < polygon - 1; i++)
        {

            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        triangles[3 * polygon - 3] = 0;
        triangles[3 * polygon - 2] = polygon;
        triangles[3 * polygon - 1] = 1;
    }

    void createProceduralMesh()
    {

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Destroy(this.GetComponent<MeshCollider>());
        // this.gameObject.AddComponent<MeshCollider>();
    }
}
