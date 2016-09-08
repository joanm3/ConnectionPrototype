using UnityEngine;
using System.Collections;

public class EquipPart : MonoBehaviour
{
    [SerializeField]
    PlayerPart playerPart;

    private bool m_isOnTrigger;

    private bool m_moveToPosition = false;

    void Start()
    {
        playerPart = GetComponentInParent<PlayerPart>();
        playerPart.IsEquipped = false;
        m_isOnTrigger = false;
    }

    void FixedUpdate()
    {
        if (m_isOnTrigger && Input.GetButtonDown("Equip"))
        {
            if (PlayerPartsManager.Ins.Parts.Contains(playerPart))
                return;
            PlayerPartsManager.Ins.Parts.Add(playerPart);
            PlayerPartsManager.Ins.UpdateUI();
            playerPart.IsEquipped = true;
            if (PlayerPartsManager.Ins.UseSelectedPartsList)
            {
                m_moveToPosition = true;
            }
            m_isOnTrigger = false;
        }

        if (m_moveToPosition)
        {
            playerPart.MoveToAnchor = true;
            m_moveToPosition = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (!playerPart)
            return;

        if (other.gameObject == PlayerPartsManager.Ins.SelectedPart.gameObject && !playerPart.IsEquipped)
            m_isOnTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!playerPart)
            return;

        if (other.gameObject == PlayerPartsManager.Ins.SelectedPart.gameObject && !playerPart.IsEquipped)
            m_isOnTrigger = false;
    }




}
