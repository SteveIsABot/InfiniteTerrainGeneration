using System.Collections.Generic;
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

    bool inViewChecker(Vector3 pos) {
        Vector3 viewCheck = Camera.main.WorldToViewportPoint(pos);
        return viewCheck.x > 0 && viewCheck.x < 1 && viewCheck.y > 0 && viewCheck.y < 1 && viewCheck.z > 0;
    }

    List<Vector3> createMultiTarget(Vector3 target) {

        float minTargetX = Mathf.FloorToInt(target.x / 10.0f) * 10f;
        float maxTargetX = Mathf.CeilToInt(target.x / 10.0f) * 10f;
        float minTargetZ = Mathf.FloorToInt(target.z / 10.0f) * 10f;
        float maxTargetZ = Mathf.CeilToInt(target.z / 10.0f) * 10f;

        List<Vector3> result = new List<Vector3> {
            new Vector3(minTargetX, 0, minTargetZ),
            new Vector3(minTargetX, 0, maxTargetZ),
            new Vector3(maxTargetX, 0, minTargetZ),
            new Vector3(maxTargetX, 0, maxTargetZ)
        };

        return result;
    }

    void SpawnGridAlgorithm() {

        Vector3 cameraPos = cameraObj.transform.position;
        Vector3 targetPos = roundTargetVector(cameraPos, 10.0f);

        List<Vector3> spawnPlaces = new List<Vector3>();

        while(activeTerrainList.Count < maxActiveGrids) {

            Vector3 currentCenterGrid = targetPos;
            
            if(inViewChecker(targetPos)) {
                Collider[] overlappingCollider = Physics.OverlapBox(targetPos, new Vector3(1f, 2.0f, 1f));
                if(overlappingCollider.Length <= 0) { spawnPlaces.Add(targetPos); }
            }

            while(spawnPlaces.Count < 10) {

                targetPos -= 10 * cameraObj.transform.right;
                List<Vector3> multiTargetPos = createMultiTarget(targetPos);
                bool stillInView = true;

                foreach(Vector3 pos in multiTargetPos) {
                    if(inViewChecker(pos)) {
                        Collider[] overlappingCollider = Physics.OverlapBox(pos, new Vector3(1f, 2.0f, 1f));
                        if(overlappingCollider.Length <= 0) { spawnPlaces.Add(pos); }
                        stillInView = true;
                    } else {
                        stillInView = false;
                    }
                }

                if(!stillInView) { break; }
            }

            targetPos = currentCenterGrid;

            while(spawnPlaces.Count < 10) {

                targetPos += 10 * cameraObj.transform.right;
                List<Vector3> multiTargetPos = createMultiTarget(targetPos);
                bool stillInView = true;

                foreach(Vector3 pos in multiTargetPos) {
                    if(inViewChecker(pos)) {
                        Collider[] overlappingCollider = Physics.OverlapBox(pos, new Vector3(1f, 2.0f, 1f));
                        if(overlappingCollider.Length <= 0) { spawnPlaces.Add(pos); }
                        stillInView = true;
                    } else {
                        stillInView = false;
                    }
                }

                if(!stillInView) { break; }

            }

            targetPos = currentCenterGrid;
            targetPos += 10 * cameraObj.transform.forward;
            targetPos = roundTargetVector(targetPos, 10.0f);
            
            if(spawnPlaces.Count > 0) { break; }

        }

        if(spawnPlaces.Count > 0) CreateNewGrids(spawnPlaces);
    }

    void Update() {
        
        UpdateList();
        SortGrids(activeTerrainList);
        SortGrids(nonActiveTerrainList);
        
        float camRotationX = Mathf.Abs(cameraObj.transform.rotation.eulerAngles.x) % 360.0f;
        if(camRotationX < 30.0f) { SpawnGridAlgorithm(); }

        DeleteGrids();
    }
}
