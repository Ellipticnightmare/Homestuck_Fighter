using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public StoryNodeReader[] nodes;
    public Text p1Multi, p2Multi;
    #region settings
    public bool ps4;
    #endregion
    //[HideInInspector]
    public GameObject player1, player2;
    public Sprite level;
    public CharacterController[] characters;
    public int charSelectInt, defeatedCount;
    private string[] unlockedCharactersArray;
    private List<string> availChars = new List<string>();
    public List<GameObject> visChars = new List<GameObject>();
    public bool defeatedCon, defeatedEnglish;
    private GameObject[] spawners;
    public Image p1Life, p2Life, p1Super, p2Super;
    public Text timerText;
    private float timer;
    public List<CharacterController> EngKilled = new List<CharacterController>();
    private string[] EngKilledArray;
    private List<string> EngKilledNames;
    private void Awake()
    {
        EngKilledArray = (PlayerPrefsX.GetStringArray("EngKilled"));
        foreach (var item in EngKilledArray)
        {
            foreach (var item2 in characters)
            {
                if (item == item2.charName)
                    EngKilled.Add(item2);
            }
        }
        defeatedCount = -1;
        timer = (60 * 14) + 54;
        defeatedEnglish = PlayerPrefsX.GetBool("DefeatedEnglish");
        defeatedCon = PlayerPrefsX.GetBool("DefeatedCon");
        unlockedCharactersArray = PlayerPrefsX.GetStringArray("unlockedChars");
        if (unlockedCharactersArray.Length > 0)
        {
            availChars.Clear();
            foreach (var item in unlockedCharactersArray)
            {
                availChars.Add(item);
                defeatedCount++;
            }
        }
        else if (unlockedCharactersArray == null || unlockedCharactersArray.Length == 0)
        {
            defeatedCount = 0;
            availChars.Add("Karkat");
            Debug.Log("Added Karkat");
        }
        foreach (var item in characters)
        {
            foreach (var i2 in availChars)
            {
                if (item.charName == i2)
                {
                    if (!visChars.Contains(item.gameObject))
                        visChars.Add(item.gameObject);
                }
            }
        }
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void LateUpdate()
    {
        defeatedCount = visChars.Count - 1;
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
            if (Input.GetButtonDown("LButtonPS4"))
                QuitGame();
        }
        else
        {
            if (Input.GetButtonDown("LButtonXbox"))
                QuitGame();
        }
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (p1Life == null)
                p1Life = GameObject.FindGameObjectWithTag("P1Life").GetComponent<Image>();
            if (p2Life == null)
                p2Life = GameObject.FindGameObjectWithTag("P2Life").GetComponent<Image>();
            if (p1Super == null)
                p1Super = GameObject.FindGameObjectWithTag("P1Super").GetComponent<Image>();
            if (p2Super == null)
                p2Super = GameObject.FindGameObjectWithTag("P2Super").GetComponent<Image>();
            if (timerText == null)
                timerText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<Text>();
            if (p1Multi == null)
                p1Multi = GameObject.FindGameObjectWithTag("P1Multi").GetComponent<Text>();
            if (p2Multi == null)
                p2Multi = GameObject.FindGameObjectWithTag("P2Multi").GetComponent<Text>();
            GetComponent<Camera>().orthographicSize = 10;
            if (timer > 0)
            {
                int minutes = Mathf.FloorToInt(timer / 60f);
                int seconds = Mathf.FloorToInt(timer - minutes * 60);
                timerText.text = "{ " + string.Format("{0:00}:{1:00}", minutes, seconds) + " }";

                timer -= Time.deltaTime;
            }
            else
            {
                //NextRound(roundCount);
                SceneManager.LoadScene("Map");
            }
            p1Multi.text = "x" + player1.GetComponent<CharacterController>().damageModifier.ToString();
            p2Multi.text = "x" + player2.GetComponent<CharacterController>().damageModifier.ToString();
            p1Life.fillAmount = player1.GetComponent<CharacterController>().health / player1.GetComponent<CharacterController>().maxHealth;
            p1Super.fillAmount = player1.GetComponent<CharacterController>().superMeter / player1.GetComponent<CharacterController>().superMeterMax;
            p2Life.fillAmount = player2.GetComponent<CharacterController>().health / player2.GetComponent<CharacterController>().maxHealth;
            p2Super.fillAmount = player2.GetComponent<CharacterController>().superMeter / player2.GetComponent<CharacterController>().superMeterMax;
        }
        else if (SceneManager.GetActiveScene().name == "Map")
        {
            GetComponent<Camera>().orthographicSize = 5;
            CharacterSelector SMana = FindObjectOfType<CharacterSelector>();

            foreach (var item in visChars)
            {
                foreach (var item2 in SMana.nodes)
                {
                    if (item2.node.enemy.GetComponent<CharacterController>().charName == item.GetComponent<CharacterController>().charName)
                    {
                        item2.node.wasDefeated = true;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        else
        {
            GetComponent<Camera>().orthographicSize = 5;
        }
    }
    public void QuitGame()
    {
        availChars.Clear();
        PlayerPrefsX.SetBool("DefeatedEnglish", defeatedEnglish);
        PlayerPrefsX.SetBool("DefeatedCon", defeatedCon);
        foreach (var item in visChars)
        {
            availChars.Add(item.GetComponent<CharacterController>().charName);
        }
        foreach (var item in EngKilled)
        {
            EngKilledNames.Add(item.charName);
        }
        EngKilledArray = EngKilledNames.ToArray();
        PlayerPrefsX.SetStringArray("EngKilled", EngKilledArray);
        PlayerPrefsX.SetStringArray("unlockedChars", availChars.ToArray());
        Application.Quit();
    }
    void SelectChar1()
    {
        player1 = visChars[charSelectInt].gameObject;
    }
    void SelectChar2()
    {
        player2 = visChars[charSelectInt].gameObject;
    }
    public void EnterGame()
    {
        if (player1 != null && player2 != null)
            SceneManager.LoadScene("Game");
        else
            return;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        if (scene.name == "Game")
            StartCoroutine(Setup(player1, player2));
        else if (scene.name == "Map")
        {
            CharacterController[] conts = GetComponentsInChildren<CharacterController>();
            foreach (var item in conts)
            {
                Destroy(item.gameObject);
            }
        }
    }
    void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "Finale")
            Destroy(this.gameObject);
    }

    public IEnumerator Setup(GameObject P1, GameObject P2)
    {
        GetComponent<Camera>().enabled = false;
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GameObject p1 = Instantiate(P1, transform);
        GameObject p2 = Instantiate(P2, transform);
        p1.transform.position = spawners[0].transform.position;
        p1.GetComponent<CharacterController>().isAI = false;
        p2.transform.position = spawners[1].transform.position;
        p2.GetComponent<CharacterController>().isAI = true;
        yield return new WaitForSeconds(.1f);
        GetComponent<Camera>().enabled = true;
        player1 = p1;
        player2 = p2;
        player1.GetComponent<CharacterController>().Begin();
        player2.GetComponent<CharacterController>().Begin();
        timer = (60 * 14) + 54;
    }
    public void DefeatedEnemy(CharacterController enemy)
    {
        if (enemy.charChoice == CharacterController.Character.English || enemy.charChoice == CharacterController.Character.Cond)
        {
            if (enemy.charName == "English")
                defeatedEnglish = true;
            if (enemy.charName == "Cond")
                defeatedCon = true;
        }
        else
        {
            foreach (var item2 in characters)
            {
                if (item2.charName == enemy.charName)
                {
                    if (!visChars.Contains(item2.gameObject))
                        visChars.Add(item2.gameObject);
                }
            }
        }
        SceneManager.LoadScene("Map");
    }
    public void Scratch()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
