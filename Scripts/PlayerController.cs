﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 0.2f;
	public float rotation = 45.0f;
	public Bullet bullet;
	public float gunDamage = 50.0f;

	[HideInInspector]
	float angle;
	float mouseX, mouseY;
	public float bulletSpeed = 4.0f;
	int currentWeapon = 1;
	float[] damage = { 50.0f, 25.0f }; 
	float[] fireRate = { 0.5f, 0.1f };
	float dtime = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		getMovement ();
		getRotation ();
		getActions ();
	}

	void getMovement() {
		float moveX = Input.GetAxisRaw ("Horizontal");
		float moveY = Input.GetAxisRaw ("Vertical");
		Vector3 movement = new Vector3 (moveX * moveSpeed, moveY * moveSpeed, 0);
		transform.position += Vector3.ClampMagnitude(movement, moveSpeed) * Time.deltaTime;
	}

	void getRotation() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.rotation = Quaternion.LookRotation (Vector3.forward, mousePos - transform.position);
		transform.Rotate (new Vector3 (0, 0, 45.0f));
	}

	void getActions() {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentWeapon = 1;
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentWeapon = 2;
		}
		if (Input.GetMouseButton (0)) {
			fireBullet ();
		} else {
			if (dtime < fireRate [currentWeapon - 1]) {
				dtime += Time.deltaTime;
			} else {
				dtime = fireRate [currentWeapon - 1];
			}
		}
	}

	void fireBullet() {
		dtime += Time.deltaTime;
		if (dtime > fireRate [currentWeapon - 1]) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			float dirX = mousePos.x - transform.position.x;
			float dirY = mousePos.y - transform.position.y;
			Vector3 dir = new Vector3 (dirX, dirY, 0);
			if (Vector3.Magnitude (dir) < .01) {
				return;
			}
			Bullet b = (Bullet)Instantiate (bullet);
			b.player = this;
			b.transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
			b.direction = Vector3.ClampMagnitude (dir * 1000, 1.0f) * bulletSpeed;
			b.transform.rotation = transform.rotation;
			b.transform.Rotate (new Vector3 (0, 0, -45.0f));
			b.damage = damage [currentWeapon - 1];
			dtime = 0.0f;
		}
	}
}
