using UnityEngine;
using System.Collections;

public class ActivationMecanism : MonoBehaviour
{
    [SerializeField]
    private bool m_activateObjects = false;

    [SerializeField]
    private Color m_activatedColor;
    [SerializeField]
    private Color m_desactivatedColor;

    [SerializeField]
    private bool m_stayActive;

    [SerializeField]
    private Transform m_activator;

    [SerializeField]
    private Transform[] m_objectsToActivate;

    private Material m_material;
    private bool m_playerTouching = false;
    private bool m_activatorTouching = false;
    //private bool m_activated; 


    void Start()
    {

        m_material = (m_activator != null) ? m_activator.GetComponent<MeshRenderer>().material :
                                            GetComponent<MeshRenderer>().material;

        m_material.color = m_desactivatedColor;

        if (m_activateObjects)
        {
            foreach (Transform objectToActivate in m_objectsToActivate)
                objectToActivate.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        PlayerPart pp = collider.GetComponent<PlayerPart>();
        if (pp)
            m_playerTouching = pp.CanActivate;

        ObjectController oc = collider.GetComponent<ObjectController>();

        if (oc != null)
            m_activatorTouching = oc.activatesMecanisms;

        if (m_playerTouching || m_activatorTouching)
        {

            AudioSource audio = this.GetComponent<AudioSource>();
            if (audio != null)
                audio.PlayOneShot(audio.clip, audio.volume);

            m_material.color = m_activatedColor;
            foreach (Transform objectToActivate in m_objectsToActivate)
            {
                //m_activated = true;
                if (m_activateObjects)
                    objectToActivate.gameObject.SetActive(true);
                else
                    objectToActivate.gameObject.SetActive(false);

            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (m_stayActive)
            return;

        PlayerPart pp = collider.GetComponent<PlayerPart>();
        if (pp != null && pp.CanActivate)
            m_playerTouching = false;

        ObjectController oc = collider.GetComponent<ObjectController>();

        if (oc != null && oc.activatesMecanisms)
            m_activatorTouching = false;

        if (!m_activatorTouching && !m_playerTouching)
        {
            m_material.color = m_desactivatedColor;
            foreach (Transform objectToActivate in m_objectsToActivate)
            {
                // m_activated = false;
                if (m_activateObjects)
                    objectToActivate.gameObject.SetActive(false);
                else
                    objectToActivate.gameObject.SetActive(true);
            }
        }
    }


}
