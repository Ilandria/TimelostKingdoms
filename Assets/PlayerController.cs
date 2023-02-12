using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Todo: replace these references with references to specific view/movement controllers. This is just here for ease of development.
	[SerializeField] private Rigidbody playerRigidbody = null;
	[SerializeField] private Transform playerRoot = null;
	[SerializeField] private Transform playerHead = null;

	[Header("Base Movement Stats")]
	[SerializeField] private float moveForce = 15.0f;
	[SerializeField] private float turnForce = 25.0f;
	private float moveForceScale = 100.0f;

	private Vector3 movementInput = Vector3.zero;
	private bool sprintInput = false;
	private bool crouchInput = false;
	private bool jumpInput = false;
	private Vector3 lookInput = Vector2.zero;

	private void FixedUpdate()
	{
		playerRigidbody.AddForce(playerRoot.TransformVector(movementInput) * moveForce * moveForceScale);
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
