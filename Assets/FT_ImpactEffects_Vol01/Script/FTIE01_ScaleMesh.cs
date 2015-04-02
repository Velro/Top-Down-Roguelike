using UnityEngine;
using System.Collections;

public class FTIE01_ScaleMesh : MonoBehaviour {

	Transform myMesh;
	Vector3 origMeshScale;
	public float scaleMeshSize = 1f;

	public void ScaleMesh () {
		myMesh = GetComponent<Transform>();
		origMeshScale = myMesh.localScale;
		myMesh.localScale = origMeshScale * scaleMeshSize;

		scaleMeshSize = 1f;
	}
	
}
