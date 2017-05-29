using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MoveUpUI : MonoBehaviour {
	void Update () {
		if (GetComponent<Text> ().fontSize != 0)
			transform.Translate (Vector3.up * Time.deltaTime * 1);
	}
}
