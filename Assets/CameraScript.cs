using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public GameObject Ship;

	public Vector3 pos_original = Camera.main.transform.position;
	public Vector3 ship_coords = GameObject.Find("Object_1").transform.position;
	float yCamera;
	float zCamera;

	// Use this for initialization
	void Start () {
		Debug.Log("Game start!");
		Ship = GameObject.FindGameObjectWithTag ("Ship");
	}
	
	// Update is called once per frame
	void Update () {
		ship_coords = Ship.transform.position;

		//MOUSE SCROLL BACK
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
		{

			if (Camera.main.transform.position.y <=30)
			{
				yCamera = ship_coords.y + 1;
				zCamera = ship_coords.z - 1;
				//pos = new Vector3(0,1,-1); //Translate 1 unit on x, and 1 unit on z
				//Camera.main.transform.position += pos;

			}
		}
	
		// MOUSE SCROLL FORWARD
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
			if (Camera.main.transform.position.y >=4)
			{
				yCamera = ship_coords.y - 1;
				zCamera = 1 + ship_coords.z;
			//	pos = new Vector3(0,-1,1); //Translate 1 unit on x, and 1 unit on z
			//	Camera.main.transform.position += pos;

			}
		}


		//MIDDLE CLICK
		if (Input.GetMouseButtonDown(2)) {	
			yCamera = ship_coords.y + 10;
			zCamera = ship_coords.z -2;
			//Camera.main.transform.position = new Vector3(0,10,-2);
			//pos = new Vector3(0,10,-2);
		}

		Vector3 pos = new Vector3 (0, yCamera, zCamera);
		Camera.main.transform.position = pos;
	}
}
