using UnityEngine;
using System.Collections;
using System;

public class eventManager : MonoBehaviour {
	
	string eventText, eventName;
	
	// Use this for initialization
	void Start () {

		
		TextAsset txt = (TextAsset)Resources.Load("events", typeof(TextAsset));
		string content = txt.text;
		
		
		string [] split = content.Split (new Char [] {'#'});
		
		int xcounter = 0;
		
		foreach (string s in split) {
			
			xcounter++;
			
			if (xcounter == 2) {
				eventName = s;

			}

			if (xcounter == 3) {
				eventText = s;
				
			}
			
		}
		//Debug.Log (xcounter);
		//Debug.Log (eventName);
	}
	// Update is called once per frame
	void Update () {
		
	}
}

