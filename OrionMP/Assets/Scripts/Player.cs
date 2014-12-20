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
			//gameObject.SetActive(false);
        }
    }
 
    private void InputMovement()
    {
		Vector3 forward = transform.rotation * Vector3.forward;
		Vector3 right = transform.rotation * Vector3.right;
        if (Input.GetKey(KeyCode.W))
			transform.Translate(forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
			transform.Translate(-1 * forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
			transform.Translate(right * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
			transform.Translate(-1 * right * speed * Time.deltaTime);
    }

	private void InputRotation()
	{
		transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X")*rotateSpeed);
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
