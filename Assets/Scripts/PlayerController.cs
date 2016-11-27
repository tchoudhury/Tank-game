using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent (typeof(PlayerShoot))]
[RequireComponent (typeof(PlayerMotor))]
[RequireComponent (typeof(PlayerSetup))]
[RequireComponent (typeof(PlayerHealth))]
public class PlayerController : NetworkBehaviour {

	PlayerHealth pHealth;
	PlayerMotor pMotor;
	PlayerSetup pSetup;
	PlayerShoot pShoot;

	Vector3 originalPosition;
	NetworkStartPosition[] spawnPoints;

	public override void OnStartLocalPlayer(){
		originalPosition = transform.position;
		spawnPoints = GameObject.FindObjectsOfType<NetworkStartPosition> ();
	}

	// Use this for initialization
	void Start () {
		pHealth = GetComponent<PlayerHealth> ();
		pMotor = GetComponent<PlayerMotor> ();
		pSetup = GetComponent<PlayerSetup> ();
		pShoot = GetComponent<PlayerShoot> ();

		GameManager gm = GameManager.Instance;
	}

	Vector3 GetInput(){
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		return new Vector3 (h, 0, v);
	}

	void FixedUpdate(){
		if (!isLocalPlayer ||pHealth.isDead) {
			return;
		}
		Vector3 inputDirection = GetInput ();
		pMotor.MovePlayer (inputDirection);
	}

	void Update(){
		if (!isLocalPlayer || pHealth.isDead) {
			return;
		}

		if (Input.GetMouseButtonDown (0)) {
			pShoot.shoot ();
		}

		Vector3 inputDirection = GetInput ();
		if (inputDirection.sqrMagnitude > 0.25f) {
			pMotor.RotateChassis (inputDirection);
		}
		Vector3 turretDirection = Utility.GetWorldPointFromScreenPoint (Input.mousePosition, pMotor.turret.transform.position.y) - pMotor.turret.position;
		pMotor.RotateTurret (turretDirection);
	}

	void Disable(){
		StartCoroutine ("RespawnRoutine");
	}

	IEnumerator RespawnRoutine(){
		transform.position = GetRandomSpawnPositions();
		pMotor.rigidbody.velocity = Vector3.zero;
		yield return new WaitForSeconds (3f);
		pHealth.Reset ();
	}

	Vector3 GetRandomSpawnPositions(){
		if (spawnPoints != null) {
			if (spawnPoints.Length > 0) {
				NetworkStartPosition startPos = spawnPoints [Random.Range (0, spawnPoints.Length)];
				return startPos.transform.position;
			}
		}

		return originalPosition;
	}
}
