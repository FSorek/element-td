using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButtonOnOff : MonoBehaviour {

    public GameObject targetObject;

    public void setRecusiveOnOff()
    {
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
