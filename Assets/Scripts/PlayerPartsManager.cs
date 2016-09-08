using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerPartsManager : MonoBehaviour
{

    public static PlayerPartsManager Ins;

    public Shoal PlayerStyle = Shoal.isolated;

    public List<PlayerPart> Parts = new List<PlayerPart>();

    public List<PlayerPart> SelectedParts = new List<PlayerPart>();

    private PlayerPart m_selectedPart;

    public PlayerPart SelectedPart
    {
        get
        {
            return m_selectedPart;
        }
        private set
        {
            m_selectedPart = value;
        }
    }

    private int m_selectedPartIndex;

    public int SelectedPartIndex
    {
        get
        {
            return m_selectedPartIndex;
        }
        private set
        {
            m_selectedPartIndex = value;
        }
    }

    public Canvas UICanvas;

    public List<Text> UIElements = new List<Text>();

    public float TimeToEquip = 2f;

    [HideInInspector]
    public bool BlockMovement = false;

    public bool BodyEquipped = false;

    public bool UseSelectedPartsList = false;


    public enum Shoal
    {
        schooling, isolated
    }

    void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
        }
        else if (Ins != this)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        if (SelectedPart == null)
        {
            SelectedPart = SelectPart(0);
            SelectedParts.Add(SelectedPart);
        }

        PlayerStyle = Shoal.isolated; 

        UpdateUI();

    }

    void FixedUpdate()
    {

        if (BlockMovement)
            return;

        //if (Input.GetAxis("Shoal") > 0.1f)
        //{
        //    switch (PlayerStyle)
        //    {
        //        case Shoal.isolated:
        //            PlayerStyle = Shoal.schooling;
        //            break;
        //        case Shoal.schooling:
        //            PlayerStyle = Shoal.isolated;
        //            break;
        //        default:
        //            PlayerStyle = Shoal.isolated;
        //            break;
        //    }
        //}


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedPart = SelectPart(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedPart = SelectPart(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectedPart = SelectPart(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectedPart = SelectPart(3);
        }

    }



    PlayerPart SelectPart(int index)
    {
        if (index < Parts.Count)
        {
            SelectedPartIndex = index;
            if (PlayerStyle == Shoal.isolated)
            {
                SelectedParts.Clear();
                SelectedParts.Add(Parts[index]);
                BodyEquipped = (Parts[index].PartType == PlayerPart.TPart.body);
            }
            else
            {
                if (!SelectedParts.Contains(Parts[index]))
                {
                    SelectedParts.Add(Parts[index]);
                    bool _bodyEquipped = false;
                    for (int i = 0; i < SelectedParts.Count; i++)
                    {
                        if (SelectedParts[i].PartType == PlayerPart.TPart.body)
                            _bodyEquipped = true;
                    }
                    BodyEquipped = _bodyEquipped;
                }

            }
            return Parts[index];
        }

        if (DebugManager.Ins.DebugPlayerPartsManager)
            Debug.Log(SelectedPart.name);

        return null;
    }

    public void UpdateUI()
    {
        Text[] uiText = UICanvas.GetComponentsInChildren<Text>(true);
        for (int i = 0; i < uiText.Length; i++)
        {
            if (uiText[i] != UICanvas && i < Parts.Count)
            {
                if (!UIElements.Contains(uiText[i]))
                {
                    UIElements.Add(uiText[i]);
                    UIElements[i].GetComponentInChildren<Image>().sprite = Parts[i].UITexture;
                    UIElements[i].gameObject.SetActive(true);
                    //SelectPart(i);
                }
                if (DebugManager.Ins.DebugPlayerPartsManager)
                {
                    Debug.Log("UIElement " + i + ": " + UIElements[i].name);
                }
            }
            else
            {
                uiText[i].gameObject.SetActive(false);
            }
        }
    }

}
