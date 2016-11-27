using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GameManager : NetworkBehaviour {

	int minPlayers = 2;
	int maxPlayers = 4;

	[SyncVar]
	public int playerCount = 0;

	public Text MessageText;

	static GameManager instance;

	public static GameManager Instance{
		get{ 
			if (instance == null) {
				instance = GameObject.FindObjectOfType<GameManager> ();

				if (instance == null) {
					instance = new GameObject ().AddComponent<GameManager> ();
				}
			}
			return instance;
		}
	}

	void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	void Start(){
		StartCoroutine ("GameLoopRoutine");
	}

	IEnumerator GameLoopRoutine(){
		yield return StartCoroutine ("EnterLobby");
		yield return StartCoroutine ("PlayGame");
		yield return StartCoroutine ("EndGame");
	}

	IEnumerator EnterLobby(){
		if (MessageText != null) {
			MessageText.gameObject.SetActive (true);
			MessageText.text = "waiting for players";
		}

		while (playerCount < minPlayers) {
			DisablePlayers ();
			yield return null;
		}
	}

	IEnumerator PlayGame(){
		if (MessageText != null) {
			MessageText.gameObject.SetActive (false);
		}

		yield return null;
	}

	IEnumerator EndGame(){
		yield return null;
	}

	void SetPlayerState(bool state){
		PlayerController[] allPlayers = FindObjectsOfType<PlayerController> ();

		foreach (PlayerController p in allPlayers) {
			p.enabled = state;
		}
	}

	void EnablePlayers(){
		SetPlayerState (true);
	}

	void DisablePlayers(){
		SetPlayerState (false);
	}
}
