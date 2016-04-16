using UnityEngine;
using System.Collections;

public class Druid : MonoBehaviour {

	//Settings
	public float baseHealth = 100;

	public float baseMovementSpeed = 10;
	public float jumpForce = 10;
	public float shootCooldown = 0.5f;
	public float shootTimer;



	public Vector3[] touchNormals;

	public float miceMovementSpeed;
	public DruidSkin[] skins = new DruidSkin[5];

	//State
	public float curHealth;
	public float curMovmentSpeed;
	public shapeshiftStates curShape;
	public PhysState physState;
	public int[] shiftSpirits = new int[5];

	//Prefabs
	public Projectile bullet;

	//References
	private Rigidbody rb;
	private Transform shootingSocket;

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

	private void Awake() {
		foreach (DruidSkin skin in skins) skin.gameObject.SetActive(false);
		for(int i = 0; i< skins[(int)(shapeshiftStates.druid)].transform.childCount; i++) {
			if( skins[(int)(shapeshiftStates.druid)].transform.GetChild(i).name == "shootingSocket") {
				shootingSocket = skins[(int)(shapeshiftStates.druid)].transform.GetChild(i);
			}
		}
		if(shootingSocket == null) Debug.LogWarning("Shooting socket was not initiated!");
	}

	private void Start() {
		rb = GetComponent<Rigidbody>();

	}
	private void Update() {
		shootTimer+=Time.deltaTime;
	}
	public void ResetDruid(Vector3 location = new Vector3()) {
		curMovmentSpeed = baseMovementSpeed;
		curHealth = baseHealth;
		transform.position = location;
		physState = PhysState.Nan;
		shiftToState(shapeshiftStates.druid);
		if(rb==null) rb = GetComponent<Rigidbody>();
		rb.velocity = new Vector3();
	}

	private void Die() {
		
	}
	public void Fire() {
		if(curShape == shapeshiftStates.druid && shootingSocket!=null && shootTimer>=shootCooldown) {
			shootTimer = 0;
			Instantiate(bullet,shootingSocket.position, transform.rotation);
		}
	}
	#endregion

	#region movment
	public void UpdateMovement(Vector3 dir) {
		if(curShape!=shapeshiftStates.stone){
			
			transform.Translate(dir*curMovmentSpeed*Time.deltaTime, Space.World);
		}
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

	public void HitByTheDoor(Door it) {
		Die();
	}
	#endregion

	#region shapeShifting 
	public bool shiftToState(shapeshiftStates state) {
		
		skins[(int)(curShape)].gameObject.SetActive(false);
		skins[(int)(state)].gameObject.SetActive(true);

		curShape = state;

		switch (state) {
		case shapeshiftStates.druid:
			curMovmentSpeed = baseMovementSpeed;
			break;

		case shapeshiftStates.stone:
			curMovmentSpeed = 0;
			break;

		case shapeshiftStates.mouse:
			curMovmentSpeed = miceMovementSpeed;

			break;
		case shapeshiftStates.fish:
			break;
		case shapeshiftStates.project:
			curMovmentSpeed = baseMovementSpeed;
			break;
		}
		return true;
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
