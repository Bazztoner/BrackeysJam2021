using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class SimpleWaypointWalker : MonoBehaviour {
	public Waypoint current;
	public float movementSpeed;

	CharacterController characterController;

	void Awake() {
		characterController = GetComponent<CharacterController>();
	}

	void Update() {
		if (current == null)
			return;

		var toWaypoint = current.transform.position - transform.position;
		var direction = toWaypoint.normalized;
		var movementDelta = direction * movementSpeed * Time.deltaTime;
		var adjustedMovementDelta = movementDelta.sqrMagnitude > toWaypoint.sqrMagnitude ? toWaypoint : movementDelta;

		characterController.Move(adjustedMovementDelta);
		transform.forward = direction;

		if ((current.transform.position - transform.position).sqrMagnitude < 0.01)
			current = current.Next;
	}
}
