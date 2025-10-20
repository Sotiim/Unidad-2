using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb; 
    void Start()
    {
        // Get the Rigidbody component from the player object
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get horizontal and vertical input (A, D, Left Arrow, Right Arrow)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

        // Apply movement using the Transform component
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Check for jump input (Spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Jump()
    {
        // Use AddForce to make the player jump
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}