using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	//public 
	public checkpointState curState;

	private  void OnTriggerEnter(Collider other) { 
		if(other.gameObject.tag == Tags.Druid && curState== checkpointState.empty) {
			SaveTheCheckpoint();
		}
	}

	private void SaveTheCheckpoint() {
		GameController.instance.PassedChecking(this);
	}
	public enum checkpointState {
		empty,
		saved,
		justSpawned
	}

}

