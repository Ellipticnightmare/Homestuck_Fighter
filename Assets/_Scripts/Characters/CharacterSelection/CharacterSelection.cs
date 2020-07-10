using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Vector2 location;
    public string charName;
    public Sprite character;
    private void Start()
    {
        GetComponent<Image>().sprite = character;
    }
}