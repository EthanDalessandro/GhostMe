using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSpline : MonoBehaviour
{
    public Spline _spline;
    private float distance = 0;
    [Range(0,30)] public float speed = 1;

    public Vector3 offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(distance > _spline.length())
        {
            distance = 0;
        }
        else
        {
            distance += Time.deltaTime * speed;
        }

        transform.position = _spline.transform.TransformPoint(_spline.computePointWithLength(distance));

        Orientation orientation = _spline.computeOrientationWithRMFWithLength(distance);

        transform.rotation = Quaternion.LookRotation(_spline.transform.TransformDirection(orientation.forward), _spline.transform.TransformDirection( orientation.upward));

        transform.position += transform.TransformDirection(offset);
    }
}
