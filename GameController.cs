using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

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


}
