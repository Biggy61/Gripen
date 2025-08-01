using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject gripen;
    public GameObject cockpit;
    public Vector3 followOffset = new(-17f, 7f, 0f);
    public float freeLookSpeed = 3f;

    private bool cockpitMode = false;
    private bool freeLookMode = false;
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            freeLookMode = !freeLookMode;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            cockpitMode = !cockpitMode;
        }
        if (freeLookMode && cockpitMode)
        {
            HandleFreeLook();
        }
        else if (!freeLookMode && cockpitMode)
        {
            CockpitView();
        }
        else if (freeLookMode && !cockpitMode)
        {
            HandleFreeLook();
        }
        else if (!freeLookMode && !cockpitMode)
        {
            DefaultView();
        }
    }

    void HandleFreeLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * freeLookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * freeLookSpeed;
        if (cockpitMode)
        {
            transform.position = cockpit.transform.position;
            transform.RotateAround(cockpit.transform.position, Vector3.up, mouseX);
            transform.RotateAround(cockpit.transform.position, transform.right, -mouseY);
        }
        if (!cockpitMode)
        {
            transform.position = new Vector3(
                gripen.transform.position.x + followOffset.x,
                gripen.transform.position.y + followOffset.y,
                gripen.transform.position.z + followOffset.z
            );   
            transform.RotateAround(gripen.transform.position, Vector3.up, mouseX);
            transform.RotateAround(gripen.transform.position, transform.right, -mouseY);
        }
    }
    
    void CockpitView()
    {
        transform.position = cockpit.transform.position;
    }

    void DefaultView()
    {
        Vector3 targetPosition = new Vector3(
            gripen.transform.position.x + followOffset.x,
            gripen.transform.position.y + followOffset.y,
            gripen.transform.position.z + followOffset.z
        );

        transform.position = targetPosition;
        transform.LookAt(gripen.transform); 
    }
}
