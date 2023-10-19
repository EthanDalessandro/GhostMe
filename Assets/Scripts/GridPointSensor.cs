using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPointSensor : MonoBehaviour
{
    private List<Vector3> _points = new List<Vector3>();

    public int XnumberPoint = 5;
    public int YnumberPoint = 10;
    public int ZnumberPoint = 5;

    public BoxCollider _boxCollider = null;

    public List<Vector3> Points { get => _points; }

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void UpdatePoints()
    {
        _points.Clear();

        for (int x = 0; x < XnumberPoint; x++)
        {
            for (int y = 0; y < YnumberPoint; y++)
            {
                for (int z = 0; z < ZnumberPoint; z++)
                {
                    Vector3 pointPosition = new Vector3(x, y, z);

                    pointPosition.x *= _boxCollider.size.x / (XnumberPoint - 1);
                    pointPosition.y *= _boxCollider.size.y / (YnumberPoint - 1);
                    pointPosition.z *= _boxCollider.size.z / (ZnumberPoint - 1);

                    pointPosition -= _boxCollider.size / 2;
                    pointPosition *= 0.9f;
                    pointPosition += _boxCollider.center;

                    _points.Add(transform.TransformPoint(pointPosition));
                }
            }
        }
    }

    void FixedUpdate()
    {
        UpdatePoints();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            Gizmos.DrawSphere(_points[i], 0.02f);
        }
    }
}
