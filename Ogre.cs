using UnityEngine;
using System.Collections;

public class Ogre : MonoBehaviour {
	
	//state
	public float health = 100;
	public aiState ogreState;
	private aiState prevState;
	public float patrolingTimer;


	//settings
	public float walkingSpeed = 10;
	public float visionArea = 30;
	public float attackDamage = 20;
	public float attackRange = 5;
	public bool bCanDoRangeAttack = false;
	public float meleAttackRange = 2;
	public float attackSpeed = 1;
	public float calmAwarenes = 3;
	public float attackedAwareness = 14;
	public float seenAwareness = 7;
	public float patrolStopTime = 4;
	public Vector3[] patrolPoints = new Vector3[2];
	public int curPatrolignPoint;

	//other stuff
	private bool bReachedDestination = true;
	//druid info
	public Vector3 directionToDruid;
	public Vector3 lastSeenPosition;
	public float distanceToDruid;
	public bool bDruidVisible;
	public bool bDruidAttackable;
	public float awarenesLevel;

	//reset stuff
	public Vector3 startingLocation;


	//references
	private Animator anim;

	public enum aiState {
		patrolling,
		aware,
		attacking,
		chasing
	}

	private void Start() {
		patrolingTimer = patrolStopTime;
		for(int i = 0; i<transform.childCount; i++) {
			if(transform.GetChild(i).tag == Tags.PatrolPoint) patrolPoints[i] = transform.GetChild(i).position;
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	private void Update() {
		if(anim==null) anim = GetComponent<Animator>();
		if(Druid.instance.curShape != shapeshiftStates.stone) {
			directionToDruid = (Druid.instance.transform.position-transform.position).normalized;
			distanceToDruid = Vector3.Distance(transform.position, Druid.instance.transform.position);
			if((distanceToDruid<=attackRange && bCanDoRangeAttack) || (distanceToDruid<=meleAttackRange)) {
				bDruidAttackable = true;
				//ogreState = aiState.attacking;
			}
			else bDruidAttackable = false;

			if(distanceToDruid<=visionArea) {
				
				Vector3 direction = Druid.instance.transform.position - transform.position;
				RaycastHit hit;
				Ray r = new Ray(transform.position + transform.up,direction.normalized);
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(r, out hit, visionArea))
				{

					// ... and if the raycast hits the player...
					if(hit.collider.gameObject.tag == Tags.Druid)
					{
						Debug.DrawLine (transform.position, hit.point, Color.cyan);

					}
				}

				bDruidVisible = true;
				//ogreState = aiState.chasing;
				lastSeenPosition = hit.point;
			}
		}

		UpdateAI();
	}


	private void UpdateAI() {
		if(ogreState!=prevState) {
			patrolingTimer = patrolStopTime;
			StopAllCoroutines();
			bReachedDestination = true;
		}
		switch (ogreState) {
		case aiState.patrolling:
			
			if(patrolingTimer>patrolStopTime) {
				//go to next point;
				StartCoroutine(GotoPoint(NextPatrolingPoint()));
				patrolingTimer = 0;
			}
			else if(bReachedDestination){
				patrolingTimer+=Time.deltaTime;
			}
			if(bDruidVisible && !bDruidAttackable) ogreState = aiState.chasing;
			else if(bDruidAttackable) ogreState = aiState.attacking;
			break;
		case aiState.chasing:
			if(!bReachedDestination) StartCoroutine(GotoPoint(lastSeenPosition));
			awarenesLevel = seenAwareness;
			if(!bDruidVisible) ogreState = aiState.aware;
			break;
		case aiState.attacking:
			Attack();
			awarenesLevel = attackedAwareness;
			if(!bDruidAttackable && bDruidVisible) ogreState = aiState.chasing;
			else if (!bDruidVisible) ogreState = aiState.aware;
			break;
		case aiState.aware:
			awarenesLevel-=Time.deltaTime;
			if(awarenesLevel<=calmAwarenes) ogreState = aiState.patrolling;
			break;
		}
		prevState = ogreState;
	}

	private void Attack() {
		Debug.Log("Should attack!");
	}
	private Vector3 NextPatrolingPoint() {
		curPatrolignPoint++;
		if(curPatrolignPoint>=patrolPoints.Length) curPatrolignPoint = 0;
		return patrolPoints[curPatrolignPoint];
	}

	IEnumerator GotoPoint(Vector3 point) {
		bReachedDestination = false;
		point.y = transform.position.y;
		point.z = transform.position.z;
		Vector3 dir = (point-transform.position).normalized;
		Debug.Log(Time.frameCount+" gotopoint coroutine in progress");
		while(Vector3.Distance(point,transform.position)>=1) {
			transform.Translate(dir*Time.deltaTime*walkingSpeed);
			yield return new WaitForEndOfFrame();
		}
		bReachedDestination = true;

	}
		
}
