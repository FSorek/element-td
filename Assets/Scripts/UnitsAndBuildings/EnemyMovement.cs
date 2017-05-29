using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour {

    public bool isJeff;
	private Transform target;
	[HideInInspector]public int waypointIndex = 0;
	private UnityEngine.AI.NavMeshAgent agent;
	private bool isMoving;
	private Vector3 distance;
	void Start()
	{
		target = Waypoints.points [waypointIndex]; 
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        agent.Stop();
		GetComponent<UnityEngine.AI.NavMeshAgent> ().SetDestination (Waypoints.points [waypointIndex].position);
		isMoving = false;
	}

	void Update()
	{

		if (!isMoving) {
            agent.Resume();
			isMoving = true;
		}
		distance = transform.position - target.position;
		if (Mathf.Abs(distance.x) <= 3f && Mathf.Abs(distance.z) <= 3f) {
			GetNextWaypoint ();
		}
	}

	public void Repath()
	{
		agent.SetDestination (Waypoints.points [waypointIndex].position);
		isMoving = false;
	}

	void GetNextWaypoint()
	{
		if (waypointIndex >= Waypoints.points.Length - 1) {
            if(isJeff)
            {
                SceneManager.LoadScene("JeffIsKill");
                return;
            }
			GetComponent<EnemyStat> ().Heal ();
			waypointIndex = 0;
			target = Waypoints.points [waypointIndex];
			agent.SetDestination (Waypoints.points [waypointIndex].position);
			agent.Stop();
            isMoving = false;
			return;
		}
		waypointIndex++;
		target = Waypoints.points [waypointIndex];
		agent.SetDestination (Waypoints.points [waypointIndex].position);
        agent.Stop();
		isMoving = false;
	}

    public void setNextClosestWaypoint(int index)
    {
        waypointIndex = index;
        Start();
    }
}
