using Fusion;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { // Check if a player has collided and the shield hasn't been claimed
            var player = other.GetComponent<Health>(); // Attempt to get the Health component

            if (player != null)
            {
                if (!player.IsProtected)
                {
                    player.IsProtected = true; // Activate protection

                    // Despawn the shield so it can't be claimed by another player
                    Destroy(this.gameObject);
                }
            }
        }
    }
}