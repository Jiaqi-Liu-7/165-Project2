using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public OVRInput.Controller controller;

    public GameObject TeleportingImage;
    public GameObject WalkingImage;

    private GameObject TeleportingSign;
    private GameObject WalkingSign;

    public static bool isTeleporting;
    public static bool isWalking;

    public bool isTeleportingCreated;
    public bool isWalkingCreated;

    // Start is called before the first frame update
    void Start()
    {
        isTeleporting = false;
        isWalking = false;
        isTeleportingCreated = false;
        isWalkingCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One, controller))
        {
            isTeleporting = true;
            isWalking = false;
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller) > 0)
            {
                appearTeleport();
                disappearWalk();
            }
            else
            {
                disappearTeleport();
            }
        }
        else if (OVRInput.Get(OVRInput.Button.Two, controller))
        {
            isTeleporting = false;
            isWalking = true;
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller) > 0)
            {
                appearWalk();
                disappearTeleport();
            }
            else
            {
                disappearWalk();
            }  
        }
        else
        {
            isTeleporting = false;
            isWalking = false;
            disappearWalk();
            disappearTeleport();
        }
    }

    private void appearTeleport()
    {
        if (!isTeleportingCreated && isTeleporting)
        {
            TeleportingSign = Instantiate(TeleportingImage, (transform.position + 2 * transform.forward), Quaternion.identity);
            TeleportingSign.transform.SetParent(transform);
            TeleportingSign.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
            isTeleportingCreated = true;
        }
    }

    private void disappearTeleport()
    {
        if (isTeleportingCreated)
        {
            Destroy(TeleportingSign);
            isTeleportingCreated = false;
        }
    }

    private void appearWalk()
    {
        if (!isWalkingCreated && isWalking)
        {
            WalkingSign = Instantiate(WalkingImage, (transform.position + 2 * transform.forward), Quaternion.identity);
            WalkingSign.transform.SetParent(transform);
            WalkingSign.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
            isWalkingCreated = true;
        }
    }

    private void disappearWalk()
    {
        if (isWalkingCreated)
        {
            Destroy(WalkingSign);
            isWalkingCreated = false;
        }
    }
}


