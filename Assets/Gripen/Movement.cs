using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    public float gripenSpeed = 200f;
    public GameObject gripen;
    public Gripen gripenScript;
    private Quaternion _startRotation;
    public float rollSpeed = 90f;
    public float pitchSpeed = 90f;
    public float thrustForce = 5000f;
    public float pitch;
    public float roll;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gripen = GameObject.Find("Gripen");
        gripenScript = gripen.GetComponent<Gripen>();
        _startRotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Thrust();
        Pitch();
        Roll();
        pitch = Input.GetAxis("Vertical");   
        roll = Input.GetAxis("Horizontal"); 

    }

    void Thrust()
    {
        rb.AddForce(transform.right * (thrustForce * Time.fixedDeltaTime), ForceMode.Force);
    }
    void Pitch()
    {
        if (!gripenScript.IsAlive()) return;
        
        transform.Rotate(Vector3.forward * (-pitch * pitchSpeed * Time.deltaTime), Space.Self);
    }
    
    void Roll()
    {
        if (!gripenScript.IsAlive()) return;

        
        transform.Rotate(Vector3.right * (-roll * rollSpeed * Time.deltaTime), Space.Self);
    }
    
}
