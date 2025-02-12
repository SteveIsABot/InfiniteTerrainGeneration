using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{

    [SerializeField] public int zSize = 10;
    [SerializeField] public int xSize = 10;
    [SerializeField] public float verticalScaleFactor = 2.0f;
    [SerializeField] public float xZoomFactor = 0.3f;
    [SerializeField] public float zZoomFactor = 0.3f;

    private List<Vector3> verticiesList = new List<Vector3>();
    private List<int> trianglePointList = new List<int>();
    private List<Color> colourList = new List<Color>();

    private Mesh gridMesh;
    private Material gridMat;

    void Start()
    {
        gridMesh = gameObject.GetComponent<MeshFilter>().mesh;
        gridMat = gameObject.GetComponent<MeshRenderer>().material;

        updateMesh();
        //updateMat();
    }

    void createVertices(){

        for(int z = -(zSize / 2); z <= (zSize / 2); z++){
            for(int x = -(xSize / 2); x <= (xSize / 2); x++){

                float originX = (transform.position.x + x) * xZoomFactor;
                float originZ = (transform.position.z + z) * zZoomFactor;
                float y = (Mathf.PerlinNoise(originX, originZ) * verticalScaleFactor) - (verticalScaleFactor / 2);

                colourList.Add(Color.black);
                verticiesList.Add(new Vector3(x, y, z));
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

        createVertices();
        createTriangles();

        gridMesh.vertices = verticiesList.ToArray();
        gridMesh.triangles = trianglePointList.ToArray();
        gridMesh.colors = colourList.ToArray();

        gridMesh.RecalculateNormals();
    }
    
    void updateMat(){
        gridMat.SetFloat("MaxHeight", verticalScaleFactor);
    }

    private void OnDrawGizmos(){

        Gizmos.color = Color.red;
        for(int i = 0; i < verticiesList.Count; i++){
            Gizmos.DrawSphere(transform.position + verticiesList[i], 0.1f);
        }
        
        Gizmos.color = Color.black;
        Gizmos.DrawWireMesh(gridMesh, transform.position);
    }
}
