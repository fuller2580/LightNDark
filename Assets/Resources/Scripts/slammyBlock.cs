using UnityEngine;
using System.Collections;

public class slammyBlock : MonoBehaviour {
	public GameObject TargetA;
	Vector3 TargetB;
	bool goToA = true;
	bool working = false;
	public float dropSpeed = 1f;
	public float returnSpeed = 1f;
	public float pauseTime = 0.0f;
	bool canMove = false;
	bool activate = false;
	public SpriteRenderer spriteRenderer = null;

	// Use this for initialization
	void Start () {
		TargetB = transform.position;
		if(TargetA){
			canMove = true;
		}
		else print("missing Target GameObject for a slam block");
		if(!spriteRenderer) spriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		if(canMove && activate){
			if(goToA)transform.position = Vector3.MoveTowards(transform.position, TargetA.transform.position, (Time.deltaTime * dropSpeed));
			else transform.position = Vector3.MoveTowards(transform.position, TargetB, (Time.deltaTime * returnSpeed));

			if(transform.position == TargetA.transform.position && goToA) StartCoroutine(startToB());
			else if(transform.position == TargetB && !goToA) StartCoroutine(startToA());
		}
		else{
			if(spriteRenderer.color.a >= 0.1f) activate = true;
		}
	}

	IEnumerator startToA(){
		if(!working){
			activate = false;
			working = true;
			yield return new WaitForSeconds(0);
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
