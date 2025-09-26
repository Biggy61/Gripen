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
    [SerializeField] private float normalThrustPercent = 50f;  
    [SerializeField] private float maxThrustPercent = 110f; 
    [SerializeField] private float noThrustPercent = 0f;      
    [SerializeField] private float thrustChangeSpeed = 50f;    
    public float maxThrustForce = 5000f;
    public float thrustForce;
    private float targetThrustPercent = 0f;
    private float currentThrustPercent = 0f;
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
        Debug.Log($"Thrust %: {currentThrustPercent}, Force: {thrustForce}");
    }

    void Thrust()
    {
 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            targetThrustPercent = maxThrustPercent; 
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            targetThrustPercent = noThrustPercent; 
        }
        else
        {
            targetThrustPercent = normalThrustPercent;
        }


        currentThrustPercent = Mathf.MoveTowards(
            currentThrustPercent, 
            targetThrustPercent, 
            thrustChangeSpeed * Time.deltaTime
        );

 
        thrustForce = (currentThrustPercent / 100f) * maxThrustForce;

     
        rb.AddForce(transform.right * thrustForce, ForceMode.Force);

       
       
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
