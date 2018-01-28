using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowImageOutline : MonoBehaviour {
	
	Outline outline;

	void Start (){	
		outline = GetComponent<Outline> ();
	}

	void Update () {
		outline.effectColor =  Color.Lerp(Color.yellow, Color.green, Mathf.PingPong(Time.time, 1));
	}
}
