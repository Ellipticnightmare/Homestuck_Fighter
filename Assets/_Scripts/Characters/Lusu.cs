using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lusu : MonoBehaviour
{
    public CharacterController cont;
    public float health;
    // Update is called once per frame
    void Update()
    {
        if(health > 0)
        {
            health -= Time.deltaTime;
            cont.canTakeDamage = false;
        }
        else
        {
            cont.canTakeDamage = true;
            Destroy(this.gameObject);
        }
    }
}