using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorImage : MonoBehaviour
{

    public float radius = 5f; // 부채꼴의 반지름
    public float angle = 90f; // 부채꼴의 각도 (도 단위)
    public int segments = 30; // 부채꼴을 그릴 때 사용할 세그먼트 수

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = GenerateSectorMesh(radius, angle, segments);
    }

    Mesh GenerateSectorMesh(float radius, float angle, int segments)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 1];
        Vector2[] uv = new Vector2[segments + 1];
        int[] triangles = new int[segments * 3];

        // 원의 중심 정점
        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0.5f);

        // 각도 계산
        float angleStep = angle / segments;
        float currentAngle = 0f;

        // 부채꼴의 정점 계산
        for (int i = 0; i < segments; i++)
        {
            float rad = Mathf.Deg2Rad * currentAngle;
            vertices[i + 1] = new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0f);
            uv[i + 1] = new Vector2(0.5f + Mathf.Cos(rad) * 0.5f, 0.5f + Mathf.Sin(rad) * 0.5f);
            currentAngle += angleStep;
        }

        // 삼각형 정의
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = (i + 1) % segments + 1;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}



