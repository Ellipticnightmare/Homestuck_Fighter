using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelector : MonoBehaviour
{
    [HideInInspector]
    public charactersHeld Chars;
    private CharacterController[] chars;
    private void Start()
    {
        chars = GetComponentsInChildren<CharacterController>();
        if (SceneManager.GetActiveScene().name != "Game")
        {
            foreach (var item in chars)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var item in chars)
            {
                if (item.charChoice.ToString() == Chars.ToString())
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            }
        }
    }
    private void Update()
    {
        chars = GetComponentsInChildren<CharacterController>();
        if (SceneManager.GetActiveScene().name != "Game")
        {
            foreach (var item in chars)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var item in chars)
            {
                if (item.charChoice.ToString() == Chars.ToString())
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            }
        }
    }
    public enum charactersHeld
    {
        Null,
        Karkat,
        Dad,
        Roxy,
        Jade,
        Kanaya,
        Vriska
    };
}