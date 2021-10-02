using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prototype : MonoBehaviour
{
    public Rigidbody TestBall;
    private Vector3 _ballStartPosition;

    public Transform arrowSpriteTransform;
    
    public Rigidbody plateRigidbody;
    
    public Transform leftHandReferenceTransform;
    public Transform rightHandReferenceTransform;

    public float minHeight;
    public float maxHeight;

    public float moveSpeed;

    public bool useAltMove;

    private float _startingGravity;
    public float gravityMult = 1f;
    public float waveTime = 2f;

    // Start is called before the first frame update
    private void Start()
    {
        _startingGravity = Physics.gravity.magnitude;
        _ballStartPosition = TestBall.transform.position;

        var halfScale = plateRigidbody.transform.localScale.x / 2f;
        
        leftHandReferenceTransform.position = Vector3.left * halfScale;
        rightHandReferenceTransform.position = Vector3.right * halfScale;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
        
        if(useAltMove)
            AltMovement();
        else
            MainMovement();

        arrowSpriteTransform.up = Physics.gravity;
    }

    private void FixedUpdate()
    {
        UpdateGravity();
        
        UpdatePlate();
        CheckBall();
    }

    private void UpdateGravity()
    {
        var t = Mathf.PingPong(Time.time + waveTime / 2f, waveTime);

        var angle = Mathf.Lerp(-10.125f,10.125f, t / waveTime);
        
        Physics.gravity = (Quaternion.Euler(0,0,angle) * Vector3.down) * _startingGravity * gravityMult; 
    }

    //====================================================================================================================//
    

    private void MainMovement()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            MoveHandToTarget(moveSpeed, ref leftHandReferenceTransform);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveHandToTarget(-moveSpeed, ref leftHandReferenceTransform);
        }
        
        if (Input.GetKey(KeyCode.P))
        {
            MoveHandToTarget(moveSpeed, ref rightHandReferenceTransform);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            MoveHandToTarget(-moveSpeed, ref rightHandReferenceTransform);
        }
    }

    private void AltMovement()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            MoveHandToTarget(moveSpeed, ref leftHandReferenceTransform);
        }
        else
        {
            MoveHandToTarget(-moveSpeed, ref leftHandReferenceTransform);
        }
        
        if (Input.GetKey(KeyCode.P))
        {
            MoveHandToTarget(moveSpeed, ref rightHandReferenceTransform);
        }
        else
        {
            MoveHandToTarget(-moveSpeed, ref rightHandReferenceTransform);
        }
    }


    //====================================================================================================================//
    
    

    private void MoveHandToTarget(in float speed, ref Transform target)
    {
        var currentPos = target.position;
        currentPos.y = Mathf.Clamp(currentPos.y + speed * Time.deltaTime, minHeight, maxHeight);
        
        target.position = currentPos;
    }

    //TODO This might need to be done in the FixedUpdate()
    private void UpdatePlate()
    {
        var leftPosition = leftHandReferenceTransform.position;
        var rightPosition = rightHandReferenceTransform.position;

        var midPoint = (leftPosition + rightPosition) / 2f;
        var direction = (leftPosition - rightPosition).normalized;

        var angle = Vector3.Angle(Vector3.up, direction);

        plateRigidbody.MovePosition(midPoint);
        plateRigidbody.MoveRotation(Quaternion.Euler(0f, 0f, angle - 90f));

    }


    //====================================================================================================================//

    private void CheckBall()
    {
        if (TestBall.position.y > -2f)
            return;

        TestBall.position = _ballStartPosition;
        TestBall.rotation = Quaternion.identity;
        TestBall.velocity = Vector3.zero;
    }

    //====================================================================================================================//
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(leftHandReferenceTransform.position, 0.2f);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(rightHandReferenceTransform.position, 0.2f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(0f, 3f, 0f), new Vector3(0f, 3f, 0f) + Physics.gravity.normalized);
    }
}
