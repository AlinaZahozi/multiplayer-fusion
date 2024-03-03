using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastAttack : NetworkBehaviour
{
    [SerializeField] int Damage = 2; // The amount of damage this attack deals.
    [SerializeField] int points = 10; // The points awarded for a successful hit.

    // Input actions for attacking and determining the attack location, configured through the Unity Input System.
    [SerializeField] InputAction attack;
    [SerializeField] InputAction attackLocation;

    // The maximum distance the raycast will check for hits.
    [SerializeField] float shootDistance = 5f;

    // Reference to a 'Score' component, assumed to be on the same GameObject, for awarding points on successful hits.
    private Score playerScore;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Get the Score component attached to the same GameObject as this script.
        playerScore = GetComponent<Score>();
    }

    // OnEnable and OnDisable are called when the GameObject is enabled/disabled, ensuring input actions are only active when the object is active.
    private void OnEnable()
    {
        attack.Enable();
        attackLocation.Enable();
    }
    private void OnDisable()
    {
        attack.Disable();
        attackLocation.Disable();
    }

    // OnValidate is used here to ensure that the input actions are set up correctly, even if not configured in the editor.
    void OnValidate()
    {
        // Ensure there's an attack action, and it's set to a button press (e.g., mouse left button).
        if (attack == null)
            attack = new InputAction(type: InputActionType.Button);
        if (attack.bindings.Count == 0)
            attack.AddBinding("<Mouse>/leftButton");

        // Ensure there's an attack location action, set to read a Vector2 value (e.g., mouse position).
        if (attackLocation == null)
            attackLocation = new InputAction(type: InputActionType.Value, expectedControlType: "Vector2");
        if (attackLocation.bindings.Count == 0)
            attackLocation.AddBinding("<Mouse>/position");
    }

    // Update is called once per frame and handles the logic for performing raycast attacks.
    void Update()
    {
        // Early exit if this instance doesn't have authority to update game state.
        if (!HasStateAuthority) return;

        // Check if the attack action was performed this frame.
        if (attack.WasPerformedThisFrame())
        {
            // Read the current attack location from the input action, translating screen coordinates to a world point.
            Vector2 attackLocationInScreenCoordinates = attackLocation.ReadValue<Vector2>();

            // Use the main camera to convert the screen point to a ray in the game world.
            var camera = Camera.main;
            Ray ray = camera.ScreenPointToRay(attackLocationInScreenCoordinates);
            // Adjust the ray origin slightly forward to avoid self-collision.
            ray.origin += camera.transform.forward;

            // Visualize the ray in the scene view for debugging.
            Debug.DrawRay(ray.origin, ray.direction * shootDistance, Color.red, duration: 1f);

            // Perform the raycast to detect hits within shootDistance.
            if (Runner.GetPhysicsScene().Raycast(ray.origin, ray.direction * shootDistance, out var hit))
            {
                // Log details about the hit object for debugging.
                GameObject hitObject = hit.transform.gameObject;
                Debug.Log($"Raycast hit: name={hitObject.name} tag={hitObject.tag} collider={hit.collider}");

                // If the hit object has a Health component, deal damage to it.
                if (hitObject.TryGetComponent<Health>(out var health))
                {
                    Debug.Log("Dealing damage");
                    health.DealDamageRpc(Damage);

                    // If this GameObject has a Score component, award points for the successful attack.
                    if (playerScore != null)
                    {
                        playerScore.DealScoreRpc(points);
                    }
                }
            }
        }
    }
}
