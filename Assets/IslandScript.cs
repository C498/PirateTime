using UnityEngine;
using System.Collections;

public class IslandScript : MonoBehaviour {

	public Vector3 islandPos;


	public float xCoord, zCoord; // island coordinates

	public string hello = "hello";



	// Use this for initialization
	void Start () {

		islandPos = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

		xCoord = transform.position.x;
		zCoord = transform.position.z;

	
	}
}
