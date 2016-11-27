using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : NetworkBehaviour {

	Rigidbody rbody;
	Collider col;

	public int speed = 100;
	List<ParticleSystem> particles;
	public float lifetime = 2f;
	public float damage = 1f;

	//public List<string> bounceTags;
	public List<string> collisionTags;

	// Use this for initialization
	void Start () {
		particles = GetComponentsInChildren <ParticleSystem> ().ToList ();
		rbody = GetComponent<Rigidbody> ();
		col = GetComponent<Collider> ();
		StartCoroutine ("SelfDestruct");
	}

	IEnumerator SelfDestruct(){
		yield return new WaitForSeconds (lifetime);
		col.enabled = false;
		rbody.velocity = Vector3.zero;
		rbody.Sleep ();

		foreach (ParticleSystem ps in particles) {
			ps.Stop ();
		}

		if (isServer) {
			Destroy (col.gameObject);
			foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>()) {
				m.enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionExit(Collision collision){
		if (rbody.velocity != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation (rbody.velocity);
		}
	}

	void OnCollisionEnter(Collision collision){
		CheckCollisions (collision);
	}

	void CheckCollisions(Collision collision){
		if (collision.collider.transform.parent != null) {
			if (collisionTags.Contains (collision.collider.transform.parent.tag)) {
				//Explode ();
				PlayerHealth health = collision.gameObject.GetComponentInParent<PlayerHealth> ();

				if (health != null) {
					health.Damage (damage);
				}
			}
		}
	}
}
