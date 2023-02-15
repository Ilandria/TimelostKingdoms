using UnityEngine;
using UnityEngine.Events;
using Context = UnityEngine.InputSystem.InputAction.CallbackContext;

public class InputProcessor : MonoBehaviour
{
	private void Start() => transform.SetParent(null, true);

	[Header("Movement")]
	[SerializeField] private UnityEvent<Vector3> onMovementInput = new();
	public void HandleMovementInput(Context context)
	{
		// Convert movement input to a vector where z is forward.
		Vector3 movementInput = context.ReadValue<Vector2>();
		movementInput.z = movementInput.y;
		movementInput.y = 0.0f;

		onMovementInput?.Invoke(movementInput);
	}

	[SerializeField] private UnityEvent<bool> onSprintInput = new();
	public void HandleSprintInput(Context context) => onSprintInput?.Invoke(context.ReadValue<float>() >= 0.5f);

	[SerializeField] private UnityEvent<bool> onCrouchInput= new();
	public void HandleCrouchInput(Context context) => onCrouchInput?.Invoke(context.ReadValue<float>() >= 0.5f);

	[SerializeField] private UnityEvent onJumpInput = new();
	public void HandleJumpInput(Context context) { if (context.ReadValue<float>() > 0.5f) { onJumpInput?.Invoke(); } }

	[Header("Camera")]
	[SerializeField] private UnityEvent<Vector3> onLookInput = new();
	public void HandleLookInput(Context context) => onLookInput?.Invoke((Vector3)context.ReadValue<Vector2>());

	[SerializeField] private UnityEvent<float> onZoomInput = new();
	public void HandleZoomInput(Context context) => onZoomInput?.Invoke(context.ReadValue<float>());

	[Header("Misc")]
	[SerializeField] private UnityEvent onToggleCursor = new();
	public void HandleToggleCursorInput(Context context) => onToggleCursor?.Invoke();
}