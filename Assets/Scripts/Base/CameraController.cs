using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget;

    [SerializeField]
    private Transform characterTransform;

    // Update is called once per frame
    private void Update()
    {
        var pos = followTarget.position;

        pos.y = characterTransform.position.y;

        followTarget.position = pos;
    }
}
