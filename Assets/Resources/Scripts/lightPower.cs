using UnityEngine;
using System.Collections;

public class lightPower : MonoBehaviour {
	public float Radius = 0;
	public float Power = 0;
	public bool isMainLight = false;
	Vector2 holdVals;
	gameManager man;
	GameObject player;
	bool oneStart = false;
	[HideInInspector]public bool hitOnce = false;
	public bool isFireball = false;
	// Use this for initialization
	void Start () {
		if(isFireball) hitOnce = true;
		if(isMainLight){
			//DontDestroyOnLoad(this.gameObject);
			man = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManager>();
		}
		player = GameObject.FindGameObjectWithTag("Player");
		if(player != null) startInfo();

	}

	// Update is called once per frame
	void Update () {
		if(!oneStart){
			player = GameObject.FindGameObjectWithTag("Player");
			if(player != null) startInfo();
		}
		if(isMainLight) Radius = man.getPower();
	}
	public float getRadius(){
		return Radius;
	}
	public void setRadius(float rad){
		Radius = rad;
		//print(Radius);
	}
	public float getPower(){
		return Power;
	}
	public void setPower(float lp){
		Power = lp;
		//print(Radius);
	}

	void startInfo(){
		oneStart = true;
		player.GetComponent<lightScript>().lights.Add(this.gameObject);
	}

	public void fishEffect(){
		if(!hitOnce){
			hitOnce = true;
			holdVals = new Vector2(Radius,Power);
			setPower(0);
			man.setPower(0);
			StartCoroutine(endFishEffect());
		}
	}

	IEnumerator endFishEffect(){
		yield return new WaitForSeconds(10);
		setPower(holdVals.y);
		man.setPower(holdVals.x);
		hitOnce = false;
	}
}
