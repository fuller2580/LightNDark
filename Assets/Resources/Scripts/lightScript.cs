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
		float pow = 0;
		for(int i = 0; i < lights.Count; i++){
			if(lights[i]){
				lp = lights[i].gameObject.GetComponent<lightPower>();
				Vector2 mypos = new Vector2(transform.position.x,transform.position.y);
				Vector2 lightpos = new Vector2(lights[i].transform.position.x,lights[i].transform.position.y);
				if(lp.getRadius() != 6) distance = (1/Vector2.SqrMagnitude(mypos-lightpos)) + lp.getRadius();
				else distance = 0;
				if(distance > maxLightDist && (distance + lp.getPower()) > (dt + pow)){
					pow = lp.getPower();
					dt = distance;
				}
			}
		}
		if(dt < maxLightDist) {
			spriteRenderer.color = new Vector4(OGCol.r,OGCol.g,OGCol.b,0);
		}
		else {
			float tempdt = dt;
			if(tempdt > 1) tempdt = 1;
			spriteRenderer.color = new Vector4(OGCol.r,OGCol.g,OGCol.b,((1+pow)*(tempdt+pow)));
		}

	}

	public IEnumerator findLights(){
		lights.Clear();
		yield return new WaitForSeconds(1);
		GameObject[] AllLights = GameObject.FindGameObjectsWithTag("LightObj");
		//print("Found "+AllLights.Length+" lights");
		for(int i = 0; i < AllLights.Length; i++){
			lights.Add(AllLights[i]);
		}
	}
}
