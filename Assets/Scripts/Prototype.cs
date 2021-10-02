using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Prototype : MonoBehaviour
{
    public Rigidbody TestBall;

    public Rigidbody[] affectors;
    
    private Vector3 _ballStartPosition;

    public Transform arrowSpriteTransform;
    
    public Rigidbody plateRigidbody;
    
    public Transform leftHandReferenceTransform;
    public Transform rightHandReferenceTransform;

    public float minHeight;
    public float maxHeight;

    public float moveSpeed;

    public bool useAltMove;

    [Header("Explosion")]
    public float explosionForce;
    public float explosionRadius;
    public Vector3 explosionLocation1;
    public Vector3 explosionLocation2;


    // Start is called before the first frame update
    private void Start()
    {
        _ballStartPosition = TestBall.transform.position;

        var halfScale = plateRigidbody.transform.localScale.x / 2f;
        
        leftHandReferenceTransform.position = Vector3.left * halfScale;
        rightHandReferenceTransform.position = Vector3.right * halfScale;

        _explosionTimer = explosionTime;
        _flipExplosion = Random.value >= 0.5f;
        UpdateVisual();
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

        ExplosionUpdate();
    }

    private void FixedUpdate()
    {
        UpdatePlate();
        CheckBall();
    }

    private bool _flipExplosion;
    public float explosionTime = 4;
    private float _explosionTimer;
    private void ExplosionUpdate()
    {
        if (_explosionTimer > 0f)
        {
            _explosionTimer -= Time.deltaTime;
            return;
        }

        _explosionTimer = explosionTime;

        foreach (var affector in affectors)
        {
            affector.AddExplosionForce(explosionForce, _flipExplosion ? explosionLocation2 : explosionLocation1, 3f);
        }
        
        
        _flipExplosion = !_flipExplosion;
        
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        arrowSpriteTransform.position = _flipExplosion ? explosionLocation2 : explosionLocation1;
        arrowSpriteTransform.up = _flipExplosion ? Vector3.left : Vector3.right;
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(explosionLocation1, explosionRadius);
        Gizmos.DrawWireSphere(explosionLocation2, explosionRadius);



    }
}
