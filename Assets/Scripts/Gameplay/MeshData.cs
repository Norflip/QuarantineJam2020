using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshData
{
    static Dictionary<int, float> volumeCache = new Dictionary<int, float>();

    public static float CalculateMeshVolume(Mesh mesh, Vector3 scale)
    {
        float scaledVolume;
        int meshKey = mesh.GetHashCode();

        if (!volumeCache.TryGetValue(meshKey, out scaledVolume))
        {
            float scaleSum = scale.x * scale.y * scale.z;

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 p1 = vertices[triangles[i + 0]];
                Vector3 p2 = vertices[triangles[i + 1]];
                Vector3 p3 = vertices[triangles[i + 2]];
                scaledVolume += SignedVolumeOfTriangle(p1, p2, p3);
            }

            scaledVolume = Mathf.Abs(scaledVolume) * scaleSum;
            volumeCache.Add(meshKey, scaledVolume);
        }

        return scaledVolume;
    }

    private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
}
