using UnityEditor;
using UnityEngine;

public class WorldScript
{
    [MenuItem("Tools/Add Mesh Colliders To Selection")]
    static void AddColliders()
    {
        GameObject[] selected = Selection.gameObjects;

        int count = 0;

        foreach (GameObject root in selected)
        {
            MeshFilter[] meshes = root.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter mesh in meshes)
            {
                if (mesh.GetComponent<Collider>() == null)
                {
                    MeshCollider mc = mesh.gameObject.AddComponent<MeshCollider>();
                    mc.convex = false;
                    count++;
                }
            }
        }

        Debug.Log($"Added {count} MeshColliders.");
    }

}
