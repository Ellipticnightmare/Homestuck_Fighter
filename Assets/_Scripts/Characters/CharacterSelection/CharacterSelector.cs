using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public float charSelectX, charSelectY, levelSelectX, levelSelectY;
    private float charSelecterX, charSelecterY, levelSelecterX, levelSelecterY;
    public float defCX, defCY, defLX, defLY;
    [HideInInspector]
    public GameManager manage;
    public Vector2 charSelection, levelSelection, LStick;
    private bool showChars, hasMovedSelect, hasPressedButton;
    private GameObject Selector, UI;
    [HideInInspector]
    public Sprite xBoxE, psE, xBoxN, psN, xBoxS, psS;
    #region characterSelect
    [HideInInspector]
    public Color black, white;
    public CharacterSelection[] chars;
    private List<CharacterSelection> unlockChars = new List<CharacterSelection>();
    [HideInInspector]
    public Image displayChar, returnSprite, showSprite, selectSprite;
    private GameObject player1;
    public CharacterSelection hoverChar;
    #endregion
    #region levels
    public GameObject Condesce, English, Finale;
    public int condThreshold;
    [HideInInspector]
    public StoryNodeReader[] nodes;
    private StoryNodeReader displayLevel;
    [HideInInspector]
    public int defeatedCount;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        defeatedCount = 0;
        Selector = GameObject.FindGameObjectWithTag("Selector1");
        UI = GameObject.FindGameObjectWithTag("Player1Selector");
        nodes = FindObjectsOfType<StoryNodeReader>();
        charSelecterX = 0;
        charSelecterY = 0;
        levelSelecterX = 0;
        levelSelecterY = 0;
        defCX = charSelectX;
        defCY = charSelectY;
        defLX = levelSelectX;
        defLY = levelSelectY;
        charSelection = new Vector2(0, 0);
        levelSelection = new Vector2(0, 0);
        chars = FindObjectsOfType<CharacterSelection>();
        manage = FindObjectOfType<GameManager>();
        if (manage.player1 == null)
        {
            foreach (var item in chars)
            {
                if (item.location == charSelection)
                {
                    hoverChar = item;
                }
            }
        }
        else
        {
            foreach (var item in chars)
            {
                if (item.charName == manage.player1.GetComponent<CharacterController>().charName)
                    hoverChar = item;
            }
        }
        foreach (var item in nodes)
        {
            if (item.location == levelSelection)
                displayLevel = item;
        }
        Condesce.SetActive(false);
        English.SetActive(false);
        Finale.SetActive(false);
        UI.SetActive(false);
        showChars = true;
    }

    // Update is called once per frame
    void Update()
    {
        #region General Management
        nodes = FindObjectsOfType<StoryNodeReader>();
        defeatedCount = manage.defeatedCount;
        if (Selector == null)
            Selector = GameObject.FindGameObjectWithTag("Selector1");
        if (manage.player1 != null)
        {
            displayChar.sprite = manage.player1.GetComponent<CharacterController>().charIcon;
            Color tmp = displayChar.color;
            tmp.a = 100f;
            displayChar.color = tmp;
        }
        else
        {
            Color tmp = displayChar.color;
            tmp.a = 0f;
            displayChar.color = tmp;
        }
        foreach (var item in nodes)
        {
            if (manage.visChars.Contains(item.node.enemy))
                item.node.wasDefeated = true;
        }
        foreach (var item in manage.visChars)
        {
            foreach (var item2 in chars)
            {
                if (item.GetComponent<CharacterController>().charName == item2.charName)
                {
                    if (!unlockChars.Contains(item2))
                        unlockChars.Add(item2);
                }
            }
        }
        if (displayLevel == null)
        {
            foreach (var item in nodes)
            {
                if (item.location == levelSelection)
                    displayLevel = item;
            }
        }
        string[] names = Input.GetJoystickNames();
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].Length != 33 && names[i].Length != 0)
            {
                returnSprite.sprite = psE;
                selectSprite.sprite = psS;
                showSprite.sprite = psN;
                if (!hasPressedButton)
                {
                    if (Input.GetButtonDown("SouthButton"))
                    {
                        OnX_Button();
                        hasPressedButton = true;
                    }
                    if (Input.GetButtonDown("NorthButton"))
                    {
                        OnT_Button();
                        hasPressedButton = true;
                    }
                    if (Input.GetButtonDown("EastButton"))
                    {
                        OnS_Button();
                        hasPressedButton = true;
                    }
                }
                else if (Input.GetButtonUp("SouthButton") || Input.GetButtonUp("NorthButton") || Input.GetButtonUp("EastButton"))
                    hasPressedButton = false;
            }
            else if (names[i].Length == 33)
            {
                returnSprite.sprite = xBoxE;
                selectSprite.sprite = xBoxS;
                showSprite.sprite = xBoxN;
                if (Input.GetButtonDown("SouthButtonX"))
                    OnX_Button();
                if (Input.GetButtonDown("NorthButtonX"))
                    OnT_Button();
                if (Input.GetButtonDown("EastButtonX"))
                    OnS_Button();
            }
            else if (names[i].Length == 0)
            {
                Debug.Log("Howdy Y'all!");
            }
        }
        #region AdaptiveSelection
        if (charSelecterY > charSelectY)
            charSelecterY = charSelectY;
        if (charSelecterX > charSelectX)
            charSelecterX = charSelectX;
        if (levelSelecterY > levelSelectY)
            levelSelecterY = levelSelectY;
        if (levelSelecterX > levelSelectX)
            levelSelecterX = levelSelectX;

        if (showChars)
        {
            if (charSelection.y >= 1)
                charSelectX = 2;
            else
                charSelectX = defCX;
        }
        else if (!showChars)
        {
            if (Finale.activeInHierarchy)
                levelSelectY = 2;
            else
                levelSelectY = 1;
            if (levelSelection.y >= 1)
            {
                if (levelSelection.y == 1 && English.activeInHierarchy)
                    levelSelectX = 2;
                else if (levelSelection.y > 1)
                    levelSelectX = 0;
                else if (levelSelection.y == 1 && !English.activeInHierarchy)
                    levelSelectX = 1;
            }
            else
            {
                if (defeatedCount < 5)
                    levelSelectX = defLX;
                else
                    levelSelectX = 3;
            }
        }
        #endregion
        #endregion
        #region LStick
        LStick = new Vector2(Input.GetAxisRaw("LStickX"), -Input.GetAxisRaw("LStickY"));
        if (!hasMovedSelect)
        {
            if (LStick.x >= .9f)
            {
                MoveRight();
                hasMovedSelect = true;
                Debug.Log("Right");
            }
            else if (LStick.x <= -.9f)
            {
                MoveLeft();
                hasMovedSelect = true;
                Debug.Log("Left");
            }
            else if (LStick.y >= .9f)
            {
                MoveUp();
                hasMovedSelect = true;
                Debug.Log("Up");
            }
            else if (LStick.y <= -.9f)
            {
                MoveDown();
                hasMovedSelect = true;
                Debug.Log("Down");
            }
        }
        else
        if (LStick.x <= .25f && LStick.x >= -.25f && LStick.y <= .25f && LStick.y >= -.25f)
            hasMovedSelect = false;
        #endregion
        #region characterSelect
        if (showChars)
        {
            UI.SetActive(true);
            charSelection = new Vector2(charSelecterX, charSelecterY);
            Selector.transform.position = hoverChar.transform.position;
            foreach (var item in chars)
            {
                if (item.location == charSelection)
                    hoverChar = item;
                if (unlockChars.Contains(item))
                    item.GetComponent<Image>().color = white;
                else
                    item.GetComponent<Image>().color = black;
            }
        }
        #endregion
        #region levelSelect
        else
        {
            UI.SetActive(false);
            levelSelection = new Vector2(levelSelecterX, levelSelecterY);
            if (displayLevel != null)
                Selector.transform.position = displayLevel.transform.position;
            foreach (var item in nodes)
            {
                if (item.location == levelSelection)
                    displayLevel = item;
            }
            if (defeatedCount >= condThreshold)
            {
                Condesce.SetActive(true);
            }
            if (manage.defeatedCon)
                English.SetActive(true);
            if (manage.defeatedEnglish)
                Finale.SetActive(true);
        }
        #endregion
    }
    public void LoadLevel(StoryNode inNode)
    {
        if (manage.player1 != null)
        {
            manage.level = inNode.level;
            manage.player2 = inNode.enemy;
            manage.EnterGame();
        }
    }
    #region Inputs
    public void OnX_Button()
    {
        if (showChars)
        {
            foreach (var item in unlockChars)
            {
                if (item.charName == hoverChar.charName)
                {
                    foreach (var item2 in manage.visChars)
                    {
                        if (item.charName == item2.GetComponent<CharacterController>().charName)
                            manage.player1 = item2;
                    }
                }
            }
            displayLevel = null;
        }
        else if (!showChars)
        {
            displayLevel.EnterLevel();
            hasPressedButton = true;
        }
        showChars = false;
    }
    public void OnT_Button()
    {
        if (!showChars)
            showChars = true;
        else
            showChars = false;
    }
    public void OnS_Button()
    {
        SceneManager.LoadScene("TitleScene");
    }
    #endregion
    #region moveSelector
    public void MoveRight()
    {
        if (showChars)
        {
            if (charSelecterX + 1 <= charSelectX)
                charSelecterX++;
            else
                charSelecterX = 0;
        }
        else
        {
            if (levelSelecterX + 1 <= levelSelectX)
                levelSelecterX++;
            else
                levelSelecterX = 0;
        }
    }
    public void MoveLeft()
    {
        if (showChars)
        {
            if (charSelecterX - 1 >= 0)
                charSelecterX--;
            else
                charSelecterX = charSelectX;
        }
        else
        {
            if (levelSelecterX - 1 >= 0)
                levelSelecterX--;
            else
                levelSelecterX = levelSelectX;
        }
    }
    public void MoveUp()
    {
        if (showChars)
        {
            if (charSelecterY + 1 <= charSelectY)
                charSelecterY++;
            else
                charSelecterY = 0;
        }
        else
        {
            if (levelSelecterY + 1 <= levelSelectY)
                levelSelecterY++;
            else
                levelSelecterY = 0;
        }
    }
    public void MoveDown()
    {
        if (showChars)
        {
            if (charSelecterY - 1 >= 0)
                charSelecterY--;
            else
                charSelecterY = charSelectY;
        }
        else
        {
            if (levelSelecterY - 1 >= 0)
                levelSelecterY--;
            else
                levelSelecterY = levelSelectY;
        }
    }
    #endregion
}