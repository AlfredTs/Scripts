using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	
	public enum doorStates {
		opened,
		closing,
		closed,
		opening
	}
	private const int lastEnum = doorStates.opening-doorStates.opened;

	public float timeOffset;
	public float[] intervals = new float[]{1f,1f,1f,1f};
	public float openY;
	public float closedY;

	private Vector3 closedPosition;
	private Vector3 openPosition;
	public bool bJammed = false;
	public doorStates curState = doorStates.closed;
	public float doorTimer;
	private Collider col;

	private void Start() {
		col = GetComponent<Collider>();
		closedY = transform.position.y;
		closedPosition = openPosition = transform.position;
		//openY+=closedY;
		openPosition.y+=openY;
		SwitchToNextState();
	}

	private void Update() {
		Vector3 initialDoorPosition = transform.position;
		doorTimer+=Time.deltaTime;

		if (curState == doorStates.closing && !bJammed) {
			//transform.position+=(closedPosition-transform.position)*(Time.deltaTime/intervals[(int)(curState)]);
			transform.position = Vector3.Lerp(transform.position, closedPosition, doorTimer/intervals[(int)(curState)]);
		}
		else if(curState == doorStates.opening && !bJammed) {
			//transform.position+=(openPosition-transform.position)*(Time.deltaTime/intervals[(int)(curState)]);
			transform.position = Vector3.Lerp(transform.position, openPosition, doorTimer/intervals[(int)(curState)]);
		}
		if(doorTimer>intervals[(int)(curState)]) {
			doorTimer-= intervals[(int)(curState)];
			SwitchToNextState();
		}

	}

	private void SwitchToNextState() {
		Debug.Log("Switchign from state "+curState);
		curState++;
		Debug.Log("To state "+curState);
		if((int)curState>lastEnum) curState = (doorStates)0;
		if(curState == doorStates.closed) col.isTrigger = false;
		else col.isTrigger = true;
		SwitchToState(curState);
		bJammed = false;
	}
	private void SwitchToState(doorStates stt) {
		curState = stt;

		//StopAllCoroutines();
		//StartCoroutine(MoveDoor(curState,intervals[(int)curState]));
	}

	private void OnTriggerEnter(Collider collisionInfo) {
		if(curState == doorStates.closing) {
			if(collisionInfo.gameObject.tag == Tags.Druid) {
				if(Druid.instance.curShape == shapeshiftStates.stone) {
					JammTheDoor();
				}
				else Druid.instance.HitByTheDoor(this);
			}
		}
	}

	private void JammTheDoor() {
		Debug.Log("JAMMMEEEEDD!");
		bJammed = true;
		//SwitchToState(doorStates.closed);
	}



}

