using UnityEngine;
using System.Collections;


public class ObjectController : MonoBehaviour
{

    public bool isDraggable;
    public bool makeStep = true;
    public bool activatesMecanisms = false;
    public bool useGravity = true;
    [HideInInspector]
    public Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        GetComponent<Rigidbody>().useGravity = useGravity;
    }



}
