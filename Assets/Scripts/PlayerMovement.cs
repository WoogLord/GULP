using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Collider gulpMeshCollider;
    public Collider singleBoneColliderRef;
    public GameObject gulpSlime;

    public float originalRadius;
    public Vector3 originalCenter;
    public float originalRadiusBones;
    public Vector3 originalCenterBones;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction lookAction;
    InputAction debugAction;

    public float moveSpeed = 100f;
    public float moveAccel = 50f;
    public float jumpAccel = 5f;
    public float jumpSpeed = 10f;
    public float gulpSpeed = 5f;
    public float gulpForce = 50f;
    public float gulpRadius = 5f;

    public float gulpedMass = 1f;
    public float currGulpedMass = 1f;

    void Start()
    {
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
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
        debugAction = InputSystem.actions.FindAction("Debug");
    }

    void FixedUpdate()
    {
        Move();
        if(debugAction.IsPressed())
            GainMass(1);
    }

    private void Move(){
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(movement * moveSpeed, ForceMode.Force);
    }

    // private void OnTriggerEnter(Collider other){
    //     if (other.CompareTag("Gulpable")){
    //         Interactable item = other.GetComponent<Interactable>();
    //         if (item != null && gulpedMass > item.gulpMass){
    //             item.StartGulping(this);
    //         }
    //     }
    // }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Gulpable")){
            Interactable item = collision.gameObject.GetComponent<Interactable>();
            if (item != null && gulpedMass > item.gulpMass){
                item.StartGulping(this);
            }
        }
    }

    public Vector3 GetCenter(){return transform.position;}

    public void GainMass(int amount){
        gulpedMass += amount;
        
        float currScale = currGulpedMass / 10;
        float targetScale = (gulpedMass / 10);
        // transform.localScale = Vector3.one * scale;
        gulpSlime.transform.localScale = (Vector3.one * targetScale) + gulpSlime.transform.localScale; //lerp this
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
