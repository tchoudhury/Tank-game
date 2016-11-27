﻿using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	private Camera mainCamera;

	// Use this for initialization
	void Start () {
		if (mainCamera == null) {
			mainCamera = Camera.main;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
	}
}
