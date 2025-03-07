using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [SerializeField] public GameObject terrainGrid;
    [SerializeField] public int maxActiveGrids = 200;
    [SerializeField] public int maxNonActiveGrids = 100;
    public List<GameObject> activeTerrainList = new List<GameObject>();
    public List<GameObject> nonActiveTerrainList = new List<GameObject>();

    private GameObject cameraObj;

    void Start() {

        cameraObj = GameObject.Find("Main Camera");
        Vector3 originPos = cameraObj.transform.position;
        originPos.y = 0;
        
        CreateNewGrid(originPos);
    }

    void CreateNewGrid(Vector3 pos) {
        GameObject copy = Instantiate(terrainGrid, pos, Quaternion.identity, transform);
        activeTerrainList.Add(copy);
    }

    void CreateNewGrids(List<Vector3> positionsToSpawn) {
        foreach(Vector3 pos in positionsToSpawn) CreateNewGrid(pos);
    }

    void UpdateList() {

        activeTerrainList.Clear();
        nonActiveTerrainList.Clear();

        foreach(Transform child in transform.GetComponentsInChildren<Transform>()) {
            
            if(child.name != "TerrainManager") {

                if(child.gameObject.GetComponent<Renderer>().enabled) { 
                    activeTerrainList.Add(child.gameObject);
                } else {
                    nonActiveTerrainList.Add(child.gameObject);
                }

            }
        }
    }

    void SortGrids(List<GameObject> list) {

        list.Sort((a, b) =>
            (a.transform.position - cameraObj.transform.position).sqrMagnitude.CompareTo((b.transform.position - cameraObj.transform.position).sqrMagnitude)
        );
    }

    void DeleteGrids() {

        for(int i = nonActiveTerrainList.Count - 1; i > maxNonActiveGrids; i--) {
            GameObject grid = nonActiveTerrainList[i];
            nonActiveTerrainList.RemoveAt(i);
            Destroy(grid);
        }

    }

    Vector3 roundTargetVector(Vector3 target, float gridSize) {
        target.x = Mathf.Round(target.x / gridSize) * gridSize;
        target.z = Mathf.Round(target.z / gridSize) * gridSize;
        target.y = 0;
        return target;
    }

    void SpawnGridAlgorithm() {

        Vector3 cameraPos = cameraObj.transform.position;
        Vector3 targetPos = roundTargetVector(cameraPos, 10.0f);

        List<Vector3> spawnPlaces = new List<Vector3>();

        while(activeTerrainList.Count < maxActiveGrids) {

            /*
            Need to make temp position for furthest away grid in fornt of camera
            targetPos += 10 * cameraObj.transform.forward;
            targetPos = roundTargetVector(targetPos, 10.0f);
            */

            //Left side generation
            while(spawnPlaces.Count < 10) {

                targetPos -= 10 * cameraObj.transform.right;
                targetPos = roundTargetVector(targetPos, 10.0f);
                Vector3 inView = Camera.main.WorldToViewportPoint(targetPos);

                if(inView.x > 0 && inView.x < 1 && inView.y > 0 && inView.y < 1 && inView.z > 0) {

                    Collider[] overlappingCollider = Physics.OverlapBox(targetPos, new Vector3(1f, 2.0f, 1f));
                    if(overlappingCollider.Length <= 0) { spawnPlaces.Add(targetPos); }

                } else {
                    break;
                }

            }

            //Right side generation
            while(spawnPlaces.Count < 10) {

                targetPos += 10 * cameraObj.transform.right;
                targetPos = roundTargetVector(targetPos, 10.0f);
                Vector3 inView = Camera.main.WorldToViewportPoint(targetPos);

                if(inView.x > 0 && inView.x < 1 && inView.y > 0 && inView.y < 1 && inView.z > 0) {

                    Collider[] overlappingCollider = Physics.OverlapBox(targetPos, new Vector3(1f, 2.0f, 1f));
                    if(overlappingCollider.Length <= 0) { spawnPlaces.Add(targetPos); }

                } else {
                    break;
                }

            }

            break;
        }

        if(spawnPlaces.Count > 0) CreateNewGrids(spawnPlaces);
    }

    void Update() {
        
        UpdateList();
        SortGrids(activeTerrainList);
        SortGrids(nonActiveTerrainList);
        
        float camRotationX = Mathf.Abs(cameraObj.transform.rotation.eulerAngles.x) % 360.0f;
        if(camRotationX < 35.0f) { SpawnGridAlgorithm(); }

        DeleteGrids();
    }
}
