using UnityEngine;
using System.Collections;

public class PlayerPart : MonoBehaviour
{

    public TPart PartType;
    public bool StartEquiped = true;
    public bool CanFly;
    public bool CanActivate;
    public Sprite UITexture;

    [HideInInspector]
    public bool MoveToAnchor;
    [Header("Part Anchors")]
    public Transform headAnchor;
    public Transform bodyAnchor;
    public Transform leftHandAnchor;
    public Transform rightHandAnchor;

    [Header("Rotating Wheels")]
    public Transform[] Wheels;
    public Transform PivotWheels;  

    private bool m_isEquiped;
    public bool IsEquipped
    {
        get
        {
            return m_isEquiped;
        }
        internal set
        {
            m_isEquiped = value;
        }
    }



    private float m_startTime;
    private float m_journeyLength;
    private bool m_calculateJourneyLength = true;

    public enum TPart
    {
        head, body, leftHand, rightHand
    }

    void Start()
    {
        m_startTime = Time.time;
    }

    void FixedUpdate()
    {
        if (MoveToAnchor)
        {
            MoveToPosition();
        }

        //if (PartType == TPart.body && transform.parent != null)
        //{
        //    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); 
        //}

        //make the parts move to the head
        //should only be if they have a path available
        //also head should place at good height (body doesnt change height). 
        //if (PlayerPartsManager.Ins.PlayerStyle == PlayerPartsManager.Shoal.schooling)
        //{
        //    if (PartType != TPart.head)
        //    {
        //        float _speed = PlayerPartsManager.Ins.TimeToEquip;
        //        MoveToPosition(this, _speed); 
        //    }
        //}
    }

    void OnCollisionExit(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero; 
    }

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void MoveToPosition(PlayerPart partToTransform, float speed)
    {



        switch (partToTransform.PartType)
        {
            case TPart.body:
                MovePart(partToTransform.transform, bodyAnchor, speed);
                break;

            case TPart.head:
                MovePart(partToTransform.transform, headAnchor, speed);

                break;

            case TPart.leftHand:
                MovePart(partToTransform.transform, leftHandAnchor, speed);

                break;

            case TPart.rightHand:
                MovePart(partToTransform.transform, rightHandAnchor, speed);
                break;

        }
    }

    void MoveToPosition()
    {
        PlayerPart _partToTransform = PlayerPartsManager.Ins.SelectedPart;
        float _speed = PlayerPartsManager.Ins.TimeToEquip;
        if (_partToTransform.PartType == TPart.body || this.PartType == TPart.body)
        {
            PlayerPartsManager.Ins.BodyEquipped = true;
        }

        switch (_partToTransform.PartType)
        {
            case TPart.body:
                MovePart(_partToTransform.transform, bodyAnchor, _speed);
                break;

            case TPart.head:
                MovePart(_partToTransform.transform, headAnchor, _speed);

                break;

            case TPart.leftHand:
                MovePart(_partToTransform.transform, leftHandAnchor, _speed);

                break;

            case TPart.rightHand:
                MovePart(_partToTransform.transform, rightHandAnchor, _speed);
                break;

        }
    }

    void MovePart(Transform selectedPartTransform, Transform anchor, float speed)
    {
        PlayerMovement.Ins.BlockMovement = true;
        PlayerPartsManager.Ins.BlockMovement = true;
        MouseLook partMouseLook = selectedPartTransform.GetComponent<MouseLook>();

        if (partMouseLook)
            partMouseLook.blockMovement = true;

        if (anchor != null)
        {
            if (m_calculateJourneyLength)
            {
                m_journeyLength = Vector3.Distance(selectedPartTransform.position, anchor.position);
                m_calculateJourneyLength = false;
            }
            float _distCovered = (Time.time - m_startTime) * (speed / 10f);
            float _fracJourney = _distCovered / m_journeyLength;
            selectedPartTransform.position = Vector3.Lerp(selectedPartTransform.position, anchor.position, _fracJourney);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, selectedPartTransform.eulerAngles, _fracJourney);


            if (Vector3.Distance(selectedPartTransform.position, anchor.position) < 0.01f)
            {
                selectedPartTransform.position = anchor.position;
                transform.eulerAngles = selectedPartTransform.eulerAngles;
                PlayerMovement.Ins.BlockMovement = false;
                PlayerPartsManager.Ins.BlockMovement = false;
                if (partMouseLook)
                    partMouseLook.blockMovement = false;

                //change this
                if(!PlayerPartsManager.Ins.SelectedParts.Contains(this))
                {
                    PlayerPartsManager.Ins.SelectedParts.Add(this); 
                }


                MoveToAnchor = false;
                m_calculateJourneyLength = true;
            }
        }
    }


}
