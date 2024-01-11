using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplitScreenSwitch : MonoBehaviour
{
    private Camera Player1Cam, Player2Cam;

    public bool isHorizontalSplit;

    [SerializeField]
    private RectTransform splitter; // change this to RectTransform
    [SerializeField]
    private TextMeshProUGUI killCounterPlayer1, killCounterPlayer2;

    [SerializeField]
    private Transform counter1position_vertical, counter2position_vertical, counter1position_horizontal, counter2position_horizontal;

    PlayerInput playerInput;

    public void Awake()
    {
        playerInput = new PlayerInput();
    }

    void Start()
    {
        playerInput.Menu.SplitScreenSwitch.performed += _ => SwitchSplitScreen();

        SetSplitterSizeAndPosition();

        Player1Cam = GameObject.Find("Player1Cam").GetComponent<Camera>();
        Player2Cam = GameObject.Find("Player2Cam").GetComponent<Camera>();
    }

    private void SwitchSplitScreen()
    {
        isHorizontalSplit = !isHorizontalSplit;
        SetSplitScreen();
    }

    public void SetSplitScreen()
    {
        if (isHorizontalSplit)
        {
            Player1Cam.rect = new Rect(0f, 0.5f, 1f, 0.5f);
            Player2Cam.rect = new Rect(0f, 0f, 1f, 0.5f);

            SetSplitterSizeAndPosition();

            Player1Cam.fieldOfView = 30f;
            Player2Cam.fieldOfView = 30f;
            killCounterPlayer1.transform.position = counter1position_horizontal.position;
            killCounterPlayer2.transform.position = counter2position_horizontal.position;
        }
        else
        {
            Player1Cam.rect = new Rect(0f, 0f, 0.5f, 1f);
            Player2Cam.rect = new Rect(0.5f, 0f, 0.5f, 1f);

            SetSplitterSizeAndPosition();

            Player1Cam.fieldOfView = 60f;
            Player2Cam.fieldOfView = 60f;
            killCounterPlayer1.transform.position = counter1position_vertical.position;
            killCounterPlayer2.transform.position = counter2position_vertical.position;
        }
    }

    private void SetSplitterSizeAndPosition()
    {
        if (isHorizontalSplit)
        {
            splitter.anchorMin = new Vector2(0, 0.5f);
            splitter.anchorMax = new Vector2(1, 0.5f);
            splitter.offsetMin = new Vector2(0, -5); // -5 to -5 gives thickness to the splitter
            splitter.offsetMax = new Vector2(0, 5);
        }
        else
        {
            splitter.anchorMin = new Vector2(0.5f, 0);
            splitter.anchorMax = new Vector2(0.5f, 1);
            splitter.offsetMin = new Vector2(-5, 0); // -5 to -5 gives thickness to the splitter
            splitter.offsetMax = new Vector2(5, 0);
        }
    }

    void OnEnable()
    {
        // enable the character controls action map
        playerInput.Enable();
    }

    void OnDisable()
    {
        // disable the character controls action map
        playerInput.Disable();
    }
}