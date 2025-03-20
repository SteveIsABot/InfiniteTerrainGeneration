using UnityEngine;

public enum SimulationState { Start, Playing, Paused};

public class SimulationSystem : MonoBehaviour
{

    public SimulationState currentState;
    private SimulationState previousState;

    [SerializeField] public GameObject CameraObj;
    [SerializeField] public GameObject TerrainManagerObj;
    [SerializeField] public GameObject StartMenu;
    [SerializeField] public GameObject PauseMenu;

    void Start() {
        currentState = SimulationState.Start;
    }

    void Update() {

        if(Input.GetKey(KeyCode.Escape)) {currentState = SimulationState.Paused;}

        if(currentState != previousState) {

            switch(currentState) {
                case SimulationState.Start: StartMenuActive(); break;
                case SimulationState.Playing: SimulationActivate(); break;
                case SimulationState.Paused: PasueMenuActivate(); break;
            }

            previousState = currentState;
        }
        
    }

    void StartMenuActive() {
        Cursor.visible = true;
        StartMenu.SetActive(true);
        PauseMenu.SetActive(false);
        CameraObj.GetComponent<CameraMovement>().enabled = false;
        TerrainManagerObj.GetComponent<TerrainController>().enabled = false;
    }

    void PasueMenuActivate() {
        Cursor.visible = true;
        StartMenu.SetActive(false);
        PauseMenu.SetActive(true);
        CameraObj.GetComponent<CameraMovement>().enabled = false;
        TerrainManagerObj.GetComponent<TerrainController>().enabled = false;
    }

    void SimulationActivate() {
        Cursor.visible = false;
        StartMenu.SetActive(false);
        PauseMenu.SetActive(false);
        CameraObj.GetComponent<CameraMovement>().enabled = true;
        TerrainManagerObj.GetComponent<TerrainController>().enabled = true;
    }

    public void OnClickStartOrContinueButton() {
        currentState = SimulationState.Playing;
    }

    public void OnClickQuitButton() {
        currentState = SimulationState.Start;
    }

}