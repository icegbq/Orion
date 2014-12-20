using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed = 10f;

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;

	private const float rotateSpeed = 1.9f;

    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
	private Quaternion syncRotation = Quaternion.identity;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationY = 0F;
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        Vector3 syncVelocity = Vector3.zero;
		Quaternion syncRot = Quaternion.identity;

        if (stream.isWriting)
        {
            syncPosition = rigidbody.position;
            stream.Serialize(ref syncPosition);

            syncPosition = rigidbody.velocity;
            stream.Serialize(ref syncVelocity);

			syncRot = rigidbody.rotation;
			stream.Serialize(ref syncRot);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncVelocity);
			stream.Serialize(ref syncRot);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = rigidbody.position;
			syncRotation = syncRot;
        }
    }

    void Awake()
    {
        lastSynchronizationTime = Time.time;
    }

    void Update()
    {
        if (networkView.isMine)
		{
			InputRotation();
            InputMovement();
            InputColorChange();
        }
        else
        {
            SyncedMovement();
			camera.enabled = false;
        }
    }
 
    private void InputMovement()
    {
		Vector3 forward = transform.rotation * Vector3.forward;
		Vector3 right = transform.rotation * Vector3.right;
        if (Input.GetKey(KeyCode.W))
			transform.position = transform.position + (forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
			transform.position = transform.position + (-1 * forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
			transform.position = transform.position + (right * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
			transform.position = transform.position + (-1 * right * speed * Time.deltaTime);
    }

	private void InputRotation()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	
	private void SyncedMovement()
    {
        syncTime += Time.deltaTime;

        transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
		transform.rotation = syncRotation;
    }
  
    private void InputColorChange()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
    }

    [RPC] void ChangeColorTo(Vector3 color)
    {
        renderer.material.color = new Color(color.x, color.y, color.z, 1f);

        if (networkView.isMine)
            networkView.RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
    }
}
