using UnityEngine;
using System.Collections;

public class Ogre : MonoBehaviour {
	
	//state
	public float health = 100;
	public aiState ogreState;
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
	public Transform[] patrolPoints = new Transform[2];

	//druid info
	public Vector3 directionToDruid;
	public Vector3 lastSeenPosition;
	public float distanceToDruid;
	public bool bDruidVisible;
	public bool bDruidAttackable;
	public float awarenesLevel;


	//references
	private Animator anim;

	public enum aiState {
		patrolling,
		aware,
		attacking,
		chasing
	}
	private void Update() {
		if(anim==null) anim = GetComponent<Animator>();
		directionToDruid = (Druid.instance.transform.position-transform.position).normalized;
		distanceToDruid = Vector3.Distance(transform.position, Druid.instance.transform.position);
		if((distanceToDruid<=attackRange && bCanDoRangeAttack) || (distanceToDruid<=meleAttackRange)) {
			bDruidAttackable = true;
			ogreState = aiState.attacking;
		}
		else if(distanceToDruid<=visionArea) {
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
					bDruidVisible = true;
					ogreState = aiState.chasing;
					lastSeenPosition = hit.point;
				}
			}
		}

		UpdateAI();
	}


	private void UpdateAI() {
		switch (ogreState) {
		case aiState.patrolling:
			patrolingTimer+=Time.deltaTime;
			if(patrolingTimer>patrolStopTime) {
				//go to next point;
			}


			break;
		case aiState.chasing:
			break;
		case aiState.attacking:
			break;
		case aiState.aware:
			break;
		default:
			break;
		}

		
	}
		
}
