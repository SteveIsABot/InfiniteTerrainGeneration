using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{

    [SerializeField] public int zSize = 10;
    [SerializeField] public int xSize = 10;
    [SerializeField] public float verticalScaleFactor = 2.0f;
    [Range(0, 1)] [SerializeField] public float xZoomFactor = 0.3f;
    [Range(0, 1)] [SerializeField] public float zZoomFactor = 0.3f;

    public Gradient colourGraident;
    [SerializeField] public bool useTex;

    private List<Vector3> verticiesList = new List<Vector3>();
    private List<int> trianglePointList = new List<int>();
    private BoxCollider AABB;

    private Mesh gridMesh;
    private Material gridMat;
    private Texture2D colourGradientTex;

    void Start()
    {
        gridMesh = gameObject.GetComponent<MeshFilter>().mesh;
        gridMat = gameObject.GetComponent<MeshRenderer>().material;
        AABB = gameObject.GetComponent<BoxCollider>();

        AABB.size = new Vector3(xSize, verticalScaleFactor, zSize);

        updateMesh();
        updateMat();
    }

    void Update() { 
        inCamera();
    }

    void createVertices() {

        for(int z = -(zSize / 2); z <= (zSize / 2); z++){
            for(int x = -(xSize / 2); x <= (xSize / 2); x++){

                float originX = (transform.position.x + x) * xZoomFactor;
                float originZ = (transform.position.z + z) * zZoomFactor;
                float y = (Mathf.PerlinNoise(originX, originZ) * verticalScaleFactor) - (verticalScaleFactor / 2);

                verticiesList.Add(new Vector3(x, y, z));
            }
        }

    }

    void createTriangles() {

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
    
    void updateMesh() {

        verticiesList.Clear();
        trianglePointList.Clear();

        createVertices();
        createTriangles();

        gridMesh.vertices = verticiesList.ToArray();
        gridMesh.triangles = trianglePointList.ToArray();

        gridMesh.RecalculateNormals();
    }
    
    void updateMat() {
        
        gridMat.SetFloat("minHeight", -(verticalScaleFactor/2.0f));
        gridMat.SetFloat("maxHeight", verticalScaleFactor/2.0f);
        
        if(!useTex) {
            gradientToTexture();
            gridMat.SetTexture("colourGradient", colourGradientTex);
        }
    }

    void gradientToTexture() {

        colourGradientTex = new Texture2D(1, 100);
        Color[] pixelColours = new Color[100];

        for(int i = 0; i < 100; i++) { pixelColours[i] = colourGraident.Evaluate((float)i / 100); }
        colourGradientTex.SetPixels(pixelColours);
        colourGradientTex.Apply();
    }

    private void OnDrawGizmos(){

        Gizmos.color = Color.red;
        for(int i = 0; i < verticiesList.Count; i++){
            Gizmos.DrawSphere(transform.position + verticiesList[i], 0.1f);
        }
        
        Gizmos.color = Color.black;
        Gizmos.DrawWireMesh(gridMesh, transform.position);
    }

    void inCamera() {
        Plane[] frustrumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        GetComponent<Renderer>().enabled = GeometryUtility.TestPlanesAABB(frustrumPlanes, AABB.bounds) ? true : false;
    }
}
