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
		TargetB = transform.localPosition;
		if(TargetA){
			canMove = true;
		}
		else print("missing Target GameObject for a slam block");
		if(!spriteRenderer) spriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		if(canMove && activate){
			if(goToA)transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetA.transform.localPosition, (Time.deltaTime * dropSpeed));
			else transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetB, (Time.deltaTime * returnSpeed));

			if(transform.localPosition == TargetA.transform.localPosition && goToA) StartCoroutine(startToB());
			else if(transform.localPosition == TargetB && !goToA) StartCoroutine(startToA());
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
