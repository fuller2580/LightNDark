﻿using UnityEngine;
using System.Collections;

public class ghostAI : MonoBehaviour {
	GameObject player;
	public float speed;
	SpriteRenderer SR;
	[HideInInspector] public Animator anim;
	[HideInInspector] public float distance;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		speed = Random.Range(0.5f,2f);
		SR = this.gameObject.GetComponent<SpriteRenderer>();
		anim = this.gameObject.GetComponent<Animator>();
		player.GetComponent<PlayerController>().ghosts.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
		distance = Vector3.Distance(player.transform.position,this.transform.position);
		print(distance);
		if(distance < 20){
			moveToPlayer();
		}
		if(SR.color.a == 0) anim.SetBool("dead", false);
		else anim.SetBool("dead", true);
	}

	void moveToPlayer(){
		if(!anim.GetBool("dead")){
			transform.position = Vector3.MoveTowards(this.transform.position,player.transform.position,Time.deltaTime*speed);
		}
	}
}
