using UnityEngine;
using System.Collections;

public class fishy : MonoBehaviour {
	Vector2 centerPoint;
	Vector2 targetPos;
	public float range = 10f;
	public float speed = 2f;
	float moveSpeed;
	bool canMove = false;
	// Use this for initialization
	void Start () {
		centerPoint = new Vector2(this.gameObject.transform.position.x,this.gameObject.transform.position.y);
		StartCoroutine(pickTarget());
	}
	
	// Update is called once per frame
	void Update () {
		if(canMove){
			if(new Vector2(transform.position.x,transform.position.y) == targetPos){
				canMove = false;
				StartCoroutine(pickTarget());
			}
			else transform.position = Vector2.MoveTowards(new Vector2(transform.position.x,transform.position.y),targetPos,moveSpeed);
		}
	}

	IEnumerator pickTarget(){
		yield return new WaitForSeconds(1);
		targetPos = (new Vector2(Random.Range(-range,range),Random.Range(-range,range)) + centerPoint);
		Vector3 myScale = transform.localScale;
		if(targetPos.x > transform.position.x) myScale.x = -1;
		else myScale.x = 1;
		transform.localScale = myScale;
		moveSpeed = speed * Time.deltaTime;
		canMove = true;
	}
}
