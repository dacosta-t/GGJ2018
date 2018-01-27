using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    Renderer rend;
    public GameObject light;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnCollisionEnter(Collision collision) {
        OnHit(new Color(0, 0, 255), 0f);
    }

    public void OnHit(Color colour, float rotation) {
        Color curColour = new Color(255, 0, 0);
        Color newColour = new Color();
        newColour.r = ((colour.r + curColour.r) / 2) / 255;
        newColour.g = ((colour.g + curColour.g) / 2) / 255;
        newColour.b = ((colour.b + curColour.b) / 2) / 255;
        Instantiate(light);
        // GetComponent<ParticleSystem>().main.startColor = newColour;
        // Set light Y rotaion

        rend.material.color = newColour;
    }
}
