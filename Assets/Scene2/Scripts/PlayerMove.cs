using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CharacterController CC;
    [SerializeField] private float moveSpeed;
    [SerializeField] GameObject snowball;
    private float velocity_y;
    private float grav = -9.8f;
    // Start is called before the first frame update
    void Start()
    {
        CC = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!CC.isGrounded)
        {
            velocity_y += grav * Time.deltaTime;
        }
        else
        {
            velocity_y = 0;
        }

        Vector3 movement = Vector3.down * -velocity_y;

        // these three lines didn't fix it even when they were removed
        float ForwardMovement = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        float SideMovement = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        movement += (transform.forward * ForwardMovement) + (transform.right * SideMovement);

        CC.Move(movement);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(snowball, transform.position, transform.rotation);
        }
    }
}
