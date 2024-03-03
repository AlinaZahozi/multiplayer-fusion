using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] NumberField HealthDisplay;

    // Public variable to indicate whether the object is currently protected (e.g., by a shield).
    public bool IsProtected = false;

    // Timer to track how long the protection lasts. Initialized to 10 seconds.
    private float protectionTimer = 10f;

    // A networked property for health that automatically synchronizes its value across the network.
    // OnChanged specifies a method to be called whenever the value changes, to update UI and log messages.
    [Networked(OnChanged = nameof(NetworkedHealthChanged))]
    public int NetworkedHealth { get; set; } = 100;

    // Static method called when NetworkedHealth changes. It logs the new health and updates the health display UI.
    private static void NetworkedHealthChanged(Changed<Health> changed)
    {
        Debug.Log($"Health changed to: {changed.Behaviour.NetworkedHealth}");
        changed.Behaviour.HealthDisplay.SetNumber(changed.Behaviour.NetworkedHealth);
    }

    // An RPC (Remote Procedure Call) method that allows any client or the server to request damage to be dealt.
    // This method is executed on the State Authority (usually the server), which has the final say in game state changes.
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(int damage)
    {
        // Checks if the object is currently protected. If not, it proceeds to apply damage.
        if (!IsProtected)
        {
            Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
            NetworkedHealth -= damage;
        }
    }

    // Update is called once per frame. It manages the countdown of the protection timer and resets protection state.
    void Update()
    {
        // If the object is protected, decrement the protection timer by the time passed since the last frame.
        if (IsProtected)
        {
            protectionTimer -= Runner.DeltaTime;
            Debug.Log(protectionTimer);

            // If the timer reaches zero or below, disable protection and reset the timer.
            if (protectionTimer <= 0)
            {
                IsProtected = false;
            }
        }
        else
        {
            // If not protected, ensure the timer is reset to its initial value.
            protectionTimer = 10f;
        }
    }
}
