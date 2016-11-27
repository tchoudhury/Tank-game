using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHealth : NetworkBehaviour {

	[SyncVar(hook="UpdateHealthBar")]
	public float currentHealth;

	public float maxHealth;

	[SyncVar]
	public bool isDead;

	public RectTransform healthBar;
	public GameObject deathPrefab;

	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage (float damage){
		if (!isServer) {
			return;
		}

		currentHealth -= damage;

		if (currentHealth <= 0 && !isDead) {
			isDead = true;
			RpcDie ();
		}
	}
		
	void UpdateHealthBar(float value){
		if (healthBar != null) {
			healthBar.sizeDelta = new Vector2 (value / maxHealth * 100f, healthBar.sizeDelta.y);
		}
	}

	[ClientRpc]
	void RpcDie(){
		SetActiveState (false);
		gameObject.SendMessage ("Disable");
	}

	void SetActiveState(bool state){
		foreach (Collider col in GetComponentsInChildren<Collider>()) {
			col.enabled = state;
		}

		foreach (Canvas c in GetComponentsInChildren<Canvas>()) {
			c.enabled = state;
		}

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = state;
		}
	}

	public void Reset(){
		currentHealth = maxHealth;
		SetActiveState (true);
		isDead = false;
	}
}
