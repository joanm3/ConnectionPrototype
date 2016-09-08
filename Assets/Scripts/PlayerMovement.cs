using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Ins;

    public KeyBoardMode keyboardMode = KeyBoardMode.QWERTY;

    public bool inverseCamera = false;
    public bool lookAtMouse = false;
    public float speed = 5f;
    public float maxSpeed = 10f;
    public float accelerationTime = 2f;

    [HideInInspector]
    public bool BlockMovement = false;

    private string m_cameraHorizontal = "CameraHorizontalQWERTY";
    private string m_cameraVertical = "CameraVerticalQWERTY";
    private string m_upDown = "UpDownQWERTY";
    private bool m_showMouse = false;
    private float m_startSpeed;





    public enum KeyBoardMode { QWERTY, AZERTY };

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
        m_startSpeed = speed;

        switch (keyboardMode)
        {
            case KeyBoardMode.QWERTY:
                m_cameraHorizontal = "CameraHorizontalQWERTY";
                m_cameraVertical = "CameraVerticalQWERTY";
                m_upDown = "UpDownQWERTY";
                break;
            case KeyBoardMode.AZERTY:
                m_cameraHorizontal = "CameraHorizontalAZERTY";
                m_cameraVertical = "CameraVerticalAZERTY";
                m_upDown = "UpDownAZERTY";
                break;
            default:
                m_cameraHorizontal = "CameraHorizontalQWERTY";
                m_cameraVertical = "CameraVerticalQWERTY";
                m_upDown = "UpDownQWERTY";
                break;
        }
    }


    void FixedUpdate()
    {

        if (BlockMovement)
            return;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_showMouse = !m_showMouse;
        }
        Cursor.visible = m_showMouse;


        if (Input.GetButton("Run"))
        {
            speed = Mathf.Lerp(m_startSpeed, maxSpeed, accelerationTime);
        }
        else if (Mathf.Abs(speed - m_startSpeed) > 0.1f)
        {
            speed = Mathf.Lerp(maxSpeed, m_startSpeed, accelerationTime);
        }
        else
        {
            speed = m_startSpeed;
        }


        if (Input.GetAxis("Shift") < 0.1)
        {
            if (PlayerPartsManager.Ins.UseSelectedPartsList)
            {
                //move selected parts
                for (int i = 0; i < PlayerPartsManager.Ins.SelectedParts.Count; i++)
                {
                    MovePlayerPart(PlayerPartsManager.Ins.SelectedParts[i],
                                   PlayerPartsManager.Ins.SelectedParts[i].PartType == PlayerPart.TPart.head,
                                   PlayerPartsManager.Ins.SelectedParts[i].CanFly);
                }
            }
            else
            {
                MovePlayerPart(PlayerPartsManager.Ins.SelectedPart,
                               PlayerPartsManager.Ins.SelectedPart.PartType == PlayerPart.TPart.head,
                               PlayerPartsManager.Ins.SelectedPart.CanFly);
            }

        }
        else
        {
            //move head
            MovePlayerPart(PlayerPartsManager.Ins.Parts[0], true, true);
        }
    }


    private void MovePlayerPart(PlayerPart playerPart, bool isCamera, bool canFly)
    {

        //Move Left Right
        if (Input.GetAxis(m_cameraHorizontal) > 0.1 || Input.GetAxis(m_cameraHorizontal) < -0.1)
        {
            if (isCamera || Input.GetAxis("Control") > 0.1f)
            {
                //temporary
                playerPart.transform.position += playerPart.transform.right * Input.GetAxis(m_cameraHorizontal) * speed * Time.fixedDeltaTime;
            }
            else
            {
                playerPart.transform.Rotate(new Vector3(0, speed * Input.GetAxis(m_cameraHorizontal), 0));
            }
        }

        //Move forward buttons
        if (Input.GetAxis(m_cameraVertical) > 0.1 || Input.GetAxis(m_cameraVertical) < -0.1)
        {
            playerPart.transform.position += playerPart.transform.forward * Input.GetAxis(m_cameraVertical) * speed * Time.fixedDeltaTime;
            if (playerPart.Wheels.Length > 0)
            {
                for (int i = 0; i < playerPart.Wheels.Length; i++)
                {

                    playerPart.Wheels[i].RotateAround(playerPart.PivotWheels.position, playerPart.PivotWheels.right, speed * 100 * Time.fixedDeltaTime);
                }
            }
        }

        //Move UpDown
        if ((Input.GetAxis(m_upDown) > 0.1 || Input.GetAxis(m_upDown) < -0.1) && canFly)
        {
            playerPart.transform.position += playerPart.transform.up * Input.GetAxis(m_upDown) * speed * Time.fixedDeltaTime;
        }
    }





}

