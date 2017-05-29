using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public string tooltipText;
	public Transform tooltip;

	void Start()
	{
		tooltip = GameObject.Find ("_GameManager").GetComponent<MenuManager> ().tooltip;
	}

	public void OnPointerEnter(PointerEventData data)
	{
		tooltip.gameObject.SetActive (true);
		tooltip.GetChild (0).GetComponent<Text> ().text = tooltipText;
		tooltip.GetComponent<RectTransform> ().position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y + 20, Input.mousePosition.z);
	}

	public void OnPointerExit(PointerEventData data)
	{
		tooltip.gameObject.SetActive (false);
	}
}
