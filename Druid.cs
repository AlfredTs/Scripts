using UnityEngine;
using System.Collections;

public class Druid : MonoBehaviour {

	//Settings
	public float baseHealth = 100;
	public float baseMovementSpeed = 10;
	public float jumpForce = 10;
	public Vector3[] touchNormals;
	//State
	public float curHealth;
	public float curMovmentSpeed;
	public shapeshiftStates curShape;
	public PhysState physState;
	public int[] shiftSpirits = new int[5];

	//References
	private Rigidbody rb;

	#region Singelton
	private static Druid _instance;

	public static Druid instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<Druid>();
			}
			return _instance;
		}
	}
	#endregion


	#region LifeCycle 
	private void Start() {
		rb = GetComponent<Rigidbody>();
		curMovmentSpeed = baseMovementSpeed;
		curHealth = baseHealth;
	}

	private void ResetDruid(Vector3 location = new Vector3()) {
				
	}

	#endregion

	#region movment
	public void UpdateMovement(Vector3 dir) {
		transform.Translate(dir*curMovmentSpeed*Time.deltaTime, Space.World);
	}

	public bool DoJump() {

		if(physState!=PhysState.inAir) {
		//if(true){
			rb.AddForce(Vector3.up*jumpForce*100);
			return true;
		}
		else return false;

	}

	#endregion

	#region DealingWithCollisions
	private void OnCollisionStay(Collision collisionInfo) {
		DoCollisionAnalysis(collisionInfo);
	}
	private void OnCollisionExit(Collision collisionInfo) {
		DoCollisionAnalysis(collisionInfo);
	}

	private void DoCollisionAnalysis(Collision collisionInfo) {

		//No point in doing that if it's in the water
		if(physState==PhysState.inWater) return;
		ContactPoint[] touchPoints = collisionInfo.contacts;
		touchNormals = new Vector3[touchPoints.Length];
		bool bIsFloorContact = false;

		for(int i = 0; i<touchPoints.Length; i++) {
			touchNormals[i] = touchPoints[i].normal;
			if(touchPoints[i].normal.y >=0.98) {

				bIsFloorContact = true;
				break;
			}
		}

		if(!bIsFloorContact) physState = PhysState.inAir;
		else physState = PhysState.onGround;
	}

	private  void OnTriggerEnter(Collider other) { 
		if(other.gameObject.tag == Tags.Water) physState = PhysState.inWater;
	}
	private  void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == Tags.Water)	physState = PhysState.Nan;
	}
	#endregion

	#region shapeShifting 
	public bool shiftToState(shapeshiftStates state) {
		return false;
	}
	#endregion
}



public enum shapeshiftStates {
	druid,
	stone,
	mouse,
	fish,
	project
}

[System.Serializable]
public enum PhysState {
	Nan,
	onGround,
	inAir,
	inWater
}
