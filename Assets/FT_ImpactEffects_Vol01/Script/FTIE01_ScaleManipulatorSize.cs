using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class FTIE01_ScaleManipulatorSize : MonoBehaviour {

	float origManiScale;
	float origManiStrength;
	public float scaleManipulatorSize = 1f;
	PlaygroundParticlesC particles;


	public void ScaleManipulatorSize () {
		if (particles==null)
			particles = GetComponent<PlaygroundParticlesC>();
		foreach (ManipulatorObjectC manipulator in particles.manipulators) {
			origManiScale = manipulator.size;
			origManiStrength = manipulator.strength;
			manipulator.size = origManiScale * scaleManipulatorSize;
			manipulator.strength = origManiStrength * scaleManipulatorSize;
		}
		scaleManipulatorSize = 1f;
	}
	
}
