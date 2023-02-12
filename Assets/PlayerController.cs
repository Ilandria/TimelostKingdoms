using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Todo: replace these references with references to specific view/movement controllers. This is just here for ease of development.
	[Header("References")]
	[SerializeField] private Rigidbody playerRigidbody = null;
	[SerializeField] private Transform playerRoot = null;

	[Header("Base Movement Stats")]
	[SerializeField] private float moveForce = 15.0f;
	[SerializeField] private float sprintBonus = 0.67f;
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
	private float groundedDrag = 0.0f;

	private void Start()
	{
		groundedDrag = playerRigidbody.drag;
	}

	private void FixedUpdate()
	{
		// Todo: change this to non-allocating raycast.
		if (Physics.Raycast(playerRoot.position + Vector3.up * 0.05f, Vector3.down, 0.1f))
		{
			playerRigidbody.drag = groundedDrag;
			Vector3 worldMovementInput = playerRoot.TransformVector(movementInput);

			// Make sprinting apply to forward movement only.
			float sprintMovementBonus = 1.0f + Mathf.Clamp01(Vector3.Dot(worldMovementInput, playerRoot.forward)) * (sprintInput ? sprintBonus : 0.0f);

			// Movement penalty for backward movement.
			float backwardMovementPenalty = 1.0f - Mathf.Clamp01(Vector3.Dot(worldMovementInput, -playerRoot.forward)) * (1.0f - backwardMultiplier);

			// Total movement force.
			float totalMoveForce = moveForce * sprintMovementBonus * backwardMovementPenalty;

			playerRigidbody.AddForce(worldMovementInput * totalMoveForce * moveForceScale);

			if (jumpInput)
			{
				playerRigidbody.AddForce(Vector3.up * jumpForce * sprintMovementBonus * backwardMovementPenalty, ForceMode.Impulse);
			}
		}
		else
		{
			playerRigidbody.drag = 0.0f;
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
		// Todo: remove this.
		this.lookInput.y = 0.0f;
	}
}
