using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{

    [SerializeField] public int zSize = 10;
    [SerializeField] public int xSize = 10;
    private List<Vector3> verticiesList = new List<Vector3>();
    private List<int> trianglePointList = new List<int>();
    private Mesh gridMesh;

    void Start()
    {
        gridMesh = gameObject.GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        updateMesh();
    }

    void createVertices(){

        for(int z = 0; z <= zSize; z++){
            for(int x = 0; x <= xSize; x++){
                verticiesList.Add(new Vector3(x, 0, z));
            }
        }

    }

    void createTriangles(){

        for(int z = 0; z < zSize; z++){
            for(int x = 0; x < xSize; x++){
                trianglePointList.Add(x + (z * (zSize + 1)));
                trianglePointList.Add(x + xSize + 1 + (z * (zSize + 1)));
                trianglePointList.Add(x + 1 + (z * (zSize + 1)));

                trianglePointList.Add(x + xSize + 1 + (z * (zSize + 1)));
                trianglePointList.Add(x + xSize + 2 + (z * (zSize + 1)));
                trianglePointList.Add(x + 1 + (z * (zSize + 1)));
            }
        }

    }
    
    void updateMesh(){

        gridMesh.Clear();
        verticiesList.Clear();
        trianglePointList.Clear();

        createVertices();
        createTriangles();

        gridMesh.vertices = verticiesList.ToArray();
        gridMesh.triangles = trianglePointList.ToArray();

        gridMesh.RecalculateNormals();
    }

    private void OnDrawGizmos(){

        Gizmos.color = Color.red;
        for(int i = 0; i < verticiesList.Count; i++){
            Gizmos.DrawSphere(verticiesList[i], 0.1f);
        }
        
        Gizmos.color = Color.black;
        Gizmos.DrawWireMesh(gridMesh);
    }
}
