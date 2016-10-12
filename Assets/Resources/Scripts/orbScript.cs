using UnityEngine;
using System.Collections;

public class orbScript : MonoBehaviour {
	public gameManager man;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			if(man) man.setPower(5f);
			Destroy(this.gameObject);
		}
	}
}
