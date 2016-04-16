using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private Vector3 movementDirection;
	private Druid daDruid;

	// Update is called once per frame
	void Update () {
		movementDirection.x = -Input.GetAxis("Horizontal");
		//movementDirection.y = Input.GetAxis("Vertical");

		Druid.instance.UpdateMovement(movementDirection);

		if(Input.GetButtonDown("Jump")) {
			Druid.instance.DoJump();
		}

		if(Input.GetButtonDown("Fire1")) {
			Druid.instance.Fire();
		}

		if(Input.GetKeyDown("1")) {
			if(!Druid.instance.shiftToState(shapeshiftStates.druid)) GameController.instance.PlayerFailedSwitching();
		}

		if(Input.GetKeyDown("2")) {
			if(!Druid.instance.shiftToState(shapeshiftStates.stone)) GameController.instance.PlayerFailedSwitching();
		}

		if(Input.GetKeyDown("3")) {
			if(!Druid.instance.shiftToState(shapeshiftStates.mouse)) GameController.instance.PlayerFailedSwitching();
		}

		if(Input.GetKeyDown("4")) {
			if(!Druid.instance.shiftToState(shapeshiftStates.fish)) GameController.instance.PlayerFailedSwitching();
		}

		if(Input.GetKeyDown("5")) {
			if(!Druid.instance.shiftToState(shapeshiftStates.project)) GameController.instance.PlayerFailedSwitching();
		}

	}
}
