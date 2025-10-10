using UnityEngine;

public class CollisionRelay : MonoBehaviour
{
    public Player player;

    private void OnCollisionEnter(Collision collision)
    {
        player.OnChildCollisionEnter(collision);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     player.OnChildTriggerEnter(other);
    // }
}