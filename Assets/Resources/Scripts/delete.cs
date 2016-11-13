using UnityEngine;
using System.Collections;

public class delete : MonoBehaviour {
	public float t = 0f;
	bool playerInZone = false;
	GameObject player;
	// Use this for initialization
	void Start () {
		StartCoroutine(end());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			playerInZone = true;
			player = col.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			playerInZone = false;
		}
	}

	IEnumerator end(){
		yield return new WaitForSeconds(t);
		switch(this.gameObject.tag){
		case "stick":
			if(playerInZone){
				player.GetComponent<PlayerController>().unStick();
			}
			break;
		default:
			break;
		}
		Destroy(this.gameObject);
	}
}
