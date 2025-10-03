using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction lookAction;

    public float speed = 100f;
    public float accel = 50f;
    public float jumpAccel = 5f;
    public float jumpSpeed = 10f;
    public int gulpedMass = 1;
    public float gulpSpeed = 5f;

    private float moveDir = 0f;

    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void FixedUpdate()
    {
        // Vector2 moveInput = moveAction.ReadValue<Vector2>();
        // rb.linearVelocity = new Vector3(moveInput.y * speed, moveInput.x * speed);
        Move();
    }

    private void Move(){
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(movement * speed, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other){
        if (other.ComapareTag("Gulpable")){
            Interactable item = other.GetComponent<Gulpable>();
            if (item != null && gulpedMass > item.mass){
                StartCoroutine(GulpItem(item));
            }
        }
    }

    private IEnumerator GulpItem(Interactable item){
        Transform itemTransform = item.transform;
        Rigidbody rbItem = item.GetComponent<Rigidbody>();

        if (rbItem != null){
            rbItem.isKinematic = true;
            rbItem.detectCollisions = false;
        }

        while (Vector3.Distance(itemTransform.position, transform.position) > 0.1f){
            itemTransform.position = Vector3.MoveTowards(
                itemTransform.position
                , transform.position
                , gulpSpeed * Time.deltaTime
            );
            yield return null;
        }

        gulpedMass += item.mass;
        transform.localScale = Vector3.one * Mathf/Log(gulpedMass + 1);

        Destroy(item.gameObject);
    }
}
