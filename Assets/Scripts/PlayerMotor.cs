using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerMotor : NetworkBehaviour {

    public Rigidbody rigidbody;

    public Transform chassis, turret;

    public float moveSpeed = 100f;
    public float chassisRotateSpeed = 1f;
    public float turretRotateSpeed = 3f;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}

	public void MovePlayer(Vector3 dir) {
		Vector3 moveDirection = dir * moveSpeed * Time.deltaTime;
        rigidbody.velocity = moveDirection;
    }

	public void FaceDirection(Transform xForm, Vector3 dir, float rotSpeed){
		if (dir != Vector3.zero && xForm != null) {
			Quaternion desiredRot = Quaternion.LookRotation (dir);
			xForm.rotation = Quaternion.Slerp (xForm.rotation, desiredRot, rotSpeed * Time.deltaTime);
		}
	}

	public void RotateChassis(Vector3 dir){
		FaceDirection (chassis, dir, chassisRotateSpeed);
	}

	public void RotateTurret(Vector3 dir){
		FaceDirection (turret, dir, turretRotateSpeed);
	}
}
