using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialObj : MonoBehaviour
{
    public Image[] Esprites;
    public Image S, W, N;
    public Sprite eXb, ePs, sXb, sPs, wXb, wPs, nXb, nPs;
    private bool ps4;
    private Text[] texts;
    public Color textColor;
    // Update is called once per frame
    void Update()
    {
        if (texts == null)
            texts = GetComponentsInChildren<Text>();
        foreach (var item in texts)
        {
            item.color = textColor;
            item.resizeTextForBestFit = true;
        }
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
            foreach (var item in Esprites)
            {
                item.sprite = ePs;
            }
            S.sprite = sPs;
            W.sprite = wPs;
            N.sprite = nPs;
        }
        else
        {
            foreach (var item in Esprites)
            {
                item.sprite = eXb;
            }
            S.sprite = sXb;
            W.sprite = wXb;
            N.sprite = nXb;
        }
    }
}
