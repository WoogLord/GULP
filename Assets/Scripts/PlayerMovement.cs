using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Collider gulpMeshCollider;

    public float originalRadius;
    public Vector3 originalCenter;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction lookAction;

    public float speed = 100f;
    public float accel = 50f;
    public float jumpAccel = 5f;
    public float jumpSpeed = 10f;
    public float gulpSpeed = 5f;
    public float gulpForce = 50f;
    public float gulpRadius = 5f;

    public int gulpedMass = 1;

    void Start()
    {
        if (gulpMeshCollider is SphereCollider sc)
        {
            originalRadius = sc.radius;
            originalCenter = sc.center;
        }        
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move(){
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(movement * speed, ForceMode.Force);
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
        float scale = Mathf.Log(gulpedMass + 1);
        transform.localScale = Vector3.one * scale;
        if (gulpMeshCollider is SphereCollider sc)
            {
                sc.radius = originalRadius * scale;
                sc.center = originalCenter * scale;
            }
    }
}
