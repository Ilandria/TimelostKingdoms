using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
	[SerializeField] private Camera playerCamera = null;
	[SerializeField] private CinemachineFreeLook cinemachineController = null;

	private void Start()
	{
		cinemachineController.transform.SetParent(null, true);
		playerCamera.transform.SetParent(null, true);
		ToggleCursor();
	}

	public void ToggleCursor()
	{
		Cursor.lockState = (Cursor.visible = !Cursor.visible) ? CursorLockMode.None : CursorLockMode.Locked;
	}

	public void OnZoomInput(float zoom)
	{
		cinemachineController.m_YAxis.m_InputAxisValue = zoom;
	}
}
