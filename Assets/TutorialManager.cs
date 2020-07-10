using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public bool ps4, didMove;
    public int i;
    public GameObject[] tutorials;
    public Image variableButton;
    public Sprite p4, xb;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in tutorials)
        {
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        tutorials[i].SetActive(true);
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
            variableButton.sprite = p4;
            if (Input.GetButtonDown("EastButton"))
                SceneManager.LoadScene("TitleScene");
        }
        else
        {
            variableButton.sprite = xb;
            if (Input.GetButtonDown("EastButtonX"))
                SceneManager.LoadScene("TitleScene");
        }
        float LStickX = Input.GetAxisRaw("LStickX");
        float LStickY = -Input.GetAxisRaw("LStickY");
        if (!didMove)
        {
            if (LStickX > .9f || LStickY > .9f)
            {
                didMove = true;
                MovePos();
            }
            else if (LStickX < -.9f || LStickY < -.9f)
            {
                didMove = true;
                MoveNeg();
            }
        }
        else
        {
            if (LStickX < .2f && LStickX > -.2f && LStickY < .2f && LStickY > -.2f)
                didMove = false;
        }
    }
    void MovePos()
    {
        tutorials[i].SetActive(false);
        if (i + 1 >= tutorials.Length)
            i = 0;
        else
            i++;
    }
    void MoveNeg()
    {
        tutorials[i].SetActive(false);
        if (i - 1 < 0)
            i = tutorials.Length - 1;
        else
            i--;
    }
}