using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour {
	public float power = 10f;
	public GameObject CollisionEffect;
	int inUse = 0;
	GameObject player;
	// Use this for initialization
	void Start () {
		if(this.gameObject.tag != "lavaball"){
			Vector3 camPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10));
			Vector3 dir = camPos - transform.position;
			float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
			
			this.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0,power));
		}
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
			if(player != null){
				player.transform.position = this.transform.position;
				player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			}
			Destroy(this.gameObject);
			break;
		case "lavaball":
			this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
			StartCoroutine(endFire());
			break;
		default:
			break;
		}
	}

	public void shootFireball(){
		this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		Vector3 camPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10));
		Vector3 dir = camPos - transform.position;
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

		this.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0,power));
		this.gameObject.GetComponentInChildren<lightPower>().hitOnce = false;
	}

	IEnumerator endFire(){
		inUse++;
		yield return new WaitForSeconds(10);
		inUse--;
		if(inUse <= 0){
		transform.position = new Vector3(-700,-700,0);
		this.gameObject.GetComponentInChildren<lightPower>().hitOnce = true;
		}
	}
	public void setPlayer(GameObject GO){
		player = GO;
	}
}
