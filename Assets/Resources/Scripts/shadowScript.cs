using UnityEngine;
using System.Collections;

public class shadowScript : MonoBehaviour {
	public bool isGrounded = false;
	public GameObject walkAnim;
	SpriteRenderer sr;
	GameObject startPar;
	// Use this for initialization
	void Start () {
		
		sr = this.gameObject.GetComponent<SpriteRenderer>();
		startPar = this.gameObject.transform.parent.gameObject;
		if(startPar) transform.parent = null;
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(sr && startPar){
			//if(!isGrounded){
			RaycastHit2D hit = Physics2D.Raycast(startPar.transform.position, -Vector3.up);
				if(hit.collider != null){
				float gndDist = 1.2f/Vector3.SqrMagnitude(startPar.transform.position-hit.transform.position);
				Vector3 point = new Vector3(hit.point.x,hit.point.y - .18f,0);
				transform.position = point;
					//print(gndDist);
					sr.color = new Vector4(1,1,1,gndDist);
				}
				//sr.color = new Vector4(1,1,1,0);
			//}
			//else{
			//	float ypos = walkAnim.transform.localPosition.y;
			//	transform.parent = startPar;
			//	transform.localPosition = startPos;
			//	if(ypos < 0f){
			//
			//		sr.color = new Vector4(1,1,1,1);
			//	}
			//	else{
			//		ypos = (Vector3.SqrMagnitude(Vector3.zero - walkAnim.transform.localPosition)/1.5f);
			//		//print(ypos);
			//		sr.color = new Vector4(1,1,1,ypos);
			//	}
			//}
		}
		else{
			if(!sr)print("no sprite renderer");
		}
	}
}
