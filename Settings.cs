using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	public float[] FastDoorIntervals = new float[]{0.5f,0.2f,0.5f,0.2f};
	/*
	#region Singelton
	private static Settings _instance;

	public static Settings instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<Settings>();
			}
			return _instance;
		}
	}
	#endregion
*/


}
