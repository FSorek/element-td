using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode	]
[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemTest : MonoBehaviour {


	ParticleSystem PS;
	ParticleSystem.Particle[] particles;
	float angle;


	public float minSpeed;
	public float maxSpeed;




	// Use this for initialization
	void Start () {
		PS = this.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame


	void LateUpdate () {
		if (PS == null)
			PS = GetComponent<ParticleSystem>();

		if (particles == null || particles.Length < PS.main.maxParticles)
			particles = new ParticleSystem.Particle[PS.main.maxParticles]; 

		int numParticlesAlive = PS.GetParticles(particles);
		//int numParticlesInRange;




		for (int i = 0; i < numParticlesAlive; i++)
		{

			Vector3 partForce = particles [i].position - transform.position;
			Vector3 velo = (Vector3.Project(particles [i].velocity, Vector3.Cross(Vector3.up, partForce)) + Vector3.Project(particles [i].velocity, Vector3.up)).normalized * Mathf.Lerp(minSpeed,maxSpeed,(Vector3.Distance(particles[i].position, transform.position)/PS.shape.radius));

			particles [i].velocity = new Vector3(velo.x, 0, velo.z);

			//particles [i].rotation += 5f;
		}


		// Apply the particle changes to the particle system
		PS.SetParticles(particles, numParticlesAlive);
	}
}
