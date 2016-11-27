using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerShoot : NetworkBehaviour {

	public Rigidbody bulletPrefab;
	public Transform bulletSpawn;

	public int shotsPerBurst = 2;
	public int shotsLeft;
	public bool isReloading;
	public float reloadTime = 1f;

	// Use this for initialization
	void Start () {
		shotsLeft = shotsPerBurst;
		isReloading = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void shoot(){
		if (isReloading || bulletPrefab == null) {
			return;
		}

		CmdShoot ();

		shotsLeft--;

		if (shotsLeft <= 0) {
			StartCoroutine ("Reload");
		}
	}

	[Command]
	void CmdShoot(){
		Bullet bullet = null;
		bullet = bulletPrefab.GetComponent<Bullet> ();

		Rigidbody rbody = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;

		if (rbody != null) {
			rbody.velocity = bullet.speed * bulletSpawn.transform.forward;
			NetworkServer.Spawn (rbody.gameObject);
		}
	}

	IEnumerator Reload(){
		isReloading = true;
		shotsLeft = shotsPerBurst;

		yield return new WaitForSeconds (reloadTime);
		isReloading = false;
	}
}
