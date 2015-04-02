using UnityEngine;
using System.Collections;

public class FTIE01_ScaleLight : MonoBehaviour {
	
	Light myLight;
	float origLightScale;
	public float scaleLightRangeSize = 1f;
	
	public void ScaleLightRange () {
		myLight = GetComponent<Light>();
		origLightScale = myLight.range;
		myLight.range = origLightScale * scaleLightRangeSize;
		
		scaleLightRangeSize = 1f;
	}
	
}
