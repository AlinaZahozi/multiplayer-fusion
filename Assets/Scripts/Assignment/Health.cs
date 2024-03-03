using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] NumberField HealthDisplay;
    public bool IsProtected = false;
    private float protectionTimer = 10f;

    [Networked(OnChanged = nameof(NetworkedHealthChanged))]
    public int NetworkedHealth { get; set; } = 100;
    private static void NetworkedHealthChanged(Changed<Health> changed)
    {
        // Here you would add code to update the player's healthbar.
        Debug.Log($"Health changed to: {changed.Behaviour.NetworkedHealth}");
        changed.Behaviour.HealthDisplay.SetNumber(changed.Behaviour.NetworkedHealth);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    // All players can call this function; only the StateAuthority receives the call.
    public void DealDamageRpc(int damage)
    {
        if (!IsProtected)
        {
            Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
            NetworkedHealth -= damage;
        }
    }
    void Update()
    {
        if (IsProtected)
        {
            protectionTimer -= Runner.DeltaTime; // Count down the timer
            Debug.Log(protectionTimer);
            if (protectionTimer <= 0)
            {
                // Protection time is up, deactivate it
                IsProtected = false;
            }
        }
        else
        {
            protectionTimer = 10f;
        }
    }
}