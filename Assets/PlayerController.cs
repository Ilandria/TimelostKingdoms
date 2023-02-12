using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Todo: replace these references with references to specific view/movement controllers. This is just here for ease of development.
	[SerializeField] private Rigidbody playerRigidbody = null;
	[SerializeField] private Transform playerRoot = null;
	[SerializeField] private Transform playerHead = null;

	private Vector3 movementInput = Vector3.zero;
	private bool sprintInput = false;
	private bool crouchInput = false;
	private bool jumpInput = false;
	private Vector3 lookInput = Vector2.zero;

	private void FixedUpdate()
	{
		playerRigidbody.AddForce(playerRoot.TransformVector(movementInput) * 350.0f);
		playerRigidbody.AddTorque(Vector3.Cross(playerRoot.forward, playerRoot.TransformVector(lookInput)) * 5.0f);
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
