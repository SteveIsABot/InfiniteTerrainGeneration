using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [SerializeField] public GameObject terrainGrid;
    public List<GameObject> terrainGridList = new List<GameObject>();

    private GameObject cameraObj;

    void Start() {

        cameraObj = GameObject.Find("Main Camera");
        Vector3 originPos = cameraObj.transform.position;
        originPos.y = 0;

        //CreateNewGrid(originPos);
    }

    void CreateNewGrid(Vector3 pos) {
        GameObject copy = Instantiate(terrainGrid, pos, Quaternion.identity, transform);
        terrainGridList.Add(copy);
    }

    void SortGrids() {
        terrainGridList.Sort((a, b) =>
            (a.transform.position - cameraObj.transform.position).sqrMagnitude.CompareTo((b.transform.position - cameraObj.transform.position).sqrMagnitude)
        );
    }

    void Update() {
        SortGrids();
    }
}
