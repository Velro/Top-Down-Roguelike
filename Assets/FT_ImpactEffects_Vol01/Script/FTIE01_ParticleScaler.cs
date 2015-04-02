using UnityEngine;
using System.Collections;
using ParticlePlayground;


public class FTIE01_ParticleScaler : MonoBehaviour {

	public float scaleSize = 1f;
	public bool scaleVelocity = true;
	public bool scaleParticleSize = true;
	public bool scaleLifetimePositioning = true;
	public bool scaleOverflowOffset = true;
	public bool scaleScatterSize = true;

	float origVelocityScale;
	float origScale;
	float origLifetimePositioningScale;
	Vector3 origOverflowOffset;
	Vector3 origScatterScale;
	
	PlaygroundParticlesC particles;

	public void ParticleScale () {
		if (particles==null)
			particles = GetComponent<PlaygroundParticlesC>();

		origVelocityScale = particles.velocityScale;
		origScale = particles.scale;
		origLifetimePositioningScale = particles.lifetimePositioningScale;
		origOverflowOffset = particles.overflowOffset;
		origScatterScale = particles.scatterScale;


		if (scaleVelocity)
			particles.velocityScale = origVelocityScale*scaleSize;
		if (scaleParticleSize)
			particles.scale = origScale*scaleSize;
		if (scaleLifetimePositioning)
			particles.lifetimePositioningScale = origLifetimePositioningScale*scaleSize;
		if (scaleOverflowOffset)
			particles.overflowOffset = origOverflowOffset*scaleSize;
		if (scaleScatterSize)
			particles.scatterScale = origScatterScale*scaleSize;

		scaleSize = 1f;
	}

}
