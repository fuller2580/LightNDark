using UnityEngine;
using System.Collections;

public class breakBlock : MonoBehaviour {
	bool needOn = false;
	bool working = false;
	bool isOff = false;
	bool oneStart = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(needOn) turnOn();
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			if(!working && !isOff)StartCoroutine(turnOff(col.gameObject));
			print("collided");
		}
	}

	IEnumerator turnOff(GameObject player){
		working = true;
		player.GetComponent<PlayerController>().breakBlocks.Add(this.gameObject);
		yield return new WaitForSeconds(.5f);
		this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		this.gameObject.GetComponent<Collider2D>().enabled = false;
		isOff = true;
		working = false;
	}
	public void turnOn(){
		needOn = true;
		if(isOff){
			needOn = false;
			this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
			this.gameObject.GetComponent<Collider2D>().enabled = true;
			isOff = false;
		}
	}
}
