using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [Header("Refs")]
    public Rigidbody yposBoneRb;
    public Player player;

    [Header("Cam vectors")]
    public Vector3 cameraOffset = new Vector3(0f, 1f, -5f);
    public Vector3 scaledOffset;
    public Vector3 desiredPosition;
    public Vector3 cameraRotation;
    public Vector3 rotationPressure = new Vector3(0f, 0f, 0f);

    [Header("Zoom")]
    public float cameraZoomMin = 1f;
    public float startingCameraZoom = 3f;
    public float currentCameraZoom;
    public float cameraZoomMax = 10f;
    public float zoomSpeed = 5f;

    [Header("Rotation")]
    public float senseX = 120f;
    public float senseY = 120f;
    public float minPitch = -25f;
    public float maxPitch = 35f;

    [Header("Lerping")]
    public float followLerpSpeed = 20f;
    private float yaw = 0f;
    private float pitch = 0f;

    void Start(){
        currentCameraZoom = startingCameraZoom;
        // yaw = cameraRotation.y;
        // pitch = cameraRotation.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // transform.rotation = Quaternion.Euler(cameraRotation);
        yposBoneRb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate(){
        float gulpedMassOffset = (((player.gulpedMass - 1f) * 0.1f) + 1f);
        float zoomScale = (currentCameraZoom / startingCameraZoom);

        scaledOffset = Quaternion.Euler(cameraRotation) * (cameraOffset * gulpedMassOffset * zoomScale);
        desiredPosition = yposBoneRb.position + scaledOffset;
    }

    void LateUpdate(){

        // transform.position = new Vector3(
        //     yposBoneRb.position.x + (cameraOffset.x * gulpedMassOffset * zoomScale)
        //     , yposBoneRb.position.y + (cameraOffset.y * gulpedMassOffset * zoomScale)
        //     , yposBoneRb.position.z + (cameraOffset.z * gulpedMassOffset * zoomScale)
        // );

        // cameraRotation.x = Mathf.Clamp(cameraRotation.x - rotationPressure.y, -30f, 30f);
        // cameraRotation.y = Mathf.Clamp(cameraRotation.y + rotationPressure.x, -180f, 180f);

        // transform.rotation = Quaternion.Euler(new Vector3(
        //     cameraRotation.x
        //     , cameraRotation.y
        //     , cameraRotation.z
        // ));

        // New code below
        currentCameraZoom = Mathf.Lerp(currentCameraZoom
            , Mathf.Clamp(currentCameraZoom, cameraZoomMin, cameraZoomMax)
            , zoomSpeed * Time.deltaTime
        );

        yaw += rotationPressure.x * senseX * Time.deltaTime; // (player.usingController ? Time.deltaTime : 1f)
        pitch -= rotationPressure.y * senseY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraRotation = new Vector3 (pitch, yaw, 0f);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followLerpSpeed * Time.deltaTime);
        // transform.position = desiredPosition;
        transform.rotation = Quaternion.Euler(cameraRotation);
    }
}
