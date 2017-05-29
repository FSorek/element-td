using UnityEngine;
using System.Collections;

public class FrontRotation : MonoBehaviour {

	public float speed;
	void Update () {
		transform.Rotate(Vector3.forward * Time.deltaTime * speed, Space.World);
	}
}
