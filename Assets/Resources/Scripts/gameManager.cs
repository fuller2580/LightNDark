using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {
	float lightPower = 0;
	public GameObject player;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);

	}
	
	// Update is called once per frame
	void Update () {

	}
	public float getPower(){
		return lightPower;
	}
	public void setPower(float lp){
		lightPower = lp;
	//	print(lightPower);
	}

	public void startGame(){
		spawnPlayer();
		startLevel("levelTut");
	}

	void spawnPlayer(){
		if(player != null){
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			SpriteRenderer playerSR = player.GetComponent<lightScript>().spriteRenderer;
			playerSR.enabled = true;
			player.GetComponentInChildren<Camera>().enabled = true;
		}
		else print("player object missing on Game Manager");
	}

	void startLevel(string level){
		player.GetComponent<PlayerController>().loadLevel(level);
	}

}
