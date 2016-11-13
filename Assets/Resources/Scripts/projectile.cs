using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour {
	public float power = 10f;
	public GameObject CollisionEffect;
	// Use this for initialization
	void Start () {
		Vector3 camPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10));
		Vector3 dir = camPos - transform.position;
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

		this.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0,power));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		if(CollisionEffect != null) Instantiate(CollisionEffect, transform.position, transform.rotation);
		switch(this.gameObject.tag){
		case "slimeball":
			Destroy(this.gameObject);
			break;
		case "waterball":
			break;
		case "lavaball":
			break;
		default:
			break;
		}
	}
}
