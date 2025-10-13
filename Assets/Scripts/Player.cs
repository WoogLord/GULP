using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public Collider gulpMeshCollider;
    public Collider singleBoneColliderRef;
    public GameObject gulpSlime;
    public CameraFollowPlayer Camera;

    public float originalRadius;
    public Vector3 originalCenter;
    public float originalRadiusBones;
    public Vector3 originalCenterBones;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction lookAction;
    InputAction zoomAction;
    InputAction debugAction;

    public float moveSpeed = 100f;
    public float moveAccel = 50f;
    public float jumpAccel = 5f;
    public float jumpSpeed = 10f;
    public float gulpForce = 50f;
    public float gulpRadius = 0.25f;

    public float gulpedMass = 1f;
    public float currGulpedMass = 1f;

    void Start()
    {
        // Depreciated since i just modify scale later
        if (gulpMeshCollider is SphereCollider sc)
        {
            originalRadius = sc.radius;
            originalCenter = sc.center;
        }        
        if (singleBoneColliderRef is SphereCollider sc_bones)
        {
            originalRadiusBones = sc_bones.radius;
            originalCenterBones = sc_bones.center;
        }        

        // better way to do inputs?
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        zoomAction = InputSystem.actions.FindAction("Zoom");
        jumpAction = InputSystem.actions.FindAction("Jump");
        debugAction = InputSystem.actions.FindAction("Debug");
    }

    void FixedUpdate()
    {
        Move();
        Look();
        if(debugAction.IsPressed())
            GainMass(1);
    }

    private void Move(){
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(movement * moveSpeed, ForceMode.Force);
    }

    private void Look(){
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        Vector2 zoomInput = zoomAction.ReadValue<Vector2>();
        Debug.Log($"Current lookInput Value: {lookInput}");
        Debug.Log($"Current zoomInput Value: {zoomInput}");

        Camera.currentCameraZoom = Mathf.Clamp(Camera.currentCameraZoom - zoomInput.y, Camera.cameraZoomMin, Camera.cameraZoomMax);
        Camera.rotationPressure = lookInput;
    }

    public void OnChildCollisionEnter(Collision collision)
    {
        Debug.Log($"[ChildCollisionEnter] Collided with {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Gulpable"))
        {
            Interactable item = collision.gameObject.GetComponent<Interactable>();
            if (item != null && gulpedMass >= item.gulpMass)
            {
                Debug.Log($"Calling StartGulping on {collision.gameObject.name}");
                item.StartGulping(this);
            }
        }
    }

    // public void OnChildTriggerEnter(Collider other)
    // {
    //     Debug.Log($"[ChildTriggerEnter] Collided with {other.name}");
    //     if (other.CompareTag("Gulpable"))
    //     {
    //         Interactable item = other.GetComponent<Interactable>();
    //         if (item != null && gulpedMass >= item.gulpMass)
    //         {
    //             Debug.Log($"Calling StartGulping on {other.name}");
    //             item.StartGulping(this);
    //         }
    //     }
    // }

    public Vector3 GetCenter(){return transform.position;}

    public void GainMass(int amount){
        gulpedMass += amount; // curr = 1f, gulpedMass = 2f
        
        float scale = 10f;
        float currScaleNormalized = currGulpedMass * scale; // 100f
        float targetScaleNormalized = (gulpedMass - 1) * scale; // 200f
        // transform.localScale = Vector3.one * scale;
        gulpSlime.transform.localScale = 
            (Vector3.one * targetScaleNormalized)
            +
            (Vector3.one * 100f) // Reference Vector
        ; //lerp this

        rb.mass = 
            (gulpedMass - 1) * 0.1f
            +
            1 // reference mass
        ;

        currGulpedMass = gulpedMass; // put in fixed update where it lerps between by linear amount
                
        if (gulpMeshCollider is SphereCollider sc)
            {
                // sc.radius = originalRadius * Mathf.Log(targetScale+1);
                // sc.center = originalCenter * Mathf.Log(targetScale+1);
            }
        foreach (Collider col in gulpSlime.GetComponentsInChildren<Collider>())
            Debug.Log($"Collider found named '{col.name}'");
            if (singleBoneColliderRef is SphereCollider sc_bones)
                {
                    // sc_bones.radius = originalRadiusBones * targetScale;
                    // sc_bones.center = originalCenterBones * targetScale;
                }
    }
}
