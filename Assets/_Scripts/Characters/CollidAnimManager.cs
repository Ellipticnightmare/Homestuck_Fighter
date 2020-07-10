using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidAnimManager : MonoBehaviour
{
    public Collider hitCol;
    public void StartCol()
    {
        hitCol.enabled = true;
    }
    public void EndCol()
    {
        hitCol.enabled = false;
    }
}