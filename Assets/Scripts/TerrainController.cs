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
        //CreateNewGrid(originPos);
    }

    void CreateNewGrid(Vector3 pos) {
        GameObject copy = Instantiate(terrainGrid, pos, Quaternion.identity, transform);
        activeTerrainList.Add(copy);
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

        Debug.Log(nonActiveTerrainList.Count);

        for(int i = nonActiveTerrainList.Count - 1; i > maxNonActiveGrids; i--) {
            GameObject grid = nonActiveTerrainList[i];
            nonActiveTerrainList.RemoveAt(i);
            Destroy(grid);
        }

    }

    void Update() {
        UpdateList();
        SortGrids(activeTerrainList);
        SortGrids(nonActiveTerrainList);

        DeleteGrids();
    }
}
