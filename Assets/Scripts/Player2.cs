using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2 : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask rayLayerMask;

    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private Rigidbody rigidbodyComponent;
    private bool lastDirection = true;

    [SerializeField] private Animator anim;

    private float groundCheckSphereSize = 0.1f;
    private bool hasJumped;
    private bool isRunning;
    public int level;

    public Vector3 checkpoint;

    static public bool isLoading;

    static public bool isRestarting;

    [SerializeField] Transform playerChainBallTransform;

    [SerializeField] public List<Transform> saveObjList;


    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            jumpKeyWasPressed = true;
        }
        anim.SetFloat("horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("vertical", rigidbodyComponent.velocity.y);
        anim.SetBool("jump", jumpKeyWasPressed);
        anim.SetBool("isGrounded", Physics.OverlapSphere(groundCheckTransform.position, groundCheckSphereSize, playerMask).Length != 0);
        horizontalInput = Input.GetAxis("Horizontal") * 10;

        if (horizontalInput > 0.1)
        {
            lastDirection = true;
        }
        else
        {
            if (horizontalInput < -0.1)
            {
                lastDirection = false;
            }
        }

        anim.SetBool("lastDirection", lastDirection);
    }

    private void FixedUpdate()
    {
        //Slopes
        float slopeCorrection = 0;

        RaycastHit hitRight;
        if (Physics.Raycast(transform.position + (Vector3.up / 10), Vector3.right, out hitRight, 0.7f, rayLayerMask))
        {
            if (horizontalInput > 0 && Physics.OverlapSphere(groundCheckTransform.position, groundCheckSphereSize, playerMask).Length == 0)
            {
                horizontalInput = 0;
            }

            if (hitRight.normal.y * 10 == 0)
            {
                rigidbodyComponent.AddForce(Vector3.left * 2, ForceMode.Impulse);
            }

            if (hitRight.normal.y * 10 > 0 && hitRight.normal.y * 10 <= 7.1)
            {
                slopeCorrection += -hitRight.normal.y * 10;
            }
        }


        RaycastHit hitLeft;
        if (Physics.Raycast(transform.position + (Vector3.up / 10), Vector3.left, out hitLeft, 0.7f, rayLayerMask))
        {
            if (horizontalInput < 0 && Physics.OverlapSphere(groundCheckTransform.position, groundCheckSphereSize, playerMask).Length == 0)
            {
                horizontalInput = 0;
            }

            if (hitLeft.normal.y * 10 == 0)
            {
                rigidbodyComponent.AddForce(Vector3.right * 2, ForceMode.Impulse);
            }

            if (hitLeft.normal.y * 10 > 0 && hitLeft.normal.y * 10 <= 7.1)
            {
                slopeCorrection += hitLeft.normal.y * 10;
            }
        }


        rigidbodyComponent.velocity = new Vector3(horizontalInput + slopeCorrection * 4, rigidbodyComponent.velocity.y, 0);



        if (Physics.OverlapSphere(groundCheckTransform.position, groundCheckSphereSize, playerMask).Length != 0)
        {
            if (hasJumped && rigidbodyComponent.velocity.y < 0.2)
            {
                hasJumped = false;
            }

            if (rigidbodyComponent.velocity.x >= 5 || rigidbodyComponent.velocity.x <= -5)
            {
                if (isRunning == false)
                {
                    isRunning = true;
                }
            }
            else
            {
                isRunning = false;
            }
        }
        else
        {
            isRunning = false;
            return;
        }

        if (jumpKeyWasPressed == true)
        {
            hasJumped = true;
            float jumpPower = 10f;
            rigidbodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }

    }

}
