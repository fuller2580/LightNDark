using UnityEngine;
using System.Collections;

public class lightScript : MonoBehaviour {
	public GameObject[] flashlight;
	float maxLightDist = .1f;
	Color OGCol;
	public SpriteRenderer spriteRenderer = null;
	lightPower lp;
	public float dt;
	// Use this for initialization
	void Start () {
		flashlight = GameObject.FindGameObjectsWithTag("LightObj");
		if(!spriteRenderer) spriteRenderer = this.GetComponent<SpriteRenderer>();
		OGCol = spriteRenderer.color;

	}

	// Update is called once per frame
	void Update () {
		float distance = 0;
		dt = 0;
		for(int i = 0; i < flashlight.Length; i++){
			if(flashlight[i]){
				lp = flashlight[i].gameObject.GetComponent<lightPower>();
				distance = (1/Vector3.SqrMagnitude(transform.position-flashlight[i].transform.position)) + lp.getPower();
				if(distance > dt)dt = distance;
			}
		}
		if(dt < maxLightDist) {
			spriteRenderer.color = new Vector4(OGCol.r,OGCol.g,OGCol.b,0);
		}
		else {
			spriteRenderer.color = new Vector4(OGCol.r,OGCol.g,OGCol.b,(dt));
		}

	}
}
