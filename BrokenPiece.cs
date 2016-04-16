using UnityEngine;
using System.Collections;

public class BrokenPiece : MonoBehaviour {

	public Rigidbody rb;
	public Collider col;
	public MeshRenderer mesh;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		mesh = GetComponent<MeshRenderer>();
	}
}
