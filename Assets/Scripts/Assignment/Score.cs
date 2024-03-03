using Fusion;
using UnityEngine;


public class Score : NetworkBehaviour
{
    [SerializeField] NumberField ScoreDisplay;

    // Networked attribute marks this property for synchronization over the network.
    // OnChanged specifies a method to call when NetworkedScore changes. The method is static and takes a Changed<Score> parameter.
    // NetworkedScore is an integer that holds the score value, initialized to 0.
    [Networked(OnChanged = nameof(NetworkedScoreChanged))]
    public int NetworkedScore { get; set; } = 0;

    // Method called automatically by Fusion when NetworkedScore changes.
    // It logs the new score and updates the score display.
    // Changed<Score> provides context about the change, including the instance of Score where the change occurred.
    private static void NetworkedScoreChanged(Changed<Score> changed)
    {
        Debug.Log($"Score changed to: {changed.Behaviour.NetworkedScore}");
        changed.Behaviour.ScoreDisplay.SetNumber(changed.Behaviour.NetworkedScore);
    }

    // Rpc attribute marks this method as a Remote Procedure Call (RPC) that can be executed across the network.
    // RpcSources.All means the RPC can be called from any client or the server.
    // RpcTargets.StateAuthority indicates that the execution target is the State Authority (usually the server).
    // DealScoreRpc is a method intended to be called to modify the score. It takes an integer 'points' to add to the current score.
    public void DealScoreRpc(int points)
    {
        Debug.Log("Received points on StateAuthority, modifying Networked variable");
        NetworkedScore += points; // Adds the received points to the networked score.
    }
}
