using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [SerializeField] public GameObject terrainGrid;
    [SerializeField] public int maxGrids = 200;
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
                
                Debug.Log(child.gameObject.GetComponent<Renderer>().enabled);

                if(child.gameObject.GetComponent<Renderer>().enabled) { 
                    activeTerrainList.Add(child.gameObject);
                } else {
                    nonActiveTerrainList.Add(child.gameObject);
                }

            }
        }
    }

    void SortGrids() {

        activeTerrainList.Sort((a, b) =>
            (a.transform.position - cameraObj.transform.position).sqrMagnitude.CompareTo((b.transform.position - cameraObj.transform.position).sqrMagnitude)
        );

        nonActiveTerrainList.Sort((a, b) =>
            (a.transform.position - cameraObj.transform.position).sqrMagnitude.CompareTo((b.transform.position - cameraObj.transform.position).sqrMagnitude)
        );
    }

    void Update() {
        UpdateList();
        SortGrids();
    }
}
