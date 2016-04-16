using UnityEngine;
using System.Collections;

public class BreakeableThing : MonoBehaviour {
	public float breakVeclocity = 1;
	private Collider col;
	private MeshRenderer mesh;

	private Vector3 breakingForce;
	private Vector3 breakingPoint;

	private BrokenPiece[] pieces;


	private void Start() {
		col = GetComponent<Collider>();
		mesh = GetComponent<MeshRenderer>();

		pieces = new BrokenPiece[transform.childCount];


		for(int i = 0; i<transform.childCount; i++) {
			Transform ch = transform.GetChild(i);
			pieces[i] = ch.GetComponent<BrokenPiece>();
	/*
			pieces[i].col = ch.GetComponent<Collider>();
			pieces[i].rb = ch.GetComponent<Rigidbody>();
			pieces[i].mesh = ch.GetComponent<MeshRenderer>();
			*/
			pieces[i].col.enabled = false;
			pieces[i].rb.isKinematic = true;
			pieces[i].mesh.enabled = false;
		}

	}
	private void OnCollisionEnter(Collision collisionInfo) {
		Debug.Log("Collided with "+collisionInfo.gameObject.name);
		if(Druid.instance.curShape == shapeshiftStates.stone && collisionInfo.relativeVelocity.magnitude>breakVeclocity) {
			breakingForce = collisionInfo.impulse;
			breakingPoint = collisionInfo.gameObject.transform.position; //would be better to make it with collision points, but should work anyway
			BreakTheThing();
		}
	}

	private void BreakTheThing() {
		float minDistanceToHitter = float.PositiveInfinity;
		Rigidbody rbToPush = pieces[0].rb;
		col.enabled = false;
		mesh.enabled = false;

		foreach (BrokenPiece p in pieces) {
			p.col.enabled = true;
			p.rb.isKinematic = false;
			p.mesh.enabled = true;
			//p.rb. = true;
			float dh = Vector3.Distance(p.rb.transform.position,breakingPoint);
			if(dh<minDistanceToHitter) {
				minDistanceToHitter = dh;
				rbToPush = p.rb;
			}
		}
		if(rbToPush!=null) rbToPush.AddForce(breakingForce);
		else pieces[0].rb.AddForce(breakingForce);

		//for now
		//gameObject.SetActive(false);
	}

}
