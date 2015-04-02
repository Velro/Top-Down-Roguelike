using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ParticlePlayground;

public class FTIE01_ParticleController : MonoBehaviour {
	//scale parameter
	public float scale = 1f;
	public float scaleLife = 1f;
	public float deadtime = 10;
	//lifetime parameter
	List<float> origLifetime = new List<float>(); 
	List<float> origSpeed = new List<float>(); 
	Animator[] anim;
	//color parameter
	public List<Gradient> particleColor = new List<Gradient>(); 
	//particleplayground
	public PlaygroundParticlesC[] particleSystems;
	//child script
	FTIE01_ScaleStateSize[] childScaleState;
	FTIE01_ParticleScaler[] childScaleParticle;
	FTIE01_ScaleManipulatorSize[] childScaleManipulator;
	FTIE01_ScaleMesh[] childScaleMesh;
	FTIE01_ScaleLight[] childScaleLight;
		
	public void ScaleParticle () {

		if (childScaleState==null){
			childScaleState = transform.GetComponentsInChildren<FTIE01_ScaleStateSize>();
		}
		if (childScaleParticle==null){
			childScaleParticle = transform.GetComponentsInChildren<FTIE01_ParticleScaler>();
		}
		if (childScaleManipulator==null){
			childScaleManipulator = transform.GetComponentsInChildren<FTIE01_ScaleManipulatorSize>();
		}
		if (childScaleMesh==null){
			childScaleMesh = transform.GetComponentsInChildren<FTIE01_ScaleMesh>();
		}
		if (childScaleLight==null){
			childScaleLight = transform.GetComponentsInChildren<FTIE01_ScaleLight>();
		}

		for (int i = 0; i < childScaleState.Length; i++) {
			childScaleState[i].scaleState = scale;
			childScaleState[i].ScaleStateSize();
		}
		for (int i = 0; i < childScaleParticle.Length; i++) {
			childScaleParticle[i].scaleSize = scale;
			childScaleParticle[i].ParticleScale();
		}
		for (int i = 0; i < childScaleManipulator.Length; i++) {
			childScaleManipulator[i].scaleManipulatorSize = scale;
			childScaleManipulator[i].ScaleManipulatorSize();
		}
		for (int i = 0; i < childScaleMesh.Length; i++) {
			childScaleMesh[i].scaleMeshSize = scale;
			childScaleMesh[i].ScaleMesh();
		}
		for (int i = 0; i < childScaleLight.Length; i++) {
			childScaleLight[i].scaleLightRangeSize = scale;
			childScaleLight[i].ScaleLightRange();
		}		
		scale = 1f;
	}

	public void ScaleLifetime () {

		anim = transform.GetComponentsInChildren<Animator>();
		for (int i = 0; i < particleSystems.Length; i++) {
			origLifetime.Add(particleSystems[i].lifetime);
			particleSystems [i].lifetime = origLifetime[i] * scaleLife;
		}
		for (int i = 0; i < anim.Length; i++) {
			origSpeed.Add(anim[i].speed);
			anim[i].speed = origSpeed[i]*(1/scaleLife);
		}	
		scaleLife = 1f;
	}


	public void GetColor () {
		if (particleColor.Count == 0){
			for (int i = 0; i < particleSystems.Length; i++) {
				particleColor.Add(particleSystems[i].lifetimeColor);
			}
		}
	}

	public void ClearColor () {
		if(particleColor != null)
			particleColor.Clear();
	}


	


	void Awake (){
		Destroy (gameObject, deadtime);
	}
}
