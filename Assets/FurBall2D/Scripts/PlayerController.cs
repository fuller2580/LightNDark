using UnityEngine;
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
	bool isSticking = false;

	int lives = 3;
	int ammo = 0;
	public int stickCount = 0;
	int fireballCount = 0;

	private Rigidbody2D rb2d;
	private Animator anim;
	private bool isGrounded = false;
	public shadowScript ss;
	public SpriteRenderer sr;
	Vector3 startSpot;

	[HideInInspector]public List<ghostAI> ghosts;
	public List<Image> livesImg;
	[HideInInspector]public List<GameObject> breakBlocks;
	float gVolume = .5f;
	public float fear = 0;
	public Image fearBar;
	public GameObject loadingScreen;

	int element = 0;
	public GameObject[] fireball;
	public GameObject waterball;
	public GameObject slimeball;

	AsyncOperation sync;
	// Use this for initialization
	void Start () {
		if(ghosts == null){
			ghosts = new List<ghostAI>();
		}
		if(breakBlocks == null){
			breakBlocks = new List<GameObject>();
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
			loadLevel(col.gameObject.GetComponent<spinSpin>().levelName);
		}

		if(col.gameObject.tag == "movingPlat"){
			this.transform.parent = col.transform;
		}
		if(col.gameObject.tag == "stick"){
			stick();
			isSticking = true;
			this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
			//this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
		}
		if(col.gameObject.tag == "stickVert"){
			this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
		}

		if(col.gameObject.tag == "warp"){
			this.transform.position = col.gameObject.GetComponent<warp>().getWarpSpot();
		}
		if(col.gameObject.tag == "tileTrigger"){
			if(col.gameObject.GetComponent<tileName>().tileOn != "")tilesOn(col.gameObject.GetComponent<tileName>().tileOn);
			if(col.gameObject.GetComponent<tileName>().tileOff != "")tilesOff(col.gameObject.GetComponent<tileName>().tileOff);
		}
		if(col.gameObject.tag == "toxic"){
			element = 3;
			ammo = 3;
			Vector4 curCol = sr.color;
			sr.color = new Vector4(0,1,0,curCol.w);
		}
		if(col.gameObject.tag == "water"){
			element = 2;
			ammo = 3;
			Vector4 curCol = sr.color;
			sr.color = new Vector4(0,0,1,curCol.w);
		}
		if(col.gameObject.tag == "lava"){
			element = 1;
			ammo = 3;
			Vector4 curCol = sr.color;
			sr.color = new Vector4(1,0,0,curCol.w);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "movingPlat"){
			this.transform.parent = null;
		}
		if(col.gameObject.tag == "stick"){
			unStick();
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

		if(Input.GetMouseButtonDown(0) && element != 0 && ammo != 0){
			shoot(element);
		}

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


	//if (Input.GetButtonDown("Vertical") && !isGrounded)
		//{
			//rb2d.AddForce(new Vector2(0,-jumpForce));
			//Boost = Instantiate(Resources.Load("Prefabs/Cloud"), transform.position, transform.rotation) as GameObject;
			//cloudanim.Play("cloud");
		//}
		if(this.gameObject.transform.position.y < -60){
			resetPos();
			loseLife();
			if(breakBlocks.Count > 0){
				for(int i = 0; i < breakBlocks.Count; i++){
					breakBlocks[i].GetComponent<breakBlock>().turnOn();
				}
				breakBlocks.Clear();
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)){
			loadLevel("level1");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2)){
			loadLevel("level2");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3)){
			loadLevel("level3");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4)){
			loadLevel("level4");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha5)){
			loadLevel("level5");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha6)){
			loadLevel("level6");
		}
		else if(Input.GetKeyDown(KeyCode.L)){
		//	lvl3TilesOn();
		}
		else if(Input.GetKeyDown(KeyCode.P)){
		//	lvl3TilesOff();
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
		float ver = Input.GetAxis("Vertical");

		anim.SetFloat ("Speed", Mathf.Abs (hor/2));

		if(!isSticking)rb2d.velocity = new Vector2 (hor * maxSpeed, rb2d.velocity.y);
		else rb2d.velocity = new Vector2 (hor * maxSpeed, ver * maxSpeed);
		  
		isGrounded = Physics2D.OverlapCircle (groundCheck.position, 0.15F, whatIsGround);

		anim.SetBool ("IsGrounded", isGrounded);

		if ((hor > 0 && !lookingRight)||(hor < 0 && lookingRight))
			Flip ();
		 
		anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().velocity.y);


		if(ghosts.Count > 0){
			for(int i = 0; i < ghosts.Count; i++){
				if(ghosts[i]){
				if(ghosts[i].distance < 3 && !ghosts[i].anim.GetBool("dead")){
					fear += Time.deltaTime;
				}
				}
				else ghosts.RemoveAt(i);
			}
		}
		fear -= Time.deltaTime*.1f;
	}

	void shoot(int el){
		ammo --;
		Vector4 curCol = sr.color;
		switch(el){
		case 1:
			fireball[fireballCount].transform.position = this.transform.position;
			fireball[fireballCount].transform.rotation = Quaternion.identity;
			fireball[fireballCount].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			fireball[fireballCount].GetComponent<projectile>().shootFireball();
			fireballCount++;
			if(fireballCount > fireball.Length-1) fireballCount = 0;
			switch(ammo){
				case 2:
					sr.color = new Vector4(1f,.25f,.25f,curCol.w);
					break;
				case 1:
					sr.color = new Vector4(1f,.5f,.5f,curCol.w);
					break;
				default:
					break;
			}
			break;
		case 2:
			GameObject WB = Instantiate(waterball, this.transform.position, Quaternion.identity) as GameObject;
			WB.GetComponent<projectile>().setPlayer(this.gameObject);
			switch(ammo){
				case 2:
					sr.color = new Vector4(.25f,.25f,1f,curCol.w);
					break;
				case 1:
					sr.color = new Vector4(.5f,.5f,1f,curCol.w);
					break;
				default:
					break;
			}
			break;
		case 3:
			Instantiate(slimeball, this.transform.position, Quaternion.identity);
			switch(ammo){
				case 2:
					sr.color = new Vector4(.25f,1f,.25f,curCol.w);
					break;
				case 1:
					sr.color = new Vector4(.5f,1f,.5f,curCol.w);
					break;
				default:
					break;
			}
			break;
		}
		if(ammo == 0) sr.color = new Vector4(1f,1f,1f,curCol.w);
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
	void tilesOff(string tag){
		GameObject[] tilesMang;
		tilesMang = GameObject.FindGameObjectsWithTag(tag);
		for(int i = 0; i < tilesMang.Length; i++){
			if(tilesMang[i].GetComponent<lightScript>())tilesMang[i].GetComponent<lightScript>().enabled = false;
		}
	}
	void tilesOn(string tag){
		GameObject[] tilesMang;
		tilesMang = GameObject.FindGameObjectsWithTag(tag);
		for(int i = 0; i < tilesMang.Length; i++){
			if(tilesMang[i].GetComponent<lightScript>())tilesMang[i].GetComponent<lightScript>().enabled = true;
		}
	}

	public void loadLevel(string level){
		ghosts.Clear();
		breakBlocks.Clear();
		resetFire();
		transform.position = getLevelPosition(level);
		setStartSpot();
		isLoading = true;
		needLights = true;
		loadingScreen.SetActive(true);
		sync = SceneManager.LoadSceneAsync(level);
		//Application.LoadLevel("level2");
		resetPos();
		StartCoroutine(this.gameObject.GetComponent<lightScript>().findLights());
	}

	Vector3 getLevelPosition(string level){
		switch(level){
		case "level1":
			return new Vector3(-5.24f,-1.54f,1f);
		case "level2":
			return new Vector3(15f,6.65f,1);
		case "level3":
			return new Vector3(13f,24f,1f);
		case "level4":
			return new Vector3(1f,2f,1f);
		case "level5":
			return new Vector3(-15.5f,0f,1f);
		case "level6":
			return new Vector3(0f,1f,1);
		case "level7":
			return new Vector3(15f,6.65f,1);
		case "level8":
			return new Vector3(15f,6.65f,1);
		case "level9":
			return new Vector3(15f,6.65f,1);
		case "level10":
			return new Vector3(15f,6.65f,1);
		default:
			return Vector3.zero;
		}
	}
	public void Flip(){
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
	void resetFire(){
		for(int i = 0; i < fireball.Length; i++){
			fireball[i].transform.position = new Vector3(-700,-700,0);
			fireball[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
		}
	}
	public void setGhostVolume(float vol)
    {
        gVolume = vol;
        NewMethod(vol);
    }

    private void NewMethod(float vol)
    {
        for (int i = 0; i < ghosts.Count; i++)
        {
            ghosts[i].GetComponent<AudioSource>().volume = vol;
        }
    }

    void stick(){
		if(!isSticking){
			isSticking = true;
			this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
			//this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
		}
		stickCount++;
	}

	public void unStick(){
		stickCount --;
		if(stickCount <= 0){
			isSticking = false;
			this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 6.1f;
			//this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		//this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	public float getVolume(){
		return gVolume;
	}
}
