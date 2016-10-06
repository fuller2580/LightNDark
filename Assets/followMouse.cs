using UnityEngine;
using System.Collections;

public class followMouse : MonoBehaviour {
	Camera cam;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {


		Vector3 pos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10));
		this.gameObject.transform.position = pos;
	}
}
