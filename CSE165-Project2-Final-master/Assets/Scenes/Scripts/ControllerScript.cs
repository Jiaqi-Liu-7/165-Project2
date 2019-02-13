using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour{

    // 2(a)
    public GameObject line;
    public LineRenderer LR;
    public Rigidbody rb;

    public OVRInput.Controller controller;

    public GameObject chairPrefab;
    public GameObject deskPrefab;

    public GameObject ground;
    public GameObject room;


    private GameObject newChair;
    private GameObject newDesk;
    private GameObject prevSelected;
    private GameObject currSelected;

    public float FloatStrength = 3.5f;

    // 2(b)
    public GameObject Player;

    private GameObject TeleportMaker;
    private GameObject WalkingMaker;

    private enum Mode
    {
        WALKING, TELEPORTING, DEFAULT
    }

    private Mode MovingMode;
    private GameObject switchMode;

    private Vector3 currPosition;
    private Quaternion currRotation;

    // Start is called before the first frame update
    void Start()
    {
        line = new GameObject();
        line.transform.position = transform.position;
        line.AddComponent<LineRenderer>();
        LR = line.GetComponent<LineRenderer>();
        LR.SetPosition(0, transform.position);
        LR.SetPosition(1, transform.position + 20 * transform.forward);
        LR.material = new Material(Shader.Find("Sprites/Default"));
        LR.SetWidth(0.003f, 0.001f);
        LR.SetColors(Color.red, Color.red);

        newChair = null;
        newDesk = null;
        prevSelected = null;
        currSelected = null;

        TeleportMaker = null;
        WalkingMaker = null;

        MovingMode = Mode.DEFAULT;

}

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;

        LR = line.GetComponent<LineRenderer>();
        LR.SetPosition(0,transform.position);
        LR.SetPosition(1, transform.position + 15 * transform.forward);


        // Rendering Chair
        /*if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
          newChair = (GameObject)Instantiate(chairPrefab, (transform.position + 3 * transform.forward), Quaternion.identity);
          rb = newChair.GetComponent<Rigidbody>();
          rb.isKinematic = false;
          highlight(newChair, Color.white);
          newChair.transform.SetParent(transform);
        }
        else if (OVRInput.Get(OVRInput.Button.One, controller))
        {
            newChair.transform.position = (transform.position + 3 * transform.forward);
            rb = newChair.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            highlight(newChair, Color.white);
            newChair.transform.SetParent(transform);
        }
        else if (OVRInput.GetUp(OVRInput.Button.One, controller))
        {
            newChair.transform.position = (transform.position + 3 * transform.forward);
            rb = newChair.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            highlight(newChair, Color.white);
            newChair.transform.SetParent(null);
        }
        

        // Render Desk
        if (OVRInput.GetDown(OVRInput.Button.Two, controller))
        {
            newDesk = (GameObject)Instantiate(deskPrefab, (transform.position + 3 * transform.forward), Quaternion.identity);
            rb = newDesk.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            highlight(newDesk, Color.white);
            newDesk.transform.SetParent(transform);
        }
        else if (OVRInput.Get(OVRInput.Button.Two, controller))
        {
            newDesk.transform.SetParent(transform);
            newDesk.transform.position = (transform.position + 3 * transform.forward);
            rb = newDesk.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            highlight(newDesk, Color.white);
            newDesk.transform.SetParent(transform);
        }
        else if (OVRInput.GetUp(OVRInput.Button.Two, controller))
        {
            newDesk.transform.position = (transform.position + 3 * transform.forward);
            rb = newDesk.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            highlight(newDesk, Color.white);
            newDesk.transform.SetParent(null);
        }
       


        // Selecting Mode
        if (Physics.Raycast(transform.position, transform.transform.TransformDirection(Vector3.forward), out hitInfo, Mathf.Infinity)
            && hitInfo.transform.gameObject != ground && hitInfo.transform.gameObject != room)
        {
            currSelected = hitInfo.transform.gameObject;
        }
        else
        {
            currSelected = null;
        }

        if(currSelected != prevSelected)
        {
            if(currSelected != null)
            {
                highlight(currSelected, Color.yellow);
            }
            if(prevSelected != null)
            {
                highlight(prevSelected, Color.white);
            }
        }

        prevSelected = currSelected;

        if(currSelected != null && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > 0)
        {
            currSelected.transform.SetParent(transform);
            currSelected.transform.position = (transform.position + 3 * transform.forward);
            rb = currSelected.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
        //currSelected.transform.SetParent(null);
        rb = currSelected.GetComponent<Rigidbody>();
        rb.isKinematic = false;*/


        // Switch Mode

        if (Switch.isTeleporting)
        {
            MovingMode = Mode.TELEPORTING;
        }
        else if (Switch.isWalking)
        {
            MovingMode = Mode.WALKING;
        }
       

        switch (MovingMode)
        {
            case Mode.DEFAULT:
                {
                    break;
                }
            // Teleporting
            case Mode.TELEPORTING:
                {
                    Vector3 TeleportPosition;
                    if (Physics.Raycast(transform.position, transform.transform.TransformDirection(Vector3.forward), out hitInfo, Mathf.Infinity))
                    {
                        if (hitInfo.collider.gameObject == ground && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller) > 0)
                        {
                            TeleportPosition = hitInfo.point;
                            Player.transform.position = new Vector3(TeleportPosition.x, Player.transform.position.y + 0.1f, TeleportPosition.z);
                        }
                    }
                    MovingMode = Mode.DEFAULT;
                    break;
                }
            // Continuous Walking
            case Mode.WALKING:
                {
                    
                    Vector3 DestinationPosition;
                    Vector3 currPosition;
                    Vector3 DeltaPos;
                    if (Physics.Raycast(transform.position, transform.transform.TransformDirection(Vector3.forward), out hitInfo, Mathf.Infinity))
                    {
                        if (hitInfo.collider.gameObject == ground && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller) > 0)
                        {
                            DestinationPosition = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
                            currPosition = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
                            DeltaPos = (DestinationPosition - currPosition) / 50;

                            for(int i = 0; i < 50; i++)
                            {
                                Player.transform.position += DeltaPos;                                                                  
                            }

                            Player.transform.rotation = Quaternion.Euler(hitInfo.transform.rotation.x, Player.transform.rotation.y, hitInfo.transform.rotation.z);
                        }
                    }
                    MovingMode = Mode.DEFAULT;
                    break;
                }
        }
    }

    private void highlight(GameObject selected, Color color)
    {
        if (selected == null)
        {
            return;
        }
        foreach (Transform child in selected.GetComponentsInChildren<Transform>())
        {
            foreach (Renderer renderer in child.gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = color;
            }
        }
    }


}
