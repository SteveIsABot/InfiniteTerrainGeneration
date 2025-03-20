using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float mainSpeed = 5f;
    [SerializeField] float speedBooster = 1.0f;
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float camSens = 0.5f;

    private float yaw = 0;
    private float pitch = 0;
    private float currentSpeed = 5.0f;
    private bool speedBoosterToggle = false;

    void Start() {
        currentSpeed = mainSpeed;
    }

    void Update() {

        pitch += Input.GetAxis("Mouse X") * camSens * 5.0f;
        yaw -= Input.GetAxis("Mouse Y") * camSens * 5.0f;
        speedBoosterToggle = Input.GetKey(KeyCode.LeftControl) ? !speedBoosterToggle : speedBoosterToggle;
        currentSpeed = speedBoosterToggle ? currentSpeed += speedBooster : currentSpeed = mainSpeed;

        yaw = Mathf.Clamp(yaw, -85.0f, 85.0f);
        currentSpeed = Mathf.Clamp(currentSpeed, mainSpeed, maxSpeed);

        transform.eulerAngles = new Vector3(yaw, pitch, 0);
        
        Vector3 camFwd = transform.forward;
        Vector3 camRight = transform.right;
        camFwd.y = 0.0f;
        camRight.y = 0.0f;
        camFwd.Normalize();
        camRight.Normalize();

        if(Input.GetKey(KeyCode.W)) { transform.position += camFwd * currentSpeed * Time.fixedDeltaTime; }
        if(Input.GetKey(KeyCode.A)) { transform.position -= camRight * currentSpeed * Time.fixedDeltaTime; }
        if(Input.GetKey(KeyCode.S)) { transform.position -= camFwd * currentSpeed * Time.fixedDeltaTime; }
        if(Input.GetKey(KeyCode.D)) { transform.position += camRight * currentSpeed * Time.fixedDeltaTime; }

        if(Input.GetKey(KeyCode.Space)) { transform.position += Vector3.up * currentSpeed * 0.5f * Time.fixedDeltaTime; }
        if(Input.GetKey(KeyCode.LeftShift)) { transform.position -= Vector3.up * currentSpeed * 0.5f * Time.fixedDeltaTime; }
    }
}
