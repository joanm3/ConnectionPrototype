using UnityEngine;
using System.Collections;

public class GrabbObject : MonoBehaviour
{

    [SerializeField]
    Color m_grabbColor;
    [SerializeField]
    private float m_speed;
    [SerializeField]
    private bool m_bDebug = false;

    [SerializeField]
    private bool m_isGrabbing = false;
    [HideInInspector]
    public Transform objectToGrabb;

    private Material _mat;

    [SerializeField]
    private AudioClip m_grabsound;
    [SerializeField]
    private AudioClip m_dropsound;
    [SerializeField]
    private AudioSource m_AudioSource;


    void Start()
    {
        m_isGrabbing = false;
    }

    void OnTriggerEnter(Collider other)
    {

        ObjectController oc = other.GetComponent<ObjectController>();

        if (oc == null)
            return;

        if (oc.isDraggable == false)
            return;

        if (objectToGrabb == null)
            objectToGrabb = other.gameObject.transform;


        if (objectToGrabb != null)
        {
            _mat = objectToGrabb.GetComponent<MeshRenderer>().material;
            _mat.color = m_grabbColor;
        }
        else
        {
            return;
        }

        if (m_bDebug)
            Debug.Log("Object to dragg is " + objectToGrabb);

    }

    void OnTriggerExit(Collider other)
    {
        ObjectController oc = other.GetComponent<ObjectController>();

        if (oc == null)
            return;
        if (oc.isDraggable == false)
            return;

        _mat.color = Color.white;

        Transform[] children = GetComponentsInChildren<Transform>();
        bool leaveObject = false;
        foreach (Transform child in children)
        {
            if (child.GetComponent<ObjectController>() != null)
            {
                if (child.GetComponent<ObjectController>().isDraggable)
                {
                    leaveObject = true;
                }
            }
        }

        if (!leaveObject)
        {
            objectToGrabb = null;
            leaveObject = false;
        }

        if (m_bDebug)
            Debug.Log("Object to dragg is " + objectToGrabb);

    }

    void Update()
    {
        if (m_bDebug)
            Debug.Log(name + " isGrabbing = " + m_isGrabbing);

        if (objectToGrabb == null || objectToGrabb == this.transform)
            return;

        _mat = objectToGrabb.GetComponent<MeshRenderer>().material;


        if (!m_isGrabbing && Input.GetButtonDown("Grabb"))
        {

            objectToGrabb.SetParent(this.transform);

            Rigidbody rigidBodyGrabb = objectToGrabb.GetComponent<Rigidbody>();
            rigidBodyGrabb.useGravity = false; 

            _mat.color = Color.white;
            if (objectToGrabb.GetComponent<ObjectController>().makeStep)
            {
                //PlayJumpSound(m_grabsound);
                StartCoroutine(DragObject(objectToGrabb));
            }

            m_isGrabbing = true;
        }

        else if (m_isGrabbing && Input.GetButtonDown("Grabb"))
        {
            // Debug.Log("Grabb button touched");
            objectToGrabb.parent = null;
            _mat.color = m_grabbColor;
            //  material.color = Color.white;
            StopAllCoroutines();
            //PlayJumpSound(m_dropsound);
            //  m_objectToGrabb = null;
            m_isGrabbing = false;

            Rigidbody rigidBodyGrabb = objectToGrabb.GetComponent<Rigidbody>();
            rigidBodyGrabb.useGravity = objectToGrabb.GetComponent<ObjectController>().useGravity;

        }
    }

    private IEnumerator DragObject(Transform objectGrabbed)
    {

        while (true)
        {
            float step = m_speed * Time.deltaTime;
            objectGrabbed.transform.position = Vector3.MoveTowards(objectGrabbed.transform.position, this.transform.position, step);
            yield return new WaitForEndOfFrame();
        }
    }


    private void PlayJumpSound(AudioClip _clip)
    {
        m_AudioSource.clip = _clip;
        m_AudioSource.Play();
    }
}
