using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(GameController))]
public class GameControllerInspector : Editor {

	GameController gc;
	void OnEnable() {
		gc = (GameController)target;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		if(Application.isPlaying)
		{
			if(GUILayout.Button("Restart Game")) {
				gc.RestartGame();
			}
		}
	}
}
