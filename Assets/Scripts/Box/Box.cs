using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BoxFace {
    public BoxCollider collider;
    public PhysicalLight light;
    public bool isInput;
    public float xOff;
    public float zOff;
    public float rotation;
    public int index;
}

public class Box : MonoBehaviour {

    private BoxFace[] faces;
    public GameObject inputLight;
    public Color curColour;

    //private PhysicalLight outputLight;

	// Use this for initialization
	void Start () {
        faces = new BoxFace[4];
        LoadFaces();
    }

    private void LoadFaces() {
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        BoxFace xPos = new BoxFace();
        xPos.collider = colliders[0];
        xPos.rotation = 270;
        xPos.xOff = transform.localScale.x / 2;
        xPos.zOff = 0;
        xPos.index = 0;
        faces[0] = xPos;

        BoxFace xNeg = new BoxFace();
        xNeg.collider = colliders[1];
        xNeg.rotation = 90;
        xNeg.xOff = -xPos.xOff;
        xNeg.zOff = 0;
        xNeg.index = 1;
        faces[1] = xNeg;

        BoxFace zPos = new BoxFace();
        zPos.collider = colliders[2];
        zPos.rotation = 180;
        zPos.xOff = 0;
        zPos.zOff = transform.localScale.z / 2;
        zPos.index = 2;
        faces[2] = zPos;

        BoxFace zNeg = new BoxFace();
        zNeg.collider = colliders[3];
        zNeg.rotation = 0;
        zNeg.xOff = 0;
        zNeg.zOff = -zPos.zOff;
        zNeg.index = 3;
        faces[3] = zNeg;
    }

    public BoxFace FindFace(float yRotation) {
        if (yRotation == 0) {
            return faces[3];
        } else if (yRotation == 90) {
            return faces[1];
        } else if (yRotation == 180) {
            return faces[2];
        } else if (yRotation == 270) {
            return faces[0];
        } else {
            print("Failed to find face");
            return new BoxFace();
        }
    }

    private BoxFace FindOppositeFace(float yRotation) {
        return FindFace(yRotation + 180 >= 360 ? yRotation - 180 : yRotation + 180);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        for (int i = 0; i < 4; i++) {
            // Cull non sourced lights
            if (!faces[i].isInput && faces[i].light != null && FindOppositeFace(faces[i].rotation).light == null) {
                print("Delete");
                Destroy(faces[i].light.gameObject);
                faces[i].light = null;
            // Create new lights
            } else if (faces[i].isInput && faces[i].light != null && FindOppositeFace(faces[i].rotation).light == null) {
                print("Create");
                ParticleSystem pSys = faces[i].light.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule pMain = pSys.main;
                CreateLight(faces[i].light, pMain.startColor.color, new Vector3(0, faces[i].rotation, 0));
            }
        }
    }

    public void OnHit(PhysicalLight source, Color colour, Vector3 rotation) {
        BoxFace inFace = FindFace(rotation.y);

        if (!faces[inFace.index].isInput && faces[inFace.index].light != null) {
            Destroy(faces[inFace.index].light.gameObject);
            faces[inFace.index].light = null;
        }

        faces[inFace.index].light = source;
        faces[inFace.index].isInput = true;

        CreateLight(source, colour, rotation);
    }

    private void CreateLight(PhysicalLight source, Color colour, Vector3 rotation) {
        BoxFace outFace = FindOppositeFace(rotation.y);
        if (!faces[outFace.index].isInput || faces[outFace.index].light == null) {
            Color newColour = new Color();
            newColour.r = ((colour.r + curColour.r) / 2);
            newColour.g = ((colour.g + curColour.g) / 2);
            newColour.b = ((colour.b + curColour.b) / 2);
            newColour.a = 1;
            Vector3 lightPos;
            if (faces[outFace.index].xOff == 0) {
                lightPos = new Vector3(source.transform.position.x, source.transform.position.y, transform.position.z + faces[outFace.index].zOff);
            } else {
                lightPos = new Vector3(transform.position.x + faces[outFace.index].xOff, source.transform.position.y, source.transform.position.z);
            }
            GameObject rayLight = Instantiate(inputLight, lightPos, Quaternion.Euler(rotation));
            ParticleSystem light = rayLight.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule lightMain = light.main;
            lightMain.startColor = newColour;
            faces[outFace.index].light = rayLight.GetComponent<PhysicalLight>();
            faces[outFace.index].isInput = false;
        }
    }

    public void OnMiss(Vector3 rotation) {
        BoxFace inFace = FindFace(rotation.y);
        if (faces[inFace.index].isInput) {
            faces[inFace.index].isInput = false;
            faces[inFace.index].light = null;
            BoxFace outFace = FindOppositeFace(rotation.y);
            if (!faces[outFace.index].isInput) {
                Destroy(faces[outFace.index].light.gameObject);
                faces[outFace.index].light = null;
            }
        }
    }
}
