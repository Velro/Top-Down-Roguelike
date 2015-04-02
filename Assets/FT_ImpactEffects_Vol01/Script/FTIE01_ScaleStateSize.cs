using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class FTIE01_ScaleStateSize : MonoBehaviour {

	float origStateScale;
	public float scaleState = 1f;
	PlaygroundParticlesC particles;


	public void ScaleStateSize () {
		if (particles==null)
			particles = GetComponent<PlaygroundParticlesC>();
		foreach (ParticleStateC activeState in particles.states) {
			origStateScale = activeState.stateScale;
			activeState.stateScale = origStateScale * scaleState;
		}
		scaleState = 1f;
	}


}
