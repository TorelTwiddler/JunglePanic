using UnityEngine;
using System.Collections;

public class ParticleDirector : MonoBehaviour {

	public Transform target;
	public float timeBeforeZoom = 1.0f;
	public float timeZooming = 1.0f;

	private ParticleSystem particleSystem;
	private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
	
	private float beforeZoomEndTime = Mathf.Infinity;

	// Use this for initialization
	void Start () {
		particleSystem = GetComponent<ParticleSystem>();
		beforeZoomEndTime = Time.time + timeBeforeZoom;
	}
	
	// Update is called once per frame
	void Update () {
		// Destry the particle system when it's done.
		if(!particleSystem.IsAlive()) {
			Destroy(gameObject);
		}
	}
	
	void LateUpdate() {
		if (Time.time > beforeZoomEndTime) {
			int length = particleSystem.GetParticles(particles);
			int i = 0;
			float beginningTime = beforeZoomEndTime;
			float endTime = beginningTime + timeZooming;

			while (i < length) {

				//Target is a Transform object
				Vector3 direction = target.position - particles[i].position;

				//float variableSpeed = (particleSystem.startSpeed / (particles[i].lifetime + 0.1f)) + particles[i].startLifetime;
				//float speed = direction.magnitude / timeZooming;
				
				direction.Normalize();
				
				// particles[i].position += direction * variableSpeed * Time.deltaTime;
				particles[i].position = Vector3.Lerp(particles[i].position, target.position, (Time.time - beginningTime) / (endTime - beginningTime));

				if(Vector3.Distance(target.position, particles[i].position) < 1.0f) {
					particles[i].lifetime = -0.1f; //Kill the particle
				}

				i++;

			}

			particleSystem.SetParticles(particles, length);
		}
	}
}
