using UnityEngine;
using System.Collections;

public class lightScript : MonoBehaviour {
	GameObject flashlight;
	float maxLightDist = .1f;
	Color OGCol;
	public SpriteRenderer spriteRenderer = null;
	gameManager man;
	public float dt;
	// Use this for initialization
	void Start () {
		flashlight = GameObject.FindGameObjectWithTag("LightObj");
		if(!spriteRenderer) spriteRenderer = this.GetComponent<SpriteRenderer>();
		OGCol = spriteRenderer.color;
		man = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
		float distance = (1/Vector3.SqrMagnitude(transform.position-flashlight.transform.position)) + man.getPower();
		dt = distance;
		if(distance < maxLightDist) {
			spriteRenderer.color = new Vector4(OGCol.r,OGCol.g,OGCol.b,0);
		}
		else {
			spriteRenderer.color = new Vector4(OGCol.r,OGCol.g,OGCol.b,(distance));
		}
	}
}
