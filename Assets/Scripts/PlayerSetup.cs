using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerSetup : NetworkBehaviour {

	[SyncVar(hook = "UpdateColor")]
	public Color playerColor;
	public string baseName = "PLAYER";

	[SyncVar(hook = "UpdateName")]
	public int playerNum = 1;
	public Text playerNameText;
		
	void Start(){
		if (!isLocalPlayer) {
			UpdateColor (playerColor);
			UpdateName (playerNum);
		}
	}
	public override void OnStartClient(){
		base.OnStartClient ();
		if (playerNameText != null) {
			playerNameText.enabled = false;
		}
	}

	public override void OnStartLocalPlayer(){
		base.OnStartLocalPlayer ();
		CmdSetupPlayer ();
	}

	void UpdateColor(Color color){
		MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer mesh in meshes) {
			mesh.material.color = color;
		}
	}

	void UpdateName(int num){
		if (playerNameText != null) {
			playerNameText.enabled = true;
			playerNameText.text = baseName + num.ToString ();
		}
	}

	[Command]
	void CmdSetupPlayer(){
		GameManager.Instance.AddPlayer (this);
		GameManager.Instance.playerCount++;
	}
}
