﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

	private float speed = 6;
	bool gameOver = false;
	public GameObject particle;
	Rigidbody rb;
	private bool selected = false;


	//uruchamiana przed startem
	void Awake(){
		rb = GetComponent<Rigidbody>();
	}

	//Zmiana kierunku 
	void SwitchDirection(){
		
		if (rb.velocity.z > 0) {
			
			rb.velocity = new Vector3 (speed, 0, 0);

		} else if (rb.velocity.x > 0) {
			
			rb.velocity = new Vector3 (0, 0, speed);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (selected) {
			//Jeśli kula wyjdzie za krawędź - spada
			if (!Physics.Raycast (transform.position, Vector3.down, 5f) && transform.position.y < 1.85) {
			
				onGameOver ();
			}

			//Zmiana kierunku przy kliknięciu
			if (SwipeManager.instance.OnSwipe (SwipeDirection.Tap) && !gameOver) {
				SwitchDirection ();	
			}

			if (rb.velocity.z > 0) {
				transform.Rotate (new Vector3 (8, 0, 0), Space.World);
				;

			} else if (rb.velocity.x > 0) {
				transform.Rotate (new Vector3 (0, 0, -8), Space.World);
				;
			}

			if (SwipeManager.instance.OnSwipe (SwipeDirection.Up) && !gameOver && Physics.Raycast (transform.position, Vector3.down, 1f)) {
				rb.AddForce (Vector3.up * 6, ForceMode.Impulse);
			}
		}
	}


	void OnTriggerEnter(Collider col){
		
		Debug.Log (col.gameObject.tag);

		if (col.gameObject.tag == "Diamond") {
			
			//Uwuchomienie efektu wizualnego podczas niszczenia diamentu
			GameObject part = Instantiate (particle, col.gameObject.transform.position, Quaternion.identity) as GameObject;
			Destroy (col.gameObject);
			ScoreManager.instance.DiamondCollision ();
			Destroy (part, 1f);
		}
	}

	void OnCollisionEnter(Collision col) {
		
		if (col.gameObject.tag == "Obstacle"){
			
			onGameOver ();
		}
	}

	public void OnStart(){

		selected = true;
		rb.velocity = new Vector3 (speed, 0, 0);
	}

	private void onGameOver(){
	
		gameOver = true;
		rb.velocity = new Vector3 (0, -5f, 0);
		GameManager.instance.GameOver();

	}

}
