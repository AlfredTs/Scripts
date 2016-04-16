using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject druid;

	public float time;
	public int score;
	public Checkpoint[] checkPoints;
	public int checkpointToStartFrom;

	public Checkpoint checkpointToRestartFrom;
	public int checkedScore;

	private void Awake() {
		Druid d = ((GameObject)(Instantiate(druid))).GetComponent<Druid>();
		d.ResetDruid(checkPoints[checkpointToStartFrom].transform.GetChild(0).position);
	}

	#region Singelton
	private static GameController _instance;

	public static GameController instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameController>();
			}
			return _instance;
		}
	}
	#endregion

	// Use this for initialization
	#region SomeStuff
	public void PlayerFailedSwitching() {
		//may be display some stuff	
	}
	#endregion

	#region GameCycle
	public void StartGame() {
	}
	public void RestartGame() {
		time = 0;
		score = checkedScore;
		Druid.instance.ResetDruid(checkpointToRestartFrom.transform.GetChild(0).position);
	}

	public void PassedChecking(Checkpoint chckP) {
		checkpointToRestartFrom = chckP;
		checkedScore = score;
	}
	#endregion
}
