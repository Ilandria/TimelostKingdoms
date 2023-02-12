using UnityEngine;
using UnityEngine.Events;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class InputHandler : MonoBehaviour
{
	[SerializeField] private UnityEvent<Vector3> onMovementInput = new();
	public void HandleMovementInput(CallbackContext context)
	{
		Vector3 movementInput = context.ReadValue<Vector2>();
		movementInput.z = movementInput.y;
		movementInput.y = 0.0f;

		onMovementInput?.Invoke(movementInput);
	}

	[SerializeField] private UnityEvent<bool> onSprintInput = new();
	public void HandleSprintInput(CallbackContext context) => onSprintInput?.Invoke(context.ReadValue<float>() >= 0.5f);

	[SerializeField] private UnityEvent<bool> onCrouchInput= new();
	public void HandleCrouchInput(CallbackContext context) => onCrouchInput?.Invoke(context.ReadValue<float>() >= 0.5f);

	[SerializeField] private UnityEvent onJumpInput = new();
	public void HandleJumpInput(CallbackContext context)
	{
		if (context.ReadValue<float>() >= 0.5f)
		{
			onJumpInput?.Invoke();
		}
	}

	[SerializeField] private UnityEvent<Vector3> onLookInput = new();
	public void HandleLookInput(CallbackContext context) => onLookInput?.Invoke((Vector3)context.ReadValue<Vector2>());
}
