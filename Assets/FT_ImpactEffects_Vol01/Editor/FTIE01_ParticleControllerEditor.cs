using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FTIE01_ParticleController))]
public class FTIE01_ParticleControllerEditor : Editor {
	
	
	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector();
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField ("Scale and Lifetime Editor");
		EditorGUILayout.LabelField ("------------------------------------------------------------------------------------------------------------------------------");

		FTIE01_ParticleController myScript = (FTIE01_ParticleController)target;
		serializedObject.Update();

		myScript.deadtime = EditorGUILayout.FloatField ("Dead Time", myScript.deadtime);


		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		myScript.scale = EditorGUILayout.FloatField ("Scaling Size", myScript.scale);
		
		if(GUILayout.Button("Scaling Particle Size",GUILayout.Width(200)))
		{
			myScript.ScaleParticle();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		myScript.scaleLife = EditorGUILayout.FloatField ("Scaling Lifetime", myScript.scaleLife);
		
		if(GUILayout.Button("Scaling Particle Lifetime",GUILayout.Width(200)))
		{
			myScript.ScaleLifetime();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();


		EditorGUILayout.LabelField ("Color Editor");
		EditorGUILayout.LabelField ("------------------------------------------------------------------------------------------------------------------------------");
		EditorGUILayout.PropertyField(serializedObject.FindProperty("particleSystems"),true);

		EditorGUILayout.PropertyField(serializedObject.FindProperty("particleColor"),true);


		EditorGUILayout.BeginHorizontal();
		
		if(GUILayout.Button("Get Particle Color"))
		{
			myScript.GetColor();
		}
		if(GUILayout.Button("Clear"))
		{
			myScript.ClearColor();
		}
		EditorGUILayout.EndHorizontal();
		
		serializedObject.ApplyModifiedProperties();
	}
}
