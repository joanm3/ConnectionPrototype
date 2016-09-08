using UnityEngine;
using System.Collections;

public class HintManagement : MonoBehaviour
{
    public string message = "";
    public bool destroyOnceRead = false;

    private GameObject player;
    private bool used = false;


    private ControlsTutorial manager;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("HintManager").GetComponent<ControlsTutorial>();
        if (manager == null)
            FindObjectOfType<ControlsTutorial>();
    }

    void Update()
    {
        //change this for performance. Only check when needed. 
        player = PlayerPartsManager.Ins.SelectedPart.gameObject;

    }


    void OnTriggerEnter(Collider other)
    {
     //   player = PlayerPartsManager.Ins.SelectedPart.gameObject;
        if ((other.gameObject == player) && !used)
        {
            manager.setShowMsg(true);
            manager.setMessage(message);
            used = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            manager.setShowMsg(false);
            if (destroyOnceRead)
                Destroy(this);
            else
                used = false;
        }
    }
}
