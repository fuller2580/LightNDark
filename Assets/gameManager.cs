using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {
	float lightPower = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	public float getPower(){
		return lightPower;
	}
	public void setPower(float lp){
		lightPower = lp;
		print(lightPower);
	}
}
