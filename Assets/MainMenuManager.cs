using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public Transform highlight;
    public LayerMask towerLayer;
    private Ray ray;
    public GameObject[] towers = new GameObject[6];
    // Use this for initialization
    public void Button_StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
	
    public void Button_Options()
    {

    }

    public void Button_Tutorial()
    {

    }

    public void Button_Exit()
    {
        Application.Quit();
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, towerLayer))
        {
            GameObject o;
            if (hit.collider.transform.FindChild("menuHighlight"))
            {
                o = hit.collider.transform.FindChild("menuHighlight").gameObject;
                o.SetActive(true);
            }
            else
            {
                GameObject h = Instantiate(highlight.gameObject, hit.collider.transform.position, Quaternion.identity, hit.collider.transform);
                h.name = highlight.name;
            }
        } else
        {
            foreach (GameObject tower in towers)
            {
                if (tower.transform.FindChild("menuHighlight"))
                {
                    tower.transform.FindChild("menuHighlight").gameObject.SetActive(false);
                }
            }
        }
    }
}
