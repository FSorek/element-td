using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour {
	public Transform escMenu;
	public Transform tooltip;
	public Transform UpgradeSelectionPanel;

	void Update () {
		MenuOpen ();
	}

	void MenuOpen()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && Time.timeScale > 0f) {
			Time.timeScale = 0f;
			escMenu.gameObject.SetActive (true);
		}
		else if (Input.GetKeyDown (KeyCode.Escape) && Time.timeScale == 0f) {
			Time.timeScale = 1f;
			escMenu.gameObject.SetActive (false);
		}
    }

	public void ExitGame()
	{
		Application.Quit ();
		Debug.Log ("Game Closed!");
	}

	public void Resume()
	{
		Time.timeScale = 1f;
		escMenu.gameObject.SetActive (false);
	}

	public void speedUp3x()
	{
			Time.timeScale = 3f;
	}

	public void speedUp1x()
	{
			Time.timeScale = 1f;
	}

	public void setTooltipText(string text)
	{
		tooltip.GetChild (0).GetComponent<Text> ().text = text;
	}


	public void showTooltip()
	{
		tooltip.gameObject.SetActive (true);
		tooltip.GetComponent<RectTransform> ().position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y + 20, Input.mousePosition.z);
	}

	public void hideTooltip()
	{
		tooltip.gameObject.SetActive (false);
	}

	public void OnOffUpgradePanel()
	{
		UpgradeSelectionPanel.gameObject.SetActive (!UpgradeSelectionPanel.gameObject.activeInHierarchy);
	}
}
