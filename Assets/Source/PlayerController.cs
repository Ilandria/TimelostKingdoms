using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Todo: replace these references with references to specific view/movement controllers. This is just here for ease of development.
	[Header("References")]
	[SerializeField] private Rigidbody playerRigidbody = null;
	[SerializeField] private Transform playerRoot = null;

	[Header("Base Movement Stats")]
	[SerializeField] private AnimationCurve accelerationCurve;
	[SerializeField] private float sprintMultiplier = 0.67f;
	[SerializeField] private float backwardMultiplier = 0.33f;
	[SerializeField] private float turnForce = 25.0f;
	[SerializeField] private float jumpForce = 350.0f;

	// Multiplier for movement to make turn & move forces in the same order of magnitude.
	private readonly float moveForceScale = 100.0f;

	private Vector3 movementInput = Vector3.zero;
	private bool sprintInput = false;
	private bool crouchInput = false;
	private bool jumpInput = false;
	private Vector3 lookInput = Vector2.zero;
	private bool isGrounded = false;
	private readonly RaycastHit[] groundHitBuffer = new RaycastHit[1];

	private void FixedUpdate()
	{
		// Todo: may need to add a layer mask - depends if jumping off of unintentional things is an issue.
		isGrounded = Physics.RaycastNonAlloc(playerRoot.position + Vector3.up * 0.05f, Vector3.down, groundHitBuffer, 0.1f) > 0;

		if (isGrounded)
		{
			// These are the same on mouse * keyboard, but can be different on gamepad (since input vector isn't guaranteed to be normalized).
			Vector3 worldMovementInput = playerRoot.TransformVector(movementInput);
			Vector3 worldMovementInputNormalized = worldMovementInput.normalized;

			// Make sprinting apply to forward movement only.
			float sprintFactor = 1.0f + Mathf.Clamp01(Vector3.Dot(worldMovementInputNormalized, playerRoot.forward)) * (sprintInput ? (sprintMultiplier - 1.0f) : 0.0f);

			// Movement penalty for backward movement.
			float backwardFactor = 1.0f - Mathf.Clamp01(Vector3.Dot(worldMovementInputNormalized, -playerRoot.forward)) * (1.0f - backwardMultiplier);

			// Used to determine if the player is trying to accelerate with or against current movement - against movement shouldn't be penalized as much. Aligned = 1, 90 deg. = 0.5, 180 deg. = 0
			float velocityAlignment = Mathf.Clamp01(Vector3.Dot(worldMovementInputNormalized, playerRigidbody.velocity.normalized) * 0.5f + 0.5f);

			// Sample the acceleration curve to get the base force we should apply for the given current velocity.
			float acceleration = accelerationCurve.Evaluate(playerRigidbody.velocity.magnitude * velocityAlignment / sprintFactor) * backwardFactor;

			// Using non-normalized worldMovementInput here as gamepads provide continuous/analogue input instead of digital like keyboards.
			playerRigidbody.AddForce(acceleration * moveForceScale * worldMovementInput);

			if (jumpInput)
			{
				playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			}
		}

		playerRigidbody.AddTorque(Vector3.Cross(playerRoot.forward, playerRoot.TransformVector(lookInput)) * turnForce);
		jumpInput = false;
	}

	public void OnMovementInput(Vector3 movementInput)
	{
		this.movementInput = movementInput;
	}

	public void OnSprintInput(bool isSprintPressed)
	{
		sprintInput = isSprintPressed;
	}

	public void OnCrouchInput(bool isCrouchPressed)
	{
		crouchInput = isCrouchPressed;
	}

	public void OnJumpInput()
	{
		jumpInput = true;
	}

	public void OnLookInput(Vector3 lookInput)
	{
		this.lookInput = lookInput;
	}
}
