using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Rigidbody yposBoneRb;
    public Player player;
    public Vector3 cameraOffset = new Vector3(0f, 2f, -4f);
    public Vector3 cameraRotation = new Vector3(10f, 0f, 0f);

    void Start(){
        transform.rotation = Quaternion.Euler(cameraRotation);
    }

    void FixedUpdate(){
        transform.position = new Vector3(
            yposBoneRb.position.x + (cameraOffset.x * (((player.gulpedMass - 1f) * 0.1f) + 1f))
            , yposBoneRb.position.y + (cameraOffset.y * (((player.gulpedMass - 1f) * 0.1f) + 1f))
            , yposBoneRb.position.z + (cameraOffset.z * (((player.gulpedMass - 1f) * 0.1f) + 1f))
        );
    }
}
