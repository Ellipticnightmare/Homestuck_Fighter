using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveLogic : MonoBehaviour
{
    public GameObject RoxyProj;
    [HideInInspector]
    public GameObject parent;
    public Collider2D[] atkCollider;
    [HideInInspector]
    public int i, dam;
    private int damMod;
    private bool canStrike;
    private int flail = 7;
    private int punch = 10;
    private int heavyPunch = 18;
    private int stomp = 12;
    private int minorDamage = 5;
    private int mediumDamage = 12;
    private int majorDamage = 30;
    private int extremeDamage = 35;
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            damMod = parent.GetComponent<CharacterController>().damageModifier;
            transform.position = parent.transform.position;
        }
    }
    public void InflictKnockback(CharacterController thisC, CharacterController otherC, int force)
    {
        otherC.gameObject.GetComponent<Rigidbody2D>().AddForce(otherC.transform.right * thisC.movement * force * 10, ForceMode2D.Impulse);
    }
    public void Teleport(int teleCheck, CharacterController target, int teleDiff)
    {
        if (teleCheck >= teleDiff)
            target.gameObject.transform.position = new Vector3(Random.Range(-16, 17), target.gameObject.transform.position.y, 0);
    }
    private void Start()
    {
        parent = this.gameObject.transform.parent.gameObject;
        foreach (var item in atkCollider)
        {
            item.enabled = false;
        }
    }
    public void BeginMove(CharacterController.MoveState mState)
    {
        canStrike = true;
        switch (mState)
        {
            case CharacterController.MoveState.standard:
                i = 0;
                break;
            case CharacterController.MoveState.special:
                i = 1;
                break;
            case CharacterController.MoveState.sideSpecial:
                i = 2;
                break;
            case CharacterController.MoveState.downSpecial:
                i = 3;
                break;
            case CharacterController.MoveState.super:
                i = 4;
                break;
        }
        atkCollider[i].enabled = true;
    }
    public void EndMove()
    {
        foreach (var item in atkCollider)
        {
            item.enabled = false;
        }
    }
    public void DealDamage(GameObject other)
    {
        CharacterController thisCont = parent.GetComponent<CharacterController>();
        CharacterController cont = other.GetComponent<CharacterController>();
        switch (parent.GetComponent<CharacterController>().charChoice)
        {
            case CharacterController.Character.Karkat:
                switch (i)
                {
                    case 0:
                        dam = flail;
                        break;
                    case 1:
                        dam = 0;
                        break;
                    case 2:
                        dam = minorDamage;
                        Debug.Log("WHY AREN'T I BEING CALLED YOU FUCEKN BISH");
                        InflictKnockback(thisCont, cont, 1);
                        break;
                    case 3:
                        dam = 0;
                        break;
                    case 4:
                        dam = 1;
                        break;
                }
                break;
            case CharacterController.Character.Cond:
                switch (i)
                {
                    case 0:
                        dam = punch;
                        break;
                    case 1:
                        dam = mediumDamage;
                        break;
                    case 2:
                        dam = mediumDamage;
                        break;
                    case 3:
                        dam = 0;
                        break;
                    case 4:
                        dam = 0;
                        break;
                }
                break;
            case CharacterController.Character.English:
                switch (i)
                {
                    case 0:
                        cont.Interrupted = true;
                        cont.stunTime = 2;
                        dam = 0;
                        break;
                    case 1:
                        dam = punch * 2;
                        InflictKnockback(thisCont, cont, 2);
                        break;
                    case 2:
                        cont.Interrupted = true;
                        dam = 20;
                        cont.stunTime = 1;
                        InflictKnockback(thisCont, cont, 1);
                        break;
                    case 3:
                        dam = stomp * 2;
                        cont.Interrupted = true;
                        cont.stunTime = 2;
                        break;
                    case 4:
                        dam = 0;
                        break;
                }
                break;
            case CharacterController.Character.Vriska:
                switch (i)
                {
                    case 0:
                        dam = punch;
                        break;
                    case 1:
                        dam = 0;
                        break;
                    case 2:
                        dam = heavyPunch;
                        break;
                    case 3:
                        dam = 0;
                        break;
                    case 4:
                        dam = 0;
                        break;
                }
                break;
            case CharacterController.Character.Kanaya:
                switch (i)
                {
                    case 0:
                        dam = flail;
                        break;
                    case 1:
                        dam = mediumDamage;
                        break;
                    case 2:
                        dam = punch;
                        InflictKnockback(thisCont, cont, 1);
                        InflictKnockback(cont, thisCont, 1);
                        break;
                    case 3:
                        dam = majorDamage;
                        break;
                    case 4:
                        dam = 0;
                        break;
                }
                break;
            case CharacterController.Character.Dad:
                switch (i)
                {
                    case 0:
                        dam = mediumDamage;
                        break;
                    case 1:
                        dam = stomp;
                        break;
                    case 2:
                        dam = heavyPunch;
                        break;
                    case 3:
                        dam = 0;
                        break;
                    case 4:
                        dam = extremeDamage;
                        InflictKnockback(thisCont, cont, 3);
                        break;
                }
                break;
            case CharacterController.Character.Jade:
                switch (i)
                {
                    case 0:
                        dam = 0;
                        break;
                    case 1:
                        dam = 0;
                        break;
                    case 2:
                        float check = Random.Range(1, 21);
                        if (check % 2 == 0)
                        {
                            float check2 = Random.Range(1, 21);
                            if (check2 % 2 == 0)
                                dam = Random.Range(1, 7);
                            else
                            {
                                cont.stunTime = 1.01f;
                                cont.Interrupted = true;
                            }
                        }
                        break;
                    case 3:
                        cont.stunTime = .65f;
                        cont.Interrupted = true;
                        Teleport(Random.Range(1, 101), thisCont, 65);
                        break;
                    case 4:
                        dam = 0;
                        break;
                }
                break;
            case CharacterController.Character.Roxy:
                switch (i)
                {
                    case 0:
                        dam = minorDamage;
                        if (thisCont.superMeter >= 5)
                        {
                            cont.stunTime = .5f;
                            cont.Interrupted = true;
                            thisCont.superMeter -= 5;
                        }
                        break;
                    case 1:
                        dam = minorDamage;
                        if (thisCont.superMeter >= 10)
                        {
                            InflictKnockback(thisCont, cont, 1);
                            thisCont.superMeter -= 10;
                        }
                        break;
                    case 2:
                        dam = 13;
                        break;
                    case 3:
                        if (thisCont.superMeter >= 10)
                        {
                            GameObject proj = Instantiate(RoxyProj, transform.position + transform.up * 1, transform.rotation);
                            proj.GetComponent<ProjectileLogic>().cont = cont;
                            thisCont.superMeter -= 10;
                        }
                        dam = 0;
                        break;
                    case 4:
                        dam = 0;
                        break;
                }
                break;
        }
        cont.takeDamage(dam * damMod);
        if (thisCont.charChoice != CharacterController.Character.Kanaya && thisCont.charChoice != CharacterController.Character.Jade)
            thisCont.superMeter += (dam * damMod) * .85f;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject != parent)
        {
            Debug.Log("Enter: " + other.gameObject.GetComponent<CharacterController>().charName);
            if (canStrike)
            {
                DealDamage(other.gameObject);
                canStrike = false;
            }
        }
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject != parent)
        {
            Debug.Log("Stay: " + other.gameObject.GetComponent<CharacterController>().charName);
            if (canStrike)
            {
                DealDamage(other.gameObject);
                canStrike = false;
            }
        }
    }
}