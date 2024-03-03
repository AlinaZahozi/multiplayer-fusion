using Fusion;
using UnityEngine;

public class Score : NetworkBehaviour
{
    [SerializeField] NumberField ScoreDisplay;

    [Networked(OnChanged = nameof(NetworkedScoreChanged))]
    public int NetworkedScore { get; set; } = 0;
    private static void NetworkedScoreChanged(Changed<Score> changed)
    {
        // Here you would add code to update the player's healthbar.
        Debug.Log($"Score changed to: {changed.Behaviour.NetworkedScore}");
        changed.Behaviour.ScoreDisplay.SetNumber(changed.Behaviour.NetworkedScore);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    // All players can call this function; only the StateAuthority receives the call.
    public void DealScoreRpc(int points)
    {
        // The code inside here will run on the client which owns this object (has state and input authority).
        Debug.Log("Received points on StateAuthority, modifying Networked variable");
        NetworkedScore += points;
    }
}