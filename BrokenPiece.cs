using UnityEngine;
using System.Collections;

public class BrokenPiece : MonoBehaviour {

	public Rigidbody rb;
	public Collider col;
	public MeshRenderer mesh;

	private void Start() {
		rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		mesh = GetComponent<MeshRenderer>();
	}
}
