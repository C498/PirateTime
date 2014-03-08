using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	Vector3 pos;
	public Vector3 pos_original = Camera.main.transform.position;
	public Vector3 ship_coords = GameObject.Find("Object_1").transform.position;


	// Use this for initialization
	void Start () {
		Debug.Log("Game start!");
	
	}
	
	// Update is called once per frame
	void Update () {


		//MOUSE SCROLL BACK
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
		{

			if (Camera.main.transform.position.y <=30)
			{
		
				pos = new Vector3(0,1,-1); //Translate 1 unit on x, and 1 unit on z
				Camera.main.transform.position += pos;

			}
		}
	
		// MOUSE SCROLL FORWARD
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
			if (Camera.main.transform.position.y >=4)
			{

				pos = new Vector3(0,-1,1); //Translate 1 unit on x, and 1 unit on z
				Camera.main.transform.position += pos;

			}
		}


		//MIDDLE CLICK
		if (Input.GetMouseButtonDown(2)) {		
			Camera.main.transform.position = new Vector3(0,10,-2);
			pos = new Vector3(0,10,-2);
		}

	}
}
