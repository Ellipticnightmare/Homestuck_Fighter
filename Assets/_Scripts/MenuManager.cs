using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private bool ps4;
    private float LStick;
    public bool movedCursor;
    public Text text;
    public GameObject[] Houses, Indicators;
    public List<GameObject> players = new List<GameObject>();
    public menuSelect SelectOption;
    public int curPlayerCount;
    public GameManager mana;
    public GameObject ScratchObj;
    private void Awake()
    {
        ScratchObj.SetActive(false);
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            mana = FindObjectOfType<GameManager>();
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Finale")
        {
            string[] names = Input.GetJoystickNames();
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Length == 33)
                    ps4 = false;
                else
                    ps4 = true;
            }
            if (ps4)
            {
                if (Input.GetButtonDown("SouthButton"))
                    OnX_Button();
                if (Input.GetButtonDown("EastButton"))
                    OnO_Button();
            }
            else if (!ps4)
            {
                if (Input.GetButtonDown("SouthButtonX"))
                    OnX_Button();
                if (Input.GetButtonDown("EastButtonX"))
                    OnO_Button();
            }
            LStick = -Input.GetAxisRaw("LStickY");
            if (!movedCursor)
            {
                if (LStick > .9f)
                {
                    movedCursor = true;
                    OnMoveUp();
                }
                else if (LStick < -.9f)
                {
                    movedCursor = true;
                    OnMoveDown();
                }
            }
            else
            if (LStick <= .9f && LStick >= -.9f)
                movedCursor = false;
            switch (SelectOption)
            {
                case menuSelect.story:
                    for (int i = 0; i < Houses.Length; i++)
                    {
                        if (i != 0)
                            Houses[i].SetActive(false);
                        else
                            Houses[i].SetActive(true);
                    }
                    for (int i = 0; i < Indicators.Length; i++)
                    {
                        if (i != 0)
                            Indicators[i].SetActive(false);
                        else
                            Indicators[i].SetActive(true);
                    }
                    break;
                case menuSelect.settings:
                    for (int i = 0; i < Houses.Length; i++)
                    {
                        if (i != 1)
                            Houses[i].SetActive(false);
                        else
                            Houses[i].SetActive(true);
                    }
                    for (int i = 0; i < Indicators.Length; i++)
                    {
                        if (i != 1)
                            Indicators[i].SetActive(false);
                        else
                            Indicators[i].SetActive(true);
                    }
                    break;
                case menuSelect.tutorial:
                    for (int i = 0; i < Houses.Length; i++)
                    {
                        if (i != 2)
                            Houses[i].SetActive(false);
                        else
                            Houses[i].SetActive(true);
                    }
                    for (int i = 0; i < Indicators.Length; i++)
                    {
                        if (i != 2)
                            Indicators[i].SetActive(false);
                        else
                            Indicators[i].SetActive(true);
                    }
                    break;
                case menuSelect.quit:
                    for (int i = 0; i < Houses.Length; i++)
                    {
                        if (i != 3)
                            Houses[i].SetActive(false);
                        else
                            Houses[i].SetActive(true);
                    }
                    for (int i = 0; i < Indicators.Length; i++)
                    {
                        if (i != 3)
                            Indicators[i].SetActive(false);
                        else
                            Indicators[i].SetActive(true);
                    }
                    break;
            }
        }
    }
    public void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Finale")
        {
            var videoPlayer = FindObjectOfType<UnityEngine.Video.VideoPlayer>();
            if (!videoPlayer.isPlaying)
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }
    public void PlayStory()
    {
        SceneManager.LoadScene("Map");
    }
    public void PlaySettings()
    {
        ScratchObj.SetActive(true);
    }
    public void QuitGame()
    {
        GameManager man = FindObjectOfType<GameManager>();
        man.QuitGame();
    }
    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public enum menuSelect
    {
        story,
        settings,
        tutorial,
        quit
    };
    private void OnMoveUp()
    {
        switch (SelectOption)
        {
            case menuSelect.story:
                SelectOption = menuSelect.quit;
                break;
            case menuSelect.settings:
                SelectOption = menuSelect.story;
                break;
            case menuSelect.tutorial:
                SelectOption = menuSelect.settings;
                break;
            case menuSelect.quit:
                SelectOption = menuSelect.tutorial;
                break;
        }
    }
    private void OnMoveDown()
    {
        Debug.Log("PressedDown");
        switch (SelectOption)
        {
            case menuSelect.story:
                SelectOption = menuSelect.settings;
                break;
            case menuSelect.settings:
                SelectOption = menuSelect.tutorial;
                break;
            case menuSelect.tutorial:
                SelectOption = menuSelect.quit;
                break;
            case menuSelect.quit:
                SelectOption = menuSelect.story;
                break;
        }
    }
    private void OnX_Button()
    {
        if (ScratchObj.activeInHierarchy)
            OnScratch();
        else
        {
            switch (SelectOption)
            {
                case menuSelect.story:
                    PlayStory();
                    break;
                case menuSelect.settings:
                    PlaySettings();
                    break;
                case menuSelect.tutorial:
                    LoadTutorial();
                    break;
                case menuSelect.quit:
                    QuitGame();
                    break;
            }
        }
    }
    private void OnO_Button()
    {
        if (ScratchObj.activeInHierarchy)
            ScratchObj.SetActive(false);
    }
    private void OnScratch()
    {
        mana.Scratch();
    }
}