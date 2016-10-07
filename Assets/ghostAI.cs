using UnityEngine;
using System.Collections;

public class ghostAI : MonoBehaviour {
	GameObject player;
	public float speed = 1f;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(player.transform.position,this.transform.position);
		print(distance);
		if(distance < 50){
			moveToPlayer();
		}
		if(this.gameObject.GetComponent<SpriteRenderer>().color.a == 0) this.gameObject.GetComponent<Animator>().SetBool("dead", false);
		else this.gameObject.GetComponent<Animator>().SetBool("dead", true);
	}

	void moveToPlayer(){
		if(!this.gameObject.GetComponent<Animator>().GetBool("dead")){
			transform.position = Vector3.MoveTowards(this.transform.position,player.transform.position,Time.deltaTime*speed);
		}
	}
}
