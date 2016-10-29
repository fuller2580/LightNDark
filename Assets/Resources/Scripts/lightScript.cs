using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lightScript : MonoBehaviour {
	public List<GameObject> lights;
	float maxLightDist = .1f;
	Color OGCol;
	public SpriteRenderer spriteRenderer = null;
	lightPower lp;
	public float dt;
	public bool isPlayer = false;
	// Use this for initialization
	void Start () {
		lights = new List<GameObject>();
		StartCoroutine(findLights());
		if(!spriteRenderer) spriteRenderer = this.GetComponent<SpriteRenderer>();
		OGCol = spriteRenderer.color;

	}

	// Update is called once per frame
	void Update () {
		float distance = 0;
		dt = 0;
		for(int i = 0; i < lights.Count; i++){
			if(lights[i]){
				lp = lights[i].gameObject.GetComponent<lightPower>();
				distance = (1/Vector3.SqrMagnitude(transform.position-lights[i].transform.position)) + lp.getPower();
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

	public IEnumerator findLights(){
		lights.Clear();
		yield return new WaitForSeconds(1);
		GameObject[] AllLights = GameObject.FindGameObjectsWithTag("LightObj");
		print("Found "+AllLights.Length+" lights");
		for(int i = 0; i < AllLights.Length; i++){
			lights.Add(AllLights[i]);
		}
	}
}
