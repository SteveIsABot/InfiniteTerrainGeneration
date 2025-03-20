using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SimulationState { Start, Playing, Paused};

public class SimulationSystem : MonoBehaviour
{

    public SimulationState currentState;
    private SimulationState previousState;

    [SerializeField] public GameObject CameraObj;
    [SerializeField] public GameObject TerrainManagerObj;
    [SerializeField] public GameObject PauseMenu;

    void Start() {
        currentState = SimulationState.Start;
    }

    void Update() {

        if(currentState != previousState) {

            switch(currentState) {
                case SimulationState.Start: /*Function*/ break;
                case SimulationState.Playing: PasueMenuActivate(); break;
                case SimulationState.Paused: SimulationActivate(); break;
            }

            previousState = currentState;
            Debug.Log(currentState);

        }
        
    }

    void PasueMenuActivate() {
        PauseMenu.SetActive(true);
        CameraObj.GetComponent<CameraMovement>().enabled = false;
        TerrainManagerObj.GetComponent<TerrainController>().enabled = false;
    }

    void SimulationActivate() {
        PauseMenu.SetActive(false);
        CameraObj.GetComponent<CameraMovement>().enabled = true;
        TerrainManagerObj.GetComponent<TerrainController>().enabled = true;
    }

}