using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
	[SerializeField]
	private Transform playerHead = null;

	private Vector3 targetOffset = Vector3.zero;
	private Vector3 targetLookOffset = Vector3.zero;

	private void Start()
	{
		transform.SetParent(null, true);
		targetOffset = transform.position - playerHead.position;
		targetLookOffset = transform.eulerAngles;
	}

	private void LateUpdate()
	{
		Vector3 offset = playerHead.TransformVector(targetOffset);
		transform.position = Vector3.Lerp(transform.position, playerHead.position + offset, 10.0f * Time.deltaTime);
		transform.eulerAngles = playerHead.eulerAngles + targetLookOffset;
	}
}
