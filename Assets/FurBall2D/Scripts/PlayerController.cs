﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {
	
	public float maxSpeed = 6f;
	public float jumpForce = 1000f;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public float verticalSpeed = 20;
	[HideInInspector]
	public bool lookingRight = true;
	bool doubleJump = false;
	public GameObject Boost;
	bool needLights = false;
	bool isLoading = false;
	private Animator cloudanim;
	public GameObject Cloud;

	int lives = 3;

	private Rigidbody2D rb2d;
	private Animator anim;
	private bool isGrounded = false;
	public shadowScript ss;
	Vector3 startSpot;

	public List<ghostAI> ghosts;
	public List<Image> livesImg;
	float volume = .5f;
	public float fear = 0;
	public Image fearBar;
	public GameObject loadingScreen;

	AsyncOperation sync;
	// Use this for initialization
	void Start () {
		if(ghosts == null){
			ghosts = new List<ghostAI>();
		}
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		//cloudanim = GetComponent<Animator>();
		DontDestroyOnLoad(this.gameObject);
		Cloud = GameObject.Find("Cloud");
		setStartSpot();
  		//cloudanim = GameObject.Find("Cloud(Clone)").GetComponent<Animator>();
	}


	void OnCollisionEnter2D(Collision2D collision2D) {
		
		if (collision2D.relativeVelocity.magnitude > 20){
			Boost = Instantiate(Resources.Load("Prefabs/Cloud"), transform.position, transform.rotation) as GameObject;
		//	cloudanim.Play("cloud");	

		}
			
	}

	void OnTriggerEnter2D(Collider2D col){

		if(col.gameObject.tag == "portal"){
			ghosts.Clear();
			transform.position = col.gameObject.transform.position;
			setStartSpot();
			string LN = col.gameObject.GetComponent<spinSpin>().levelName;
			isLoading = true;
			needLights = true;
			loadingScreen.SetActive(true);
			sync = SceneManager.LoadSceneAsync(LN);
			//Application.LoadLevel("level2");
			resetPos();
			StartCoroutine(this.gameObject.GetComponent<lightScript>().findLights());
		}

		if(col.gameObject.tag == "movingPlat"){
			this.transform.parent = col.transform;
		}
		if(col.gameObject.tag == "stick"){
			this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
		}
		if(col.gameObject.tag == "stickVert"){
			this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
		}

		if(col.gameObject.tag == "warp"){
			this.transform.position = col.gameObject.GetComponent<warp>().getWarpSpot();
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "movingPlat"){
			this.transform.parent = null;
		}
		if(col.gameObject.tag == "stick"){
			//this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		if(col.gameObject.tag == "stickVert"){
			this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}


	
	// Update is called once per frame
	void Update () {
		if(fear > 100) fear = 100;
		else if(fear < 0) fear = 0;
		fearBar.fillAmount = fear/100;
		ss.isGrounded = isGrounded;
	if (Input.GetButtonDown("Jump") && (isGrounded || !doubleJump))
		{
			rb2d.AddForce(new Vector2(0,jumpForce));

			if (!doubleJump && !isGrounded)
			{
				doubleJump = true;
				Boost = Instantiate(Resources.Load("Prefabs/Cloud"), transform.position, transform.rotation) as GameObject;
			//	cloudanim.Play("cloud");		
			}
		}


	if (Input.GetButtonDown("Vertical") && !isGrounded)
		{
			rb2d.AddForce(new Vector2(0,-jumpForce));
			Boost = Instantiate(Resources.Load("Prefabs/Cloud"), transform.position, transform.rotation) as GameObject;
			//cloudanim.Play("cloud");
		}
		if(this.gameObject.transform.position.y < -60){
			resetPos();
			loseLife();
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)){
			ghosts.Clear();
			SceneManager.LoadSceneAsync("Level1");
			transform.position = new Vector3(-5.24f,-1.54f,1f);
			setStartSpot();
			resetPos();
			StartCoroutine(this.gameObject.GetComponent<lightScript>().findLights());
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2)){
			ghosts.Clear();
			SceneManager.LoadSceneAsync("level2");
			transform.position = new Vector3(15f,6.65f,1);
			setStartSpot();
			resetPos();
			StartCoroutine(this.gameObject.GetComponent<lightScript>().findLights());
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3)){
			ghosts.Clear();
			SceneManager.LoadSceneAsync("level3");
			transform.position = new Vector3(13f,24f,1f);
			setStartSpot();
			resetPos();
			StartCoroutine(this.gameObject.GetComponent<lightScript>().findLights());
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4)){
			ghosts.Clear();
			SceneManager.LoadSceneAsync("level4");
			transform.position = new Vector3(1f,2f,1f);
			setStartSpot();
			resetPos();
			StartCoroutine(this.gameObject.GetComponent<lightScript>().findLights());
		}

		if(needLights){
			if(!isLoading){
				needLights = false;
				StartCoroutine(this.gameObject.GetComponent<lightScript>().findLights());
				loadingScreen.SetActive(false);
			}
		}

		if(isLoading){
			if(sync.isDone)isLoading = false;				
		}
	}


	void FixedUpdate()
	{
		if (isGrounded){
			doubleJump = false;
		}

		float hor = Input.GetAxis ("Horizontal");

		anim.SetFloat ("Speed", Mathf.Abs (hor/2));

		rb2d.velocity = new Vector2 (hor * maxSpeed, rb2d.velocity.y);
		  
		isGrounded = Physics2D.OverlapCircle (groundCheck.position, 0.15F, whatIsGround);

		anim.SetBool ("IsGrounded", isGrounded);

		if ((hor > 0 && !lookingRight)||(hor < 0 && lookingRight))
			Flip ();
		 
		anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().velocity.y);


		if(ghosts.Count > 0){
			for(int i = 0; i < ghosts.Count; i++){
				if(ghosts[i].distance < 3 && !ghosts[i].anim.GetBool("dead")){
					fear += Time.deltaTime;
				}
			}
		}
		fear -= Time.deltaTime*.1f;
	}

	void loseLife(){
		lives --;
		print(lives);
		if(lives>=0){
			for(int i = 0; i < livesImg.Count; i++){
				livesImg[i].enabled = false;
			}
			if(lives > 0){
				for(int i = 0; i < lives; i++){
					livesImg[i].enabled = true;
				}
			}
		}
		else endGame();
	}

	void endGame(){
		print("eventually will go to lose screen");
	}
	
	public void Flip()
	{
		lookingRight = !lookingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}
	public void setStartSpot(){
		Rigidbody2D rig = this.gameObject.GetComponent<Rigidbody2D>();
		rig.velocity = Vector3.zero;
		startSpot = this.transform.position;
	}
	void resetPos(){
		Rigidbody2D rig = this.gameObject.GetComponent<Rigidbody2D>();
		rig.velocity = Vector3.zero;
		this.gameObject.transform.position = startSpot;
	}
	public void setGhostVolume(float vol){
		volume = vol;
		for(int i = 0; i < ghosts.Count; i++){
			ghosts[i].GetComponent<AudioSource>().volume = vol;
		}
	}

	public float getVolume(){
		return volume;
	}
}
