using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;

	void Start ()
	{
		gameObject.transform.position = new Vector3(player.transform.position.x,3f,-26f);
		offset = transform.position - player.transform.position;
	}

	void LateUpdate ()
	{
		transform.position = player.transform.position + offset;
	}
}