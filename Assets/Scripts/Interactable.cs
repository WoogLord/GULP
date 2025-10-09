using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int gulpMass = 1; // what is checked and added
    private bool isGulping = false;
    private Player targetPlayer;
    private Rigidbody rb;
    public Collider itemMeshCollider;

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isGulping || targetPlayer == null) return; // why?

        Vector3 dir = (
            // targetPlayer.GetCenter() - 
            rb.position
        );
        float distance = dir.magnitude;
        dir.Normalize();

        // Smooth pull toward player center
        rb.linearVelocity = dir * Mathf.Min(targetPlayer.gulpSpeed, distance * 10f);

        // Consume when close
        if (distance < 0.5f)
        {
            targetPlayer.GainMass(gulpMass);
            Destroy(gameObject); // destroy Interactable parent
            Debug.Log("Gulped");
        }
    }

    public void StartGulping(PlayerMovement player)
    {
        if (isGulping) return;

        targetPlayer = player;
        isGulping = true;

        // physics settings for smooth suction
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // pick the collider we want to ignore with the player
        Collider itemCol = itemMeshCollider != null ? itemMeshCollider : GetComponent<Collider>();
        if (itemCol == null)
        {
            Debug.LogWarning($"{name}: no item collider found to ignore collisions.");
        }
        else
        {
            // ignore collisions with every collider that belongs to the player (including child bones)
            foreach (Collider col in player.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(itemCol, col, true);
                Debug.Log($"Ignoring collision between item '{itemCol.name}' and player collider '{col.name}'");
            }
        }
    }

}
