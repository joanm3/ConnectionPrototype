using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour
{

    public bool DebugPlayerPartsManager;
    public bool DebugPlayerMovement;
    public bool DebugPlayerparts; 


    public static DebugManager Ins;

    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {

    }
}
