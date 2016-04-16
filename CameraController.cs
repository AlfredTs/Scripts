using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float followSpeed = 10;
	private Vector3 initialPosition;
	private Vector3 neededPosition;
	private Vector3 initialOffset = new Vector3(7.5f, -6.6f,0);

	//private Quaternion initialRotation;


	private void Update() {
		if(target == null && Druid.instance!=null) {
			target = Druid.instance.transform;
			initialPosition = transform.position;
			//initialOffset = target.transform.position - transform.position;
			//initialOffset.z = 0;
			Debug.Log("Reporting the initial offset "+initialOffset);
		}
		else {
			neededPosition = new Vector3(target.position.x, target.position.y, initialPosition.z)-initialOffset;
			//transform.Translate((transform.position-neededPosition)*Time.deltaTime);
			transform.position+=(neededPosition-transform.position)*Time.deltaTime*followSpeed;
		}
	}
}
