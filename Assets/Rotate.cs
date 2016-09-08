using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{

    public Transform PivotPoint;
    public Axis RotationAxis = Axis.y;
    public float Speed = 10f;


    public enum Axis { x, y, z };

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(RotationAxis)
        {
            case Axis.x:
                transform.RotateAround(PivotPoint.position, Vector3.right, Speed * Time.fixedDeltaTime);
                break;
            case Axis.y:
                transform.RotateAround(PivotPoint.position, Vector3.left, Speed * Time.fixedDeltaTime);
                break;
            case Axis.z:
                transform.RotateAround(PivotPoint.position, Vector3.forward, Speed * Time.fixedDeltaTime);
                break;
        }
    }
}
