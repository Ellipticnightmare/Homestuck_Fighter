using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryNodeReader : MonoBehaviour
{
    private CharacterSelector Smana;
    public StoryNode node;
    public bool isFinale;
    public Vector2 location;
    public void Start()
    {
        transform.position = node.gameObject.transform.position;
        Smana = FindObjectOfType<CharacterSelector>();
        StartCoroutine(CheckCompleted());
    }
    public IEnumerator CheckCompleted()
    {
        yield return new WaitForSeconds(.01f);
        if (node.wasDefeated)
            Smana.defeatedCount++;
    }
    public void EnterLevel()
    {
        if (isFinale)
        {
            SceneManager.LoadScene("Finale");
            PlayerPrefsX.SetBool("DefeatedEnglish", Smana.manage.defeatedEnglish);
            PlayerPrefsX.SetBool("DefeatedCon", Smana.manage.defeatedCon);
        }
        else
            Smana.LoadLevel(node);
    }
}