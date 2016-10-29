using UnityEngine;
using System.Collections;

public class warp : MonoBehaviour {
	public GameObject warpSpot;

	public Vector3 getWarpSpot(){
		return warpSpot.transform.position;
	}
}
