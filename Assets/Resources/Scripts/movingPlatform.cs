using UnityEngine;
using System.Collections;

public class movingPlatform : MonoBehaviour {
	public GameObject TargetA;
	public GameObject TargetB;
	bool goToA = false;
	bool working = false;
	public float speed = 1f;
	public float pauseTime = 0.0f;
	bool canMove = false;

	// Use this for initialization
	void Start () {
		if(TargetA && TargetB){
			canMove = true;
		}
		else print("missing Target GameObject for a moving platform");
	}
	
	// Update is called once per frame
	void Update () {
		if(canMove){
			if(goToA)transform.position = Vector3.MoveTowards(transform.position, TargetA.transform.position, (Time.deltaTime * speed));
			else transform.position = Vector3.MoveTowards(transform.position, TargetB.transform.position, (Time.deltaTime * speed));
			
			if(transform.position == TargetA.transform.position && goToA) StartCoroutine(startToB());
			else if(transform.position == TargetB.transform.position && !goToA) StartCoroutine(startToA());
		}
	}

	IEnumerator startToA(){
		if(!working){
			working = true;
			yield return new WaitForSeconds(pauseTime);
			goToA = true;
			working = false;
		}
	}

	IEnumerator startToB(){
		if(!working){
			working = true;
			yield return new WaitForSeconds(pauseTime);
			goToA = false;
			working = false;
		}
	}
}
