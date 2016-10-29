using UnityEngine;
using System.Collections;

public class lightPower : MonoBehaviour {
	public float Power = 0;
	public bool isMainLight = false;
	gameManager man;
	lightScript player;
	// Use this for initialization
	void Start () {
		if(isMainLight)DontDestroyOnLoad(this.gameObject);
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<lightScript>();
		player.lights.Add(this.gameObject);
		man = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManager>();
	}

	// Update is called once per frame
	void Update () {
		if(isMainLight) Power = man.getPower();
	}
	public float getPower(){
		return Power;
	}
	public void setPower(float lp){
		Power = lp;
		print(Power);
	}
}
