using UnityEngine;
using System.Collections;

public class KeepRotation : MonoBehaviour {
	void FixedUpdate () {
		transform.rotation = Quaternion.Euler(new Vector3 (60, 0, 0));
	}
}
