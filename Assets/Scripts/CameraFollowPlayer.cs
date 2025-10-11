using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Rigidbody yposBoneRb;
    public Player player;
    public Vector3 cameraOffset = new Vector3(0f, 2f, -4f);
    public Vector3 cameraRotation = new Vector3(10f, 0f, 0f);
    public float cameraZoomMin = 1f;
    public float startingCameraZoom = 3f;
    public float currentCameraZoom;
    public float cameraZoomMax = 10f;
    public Vector3 rotationPressure = new Vector3(0f, 0f, 0f);

    void Start(){
        currentCameraZoom = startingCameraZoom;
        transform.rotation = Quaternion.Euler(cameraRotation);
    }

    void FixedUpdate(){
        float gulpedMassOffset = (((player.gulpedMass - 1f) * 0.1f) + 1f);
        float zoomScale = (currentCameraZoom / startingCameraZoom);
        transform.position = new Vector3(
            yposBoneRb.position.x + (cameraOffset.x * gulpedMassOffset * zoomScale)
            , yposBoneRb.position.y + (cameraOffset.y * gulpedMassOffset * zoomScale)
            , yposBoneRb.position.z + (cameraOffset.z * gulpedMassOffset * zoomScale)
        );

        cameraRotation.x = Mathf.Clamp(cameraRotation.x - rotationPressure.y, -30f, 30f);
        cameraRotation.y = Mathf.Clamp(cameraRotation.y + rotationPressure.x, -180f, 180f);

        transform.rotation = Quaternion.Euler(new Vector3(
            cameraRotation.x
            , cameraRotation.y
            , cameraRotation.z
        ));
    }
}
