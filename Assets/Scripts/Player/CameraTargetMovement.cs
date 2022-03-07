using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTargetMovement : MonoBehaviour
{
    // Constants
    [Range(1, 179)] private const float maxFollowTargetAngle = 40;
    [Range(181, 359)] private const float minFollowTargetAngle = 340; // NOTE: actually -20, but we only see positive values

    // Inspector values
    [SerializeField] private PlayerDefault pd;

    private InputAction look;
    private Cinemachine3rdPersonFollow vCamera;

    // Start is called before the first frame update
    void Start()
    {
        bool found = false;

        CinemachineVirtualCamera[] potentialCameras = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach(CinemachineVirtualCamera camera in potentialCameras)
        {
            if (camera.gameObject.name.Equals("PlayerAimCam"))
            {
                if (camera.GetCinemachineComponent(CinemachineCore.Stage.Body) is Cinemachine3rdPersonFollow transposer)
                {
                    vCamera = transposer;
                    found = true;
                }
                else
                    Debug.LogError($"Unable to access Cinemachine3rdPersonFollow in {camera}");
                break;
            }
        }

        if (!found)
            Debug.LogError("Unable to find PlayerAimCam");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Actual rotation
        transform.rotation *=
            Quaternion.AngleAxis(-look.ReadValue<Vector2>().y * pd.sensitivity, Vector3.right);

        // Clamp rotation
        float xAngle = transform.localEulerAngles.x;
        if (xAngle > 180)
            xAngle = Mathf.Max(minFollowTargetAngle, xAngle);
        else if (xAngle < 180)
            xAngle = Mathf.Min(maxFollowTargetAngle, xAngle);
        transform.localEulerAngles = new Vector3(xAngle, 0, 0);

        // Set follow radius
        vCamera.CameraDistance = 6; //TODO (Simon): Actually have this value change
    }

    private void OnEnable()
    {
        var playerInputMap = InputManager.inputActions.Player;
        look = playerInputMap.Look;
        look.Enable();
    }

    private void OnDisable()
    {
        look.Disable();
    }
}
