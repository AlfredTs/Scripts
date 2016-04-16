using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float bulletSpeed = 10;
	public float bulletDamage = 25;
	private Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}
	private void Update() {
		transform.Translate(-1*transform.right*bulletSpeed*Time.deltaTime);
	}

	private void OnTriggerEnter(Collider collisionInfo) {
		if(collisionInfo.gameObject.tag == Tags.Ogre) {
			collisionInfo.gameObject.GetComponent<Ogre>().TakeDamage(bulletDamage);
		}
	}
}
