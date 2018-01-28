using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBox : MonoBehaviour
{

    private BoxFace[] faces;
    public GameObject inputLight;
    public Color curColour;

    // Use this for initialization
    void Start()
    {
        faces = new BoxFace[4];
        LoadFaces();
    }

    private void LoadFaces()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        BoxFace xPos = new BoxFace();
        xPos.rotation = 270;
        xPos.xOff = collider.size.z / 2;
        xPos.zOff = 0;
        xPos.index = 0;
        faces[0] = xPos;

        BoxFace xNeg = new BoxFace();
        xNeg.rotation = 90;
        xNeg.xOff = -xPos.xOff;
        xNeg.zOff = 0;
        xNeg.index = 1;
        faces[1] = xNeg;

        BoxFace zPos = new BoxFace();
        zPos.rotation = 180;
        zPos.xOff = 0;
        zPos.zOff = collider.size.z / 2;
        zPos.index = 2;
        faces[2] = zPos;

        BoxFace zNeg = new BoxFace();
        zNeg.rotation = 0;
        zNeg.xOff = 0;
        zNeg.zOff = -zPos.zOff;
        zNeg.index = 3;
        faces[3] = zNeg;
    }

    public BoxFace FindFace(float yRotation)
    {
        if (yRotation == 0)
        {
            return faces[3];
        }
        else if (yRotation == 90)
        {
            return faces[1];
        }
        else if (yRotation == 180)
        {
            return faces[2];
        }
        else if (yRotation == 270)
        {
            return faces[0];
        }
        else
        {
            print("Failed to find face");
            return new BoxFace();
        }
    }

    public BoxFace FindOppositeFace(float yRotation)
    {
        return FindFace(yRotation + 180 >= 360 ? yRotation - 180 : yRotation + 180);
    }

    public BoxFace FindOutFace(float inputLightYRotation)
    {
        int mirrorRotation = (int)Mathf.Round(transform.eulerAngles.y);
        if (mirrorRotation == 45)
        {
            if (inputLightYRotation == 0)
            {
                return FindFace(90);
            }
            else if (inputLightYRotation == 90)
            {
                return FindFace(0);
            }
            else if (inputLightYRotation == 180)
            {
                return FindFace(270);
            }
            else if (inputLightYRotation == 270)
            {
                return FindFace(180);
            }
            else
            {
                print("Light with incorrect rotation");
                return new BoxFace();
            }
        }
        else if (mirrorRotation == 135)
        {
            if (inputLightYRotation == 0)
            {
                return FindFace(270);
            }
            else if (inputLightYRotation == 90)
            {
                return FindFace(180);
            }
            else if (inputLightYRotation == 180)
            {
                return FindFace(90);
            }
            else if (inputLightYRotation == 270)
            {
                return FindFace(0);
            }
            else
            {
                print("Light with incorrect rotation");
                return new BoxFace();
            }
        }
        else
        {
            print("FindOutFace - Not valid rotation for mirror of 45 or 135 on Y");
            print("Actual rotation: " + mirrorRotation);
            return new BoxFace();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            // Cull non sourced lights
            BoxFace outFace = FindOutFace(faces[i].rotation);
            if (!faces[i].isInput && faces[i].light != null && outFace.light == null)
            {
                print("Delete");
                Destroy(faces[i].light.gameObject);
                faces[i].light = null;
                // Create new lights
            }
            else if (faces[i].isInput && faces[i].light != null && outFace.light == null)
            {
                print("Create");
                ParticleSystem pSys = faces[i].light.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule pMain = pSys.main;
                CreateLight(faces[i].light, pMain.startColor.color, new Vector3(0, faces[i].rotation, 0));
            }
        }
    }

    public void OnHit(PhysicalLight source, Color colour, Vector3 rotation)
    {
        BoxFace inFace = FindFace(rotation.y);

        if (!faces[inFace.index].isInput && faces[inFace.index].light != null)
        {
            Destroy(faces[inFace.index].light.gameObject);
            faces[inFace.index].light = null;
        }

        faces[inFace.index].light = source;
        faces[inFace.index].isInput = true;

        CreateLight(source, colour, rotation);
    }

    private void CreateLight(PhysicalLight source, Color colour, Vector3 rotation)
    {
        BoxFace outFace = FindOutFace(rotation.y);
        if (!faces[outFace.index].isInput || faces[outFace.index].light == null)
        {

            Vector3 lightPos;
            if (faces[outFace.index].xOff == 0)
            {
                lightPos = new Vector3(transform.position.x + (faces[outFace.index].zOff * 2), source.transform.position.y, transform.position.z + (faces[outFace.index].zOff * 2));
            }
            else
            {
                lightPos = new Vector3(transform.position.x + (faces[outFace.index].xOff * 2), source.transform.position.y, source.transform.position.z + (faces[outFace.index].xOff * 2));
            }
            GameObject rayLight = Instantiate(inputLight, lightPos, Quaternion.Euler(new Vector3(0, FindOppositeFace(outFace.rotation).rotation, 0)));
            ParticleSystem light = rayLight.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule lightMain = light.main;
            lightMain.startColor = colour;
            faces[outFace.index].light = rayLight.GetComponent<PhysicalLight>();
            faces[outFace.index].isInput = false;
        }
    }

    public void OnMiss(Vector3 rotation)
    {
        BoxFace inFace = FindFace(rotation.y);
        if (faces[inFace.index].isInput)
        {
            faces[inFace.index].isInput = false;
            faces[inFace.index].light = null;
            BoxFace outFace = FindOutFace(rotation.y);
            if (!faces[outFace.index].isInput)
            {
                Destroy(faces[outFace.index].light.gameObject);
                faces[outFace.index].light = null;
            }
        }
    }

    public void OnMissChain(Vector3 rotation)
    {
        BoxFace opFace = FindOutFace(rotation.y);
        if (opFace.light != null && opFace.light.box != null)
        {
            opFace.light.box.GetComponent<Box>().OnMissChain(rotation);
        }
        BoxFace inFace = FindFace(rotation.y);
        if (faces[inFace.index].isInput)
        {
            faces[inFace.index].isInput = false;
            faces[inFace.index].light = null;
            BoxFace outFace = FindOutFace(rotation.y);
            if (!faces[outFace.index].isInput)
            {
                Destroy(faces[outFace.index].light.gameObject);
                faces[outFace.index].light = null;
            }
        }
    }
}
