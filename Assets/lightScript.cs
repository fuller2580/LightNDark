using UnityEngine;
using System.Collections;

public class lightScript : MonoBehaviour {
	GameObject flashlight;
	float maxLightDist = .1f;
	Color OGCol;
	public SpriteRenderer spriteRenderer = null;
	// Use this for initialization
	void Start () {
		flashlight = GameObject.FindGameObjectWithTag("LightObj");
		if(!spriteRenderer) spriteRenderer = this.GetComponent<SpriteRenderer>();
		OGCol = spriteRenderer.color;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = 1/Vector3.SqrMagnitude(transform.position-flashlight.transform.position);
		if(distance < maxLightDist) {
			spriteRenderer.color = new Vector4(OGCol.r,OGCol.g,OGCol.b,0);
		}
		else {
			spriteRenderer.color = new Vector4(OGCol.r,OGCol.g,OGCol.b,(distance));
		}
	}
}
