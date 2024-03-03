using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the trigger is tagged as "Player".
        // Tags are a way to identify GameObjects in Unity, making it easier to find and interact with them.
        if (other.CompareTag("Player"))
        {
            // Attempt to get the Health component attached to the player GameObject.
            // The Health component is assumed to be a custom script managing the player's health.
            var player = other.GetComponent<Health>();

            // Check if the player actually has a Health component attached.
            if (player != null)
            {
                // If the player is not already protected by a shield,
                // then proceed to give them protection.
                if (!player.IsProtected)
                {
                    // Set the player's IsProtected status to true,
                    // indicating that the player is now under the shield's protection.
                    player.IsProtected = true;

                    // Destroy this shield GameObject, removing it from the game.
                    // This typically occurs after the shield has been used to protect the player.
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
