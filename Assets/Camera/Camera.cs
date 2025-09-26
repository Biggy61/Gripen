using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Targets")]
    public GameObject gripen;   // Plane
    public GameObject cockpit;  // Cockpit reference

    [Header("Follow Settings")]
    public Vector3 defaultOffset = new(-17f, 7f, 0f); // default camera offset
    public float followSmooth = 5f;                   // smoothing for camera movement

    [Header("Freelook Settings")]
    public float freeLookSpeed = 3f;    // mouse sensitivity
    public float snapBackSpeed = 5f;    // how fast camera recenters after releasing freelook
    public float maxPitch = 60f;        // clamp up/down rotation
    public float orbitDistance = 20f;   // distance from plane in outside view

    private bool cockpitMode = false;
    private Vector2 freelookAngles = Vector2.zero; // x = yaw, y = pitch

    void Update()
    {
        // Toggle cockpit mode
        if (Input.GetKeyDown(KeyCode.C))
            cockpitMode = !cockpitMode;
    }

    void LateUpdate()
    {
        if (cockpitMode)
        {
            if (Input.GetKey(KeyCode.V))
                CockpitFreeLook();
            else
                CockpitView();
        }
        else
        {
            if (Input.GetKey(KeyCode.V))
                OutsideFreeLook();
            else
                OutsideDefaultView();
        }
    }

    // ------------------- OUTSIDE VIEW -------------------
    void OutsideFreeLook()
    {
        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * freeLookSpeed;
        float mouseY = -Input.GetAxis("Mouse Y") * freeLookSpeed;

        freelookAngles.x += mouseX;
        freelookAngles.y = Mathf.Clamp(freelookAngles.y + mouseY, -maxPitch, maxPitch);

        // Compute orbit position in spherical coordinates
        Vector3 planePos = gripen.transform.position;
        float yawRad = Mathf.Deg2Rad * freelookAngles.x;
        float pitchRad = Mathf.Deg2Rad * freelookAngles.y;

        Vector3 offset;
        offset.x = orbitDistance * Mathf.Cos(pitchRad) * Mathf.Sin(yawRad);
        offset.y = orbitDistance * Mathf.Sin(pitchRad);
        offset.z = orbitDistance * Mathf.Cos(pitchRad) * Mathf.Cos(yawRad);

        Vector3 targetPos = planePos + offset;

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPos, followSmooth * Time.deltaTime);

        // Look at the plane
        transform.LookAt(planePos);
    }

    void OutsideDefaultView()
    {
        // Smooth snapback of angles
        freelookAngles = Vector2.Lerp(freelookAngles, Vector2.zero, snapBackSpeed * Time.deltaTime);

        Vector3 planePos = gripen.transform.position;

        // Compute default orbit position behind plane
        Vector3 offset = defaultOffset;
        Vector3 targetPos = planePos + offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, followSmooth * Time.deltaTime);
        transform.LookAt(planePos);
    }

    // ------------------- COCKPIT VIEW -------------------
    void CockpitFreeLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * freeLookSpeed;
        float mouseY = -Input.GetAxis("Mouse Y") * freeLookSpeed;

        freelookAngles.x += mouseX;
        freelookAngles.y = Mathf.Clamp(freelookAngles.y + mouseY, -maxPitch, maxPitch);

        transform.position = cockpit.transform.position;

        // Rotate relative to cockpit
        Quaternion rotation = cockpit.transform.rotation * Quaternion.Euler(freelookAngles.y, freelookAngles.x, 0);
        transform.rotation = rotation;
    }

    void CockpitView()
    {
        // Smoothly reset freelook
        freelookAngles = Vector2.Lerp(freelookAngles, Vector2.zero, snapBackSpeed * Time.deltaTime);

        transform.position = cockpit.transform.position;
        transform.rotation = cockpit.transform.rotation * Quaternion.Euler(freelookAngles.y, freelookAngles.x, 0);
    }
}
