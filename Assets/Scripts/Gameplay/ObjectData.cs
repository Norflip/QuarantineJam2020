using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    static Dictionary<int, float> volumeCache = new Dictionary<int, float>();

    public MaterialData materialData;

    public float Volume => m_volume;
    float m_volume;

    public float ScaledVolume => m_scaledVolume;
    float m_scaledVolume;

    public float Mass => ScaledVolume * (materialData != null ? materialData.density : 1.0f);

    void Start()
    {
        RecalculateMeshVolume(materialData);
    }

    public void RecalculateMeshVolume(MaterialData materialData)
    {
        if(materialData != null)
        {
            this.materialData = materialData;
            RecalculateMeshVolume();
        }
    }

    public void RecalculateMeshVolume()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        int meshKey = mesh.GetHashCode();

        if (!volumeCache.TryGetValue(meshKey, out m_volume))
        {
            m_volume = VolumeOfMesh(mesh);
            volumeCache.Add(meshKey, m_volume);
        }

        float scale = transform.localScale.x * transform.localScale.y * transform.localScale.z;
        m_scaledVolume = m_volume * scale;

        GetComponent<Rigidbody>().mass = Mass;
    }

    public static float VolumeOfMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        float volume = 0;

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }

        return Mathf.Abs(volume);
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
