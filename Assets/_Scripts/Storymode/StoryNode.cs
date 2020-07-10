using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryNode : MonoBehaviour
{
    public GameObject enemy;
    public Sprite level;
    public bool wasDefeated, isFinale;
    private void Start()
    {
        if (!isFinale)
            GetComponent<Image>().sprite = enemy.GetComponent<CharacterController>().charIcon;
        else
            GetComponent<Image>().sprite = level;
    }
    private void Update()
    {
        if (wasDefeated)
            GetComponent<Image>().color = Color.black;
        else
            GetComponent<Image>().color = Color.white;
    }
}