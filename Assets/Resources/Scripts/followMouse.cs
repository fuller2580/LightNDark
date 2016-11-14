using UnityEngine;
using System.Collections;

public class followMouse : MonoBehaviour {
	public Camera cam;
	// Use this for initialization
	void Start () {
		if(cam == null)cam = Camera.main;
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {

		if(cam != null){
			Vector3 pos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10));
			this.gameObject.transform.position = pos;
		}
	}
}
