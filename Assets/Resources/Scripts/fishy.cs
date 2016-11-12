using UnityEngine;
using System.Collections;

public class fishy : MonoBehaviour {
	Vector2 centerPoint;
	Vector2 targetPos;
	Vector2 thisPos;
	Vector2 lightPos;
	public float range = 10f;
	public float speed = 2f;
	float lightRange = 6f;
	public float lightOff = 0.1f;
	GameObject tarLight;
	float moveSpeed;
	bool canMove = false;
	bool chasing = false;
	bool needsCheck = false;
	lightPower tarLP;
	// Use this for initialization
	void Start () {
		StartCoroutine(findLights());
		centerPoint = new Vector2(this.gameObject.transform.position.x,this.gameObject.transform.position.y);
		StartCoroutine(pickTarget());
	}
	
	// Update is called once per frame
	void Update () {
		thisPos = new Vector2(transform.position.x,transform.position.y);
		if(tarLight != null && !tarLP.hitOnce){
			lightPos = new Vector2(tarLight.transform.position.x,tarLight.transform.position.y);
			float distance = Vector2.Distance(lightPos,thisPos);
			if(distance < lightRange){
				if(distance < lightOff){
					chasing = false;
					needsCheck = true;
					tarLP.fishEffect();
				}
				else{
					chasing = true;
					needsCheck = true;
					transform.position = Vector2.MoveTowards(thisPos,lightPos,moveSpeed*2);
					checkDirection();
				}
			}
			else chasing = false;
		}
		if(canMove && !chasing){
			if(new Vector2(transform.position.x,transform.position.y) == targetPos){
				canMove = false;
				StartCoroutine(pickTarget());
			}
			else{
				if(needsCheck)checkDirection();
				needsCheck = false;
				transform.position = Vector2.MoveTowards(thisPos,targetPos,moveSpeed);
			}
		}
	}

	IEnumerator pickTarget(){
		yield return new WaitForSeconds(1);
		targetPos = (new Vector2(Random.Range(-range,range),Random.Range(-range,range)) + centerPoint);

		moveSpeed = speed * Time.deltaTime;
		canMove = true;
	}

	void checkDirection(){
		Vector3 myScale = transform.localScale;
		float posToCheck;

		if(chasing) posToCheck = lightPos.x;
		else posToCheck = targetPos.x;
		
		if(posToCheck > thisPos.x) myScale.x = -1;
		else myScale.x = 1;

		transform.localScale = myScale;
	}

	IEnumerator findLights(){
		yield return new WaitForSeconds(1);
		GameObject[] AllLights = GameObject.FindGameObjectsWithTag("LightObj");
		//print("Found "+AllLights.Length+" lights");
		for(int i = 0; i < AllLights.Length; i++){
			if(AllLights[i].GetComponent<lightPower>().isMainLight){
				tarLight = AllLights[i].gameObject;
				tarLP = AllLights[i].GetComponent<lightPower>();
				break;
			}
		}
	}
}
