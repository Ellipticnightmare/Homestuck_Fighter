using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public GameObject Lusu;
    public bool ps4, canTakeDamage, startHeal;
    public LayerMask ignoreCols;
    public bool DebugMode;
    [HideInInspector]
    public bool isAI, isGrounded, nBut, sBut, eBut, wBut, Interrupted, isAlive, secondForm;
    public string charName;
    public Sprite charIcon;
    private Animator anim;
    private GameManager manage;
    [HideInInspector]
    public float damageDealt, speed, stunTime;
    [HideInInspector]
    public int damageModifier;
    public JoystickState jsState;
    [HideInInspector]
    public MoveState mState;
    [HideInInspector]
    public MoveState inputState;
    public Character charChoice;
    private Vector2 i_movement;
    [HideInInspector]
    public float movement, moveCheck, defeatedCount, healRate, VriskaDicePool, VriskaCharge, dashDir;
    public float meleeRange, otrRange, health, maxHealth, superMeter, superMeterMax;
    public GameObject otrPlayer;
    public bool usingMove, canMove, isMoveSpecial;
    // Start is called before the first frame update
    public void Begin()
    {
        canTakeDamage = true;
        damageDealt = 0;
        canMove = true;
        isGrounded = true;
        speed = 1.5f;
        maxHealth = 200;
        superMeterMax = 100;
        superMeter = 0;
        health = maxHealth;
        Character parsed_enum = (Character)System.Enum.Parse(typeof(Character), charName);
        manage = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        if (isAI)
        {
            otrPlayer = manage.player1;
        }
        else
        {
            otrPlayer = manage.player2;
        }
        if (manage.player2.GetComponent<CharacterController>().charChoice == Character.English && !isAI)
            damageModifier = manage.EngKilled.Count + 1;
        else
            damageModifier = 1;
        StartCoroutine(Setup());
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (health >= maxHealth)
                health = maxHealth;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            if (Interrupted)
                canTakeDamage = true;
            else
            {
                if (isMoveSpecial)
                {
                    transform.position += new Vector3(13 * Time.deltaTime * dashDir, 0, 0);
                }
            }
            if (superMeter < superMeterMax)
            {
                if (charChoice == Character.English || charChoice == Character.Cond)
                    superMeter += 3.5f * Time.deltaTime;
                else
                    superMeter += .5f * Time.deltaTime;
            }
            if (otrPlayer.transform.position.x < this.gameObject.transform.position.x)
                movement = -1;
            else if (otrPlayer.transform.position.x > this.gameObject.transform.position.x)
            {
                movement = 1;
            }
            if (anim == null)
                anim = GetComponent<Animator>();
            if (manage == null)
                manage = transform.parent.GetComponent<GameManager>();
            if (startHeal)
            {
                if (charChoice == Character.Roxy)
                {
                    if (superMeter < superMeterMax)
                        superMeter += healRate * Time.deltaTime;
                }
                else
                {
                    if (health < maxHealth)
                        health += healRate * Time.deltaTime * 5;
                }
            }
            if (!isAI)
            {
                if (otrPlayer == null)
                    otrPlayer = manage.player2;
                string[] names = Input.GetJoystickNames();
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i].Length == 33)
                        ps4 = false;
                    else
                        ps4 = true;
                }
                if (superMeter > superMeterMax)
                {
                    superMeter = superMeterMax;
                }
                i_movement = new Vector2(Input.GetAxisRaw("LStickX"), -Input.GetAxisRaw("LStickY"));
                if (!Interrupted)
                {
                    float dist;
                    if (charChoice == Character.Dad)
                        dist = 2.9f;
                    else
                        dist = 1.5f;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, dist, 1 << ignoreCols);
                    if (hit.collider != null)
                        if (hit.collider.tag == "Ground")
                            isGrounded = true;
                        else
                            isGrounded = false;
                    else
                        isGrounded = false;

                    if (jsState == JoystickState.left || jsState == JoystickState.right && canMove)
                        anim.SetBool("Move", true);
                    else
                        anim.SetBool("Move", false);
                    #region key Inputs
                    if (i_movement.x < -.5f)
                        jsState = JoystickState.left;
                    else if (i_movement.x > .5f)
                        jsState = JoystickState.right;
                    else if (i_movement.y > .1f)
                        jsState = JoystickState.up;
                    else if (i_movement.y < -.5f)
                        jsState = JoystickState.down;
                    else if (i_movement.y < .2f && i_movement.y > -.2f && i_movement.x < .2f && i_movement.x > -.2f)
                        jsState = JoystickState.resting;
                    if (!ps4)
                    {
                        nBut = Input.GetButton("NorthButtonX");
                        sBut = Input.GetButton("SouthButtonX");
                        eBut = Input.GetButton("EastButtonX");
                        wBut = Input.GetButton("WestButtonX");
                    }
                    else if (ps4)
                    {
                        nBut = Input.GetButton("NorthButton");
                        sBut = Input.GetButton("SouthButton");
                        eBut = Input.GetButton("EastButton");
                        wBut = Input.GetButton("WestButton");
                    }
                    #endregion

                    if (canMove)
                    {
                        if (jsState == JoystickState.left)
                        {
                            transform.localScale = new Vector3(-1, 1, 1);
                            transform.Translate(-speed * 2 * Time.deltaTime, 0, 0);
                        }
                        else if (jsState == JoystickState.right)
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                            transform.Translate(speed * 2 * Time.deltaTime, 0, 0);
                        }
                        else if (jsState == JoystickState.resting)
                            transform.Translate(0, 0, 0);
                        if (isGrounded)
                        {
                            if (nBut || jsState == JoystickState.up)
                                GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 1, ForceMode2D.Impulse);
                        }
                        if (eBut && (jsState == JoystickState.right || jsState == JoystickState.left))
                        {
                            canMove = false;
                            usingMove = true;
                            StartCoroutine(UseMove(MoveState.sideSpecial));
                        }
                        if (eBut && jsState == JoystickState.down)
                        {
                            canMove = false;
                            usingMove = true;
                            StartCoroutine(UseMove(MoveState.downSpecial));
                        }
                        if (eBut && jsState == JoystickState.up)
                        {
                            canMove = false;
                            usingMove = true;
                            StartCoroutine(UseMove(MoveState.upSpecial));
                        }
                        if (eBut && jsState == JoystickState.resting)
                        {
                            canMove = false;
                            usingMove = true;
                            StartCoroutine(UseMove(MoveState.special));
                        }
                        if (wBut)
                        {
                            if (superMeter == superMeterMax && charChoice != Character.Roxy)
                            {
                                canMove = false;
                                usingMove = true;
                                superMeter -= superMeter;
                                StartCoroutine(UseMove(MoveState.super));
                            }
                            else if (charChoice == Character.Roxy)
                            {
                                canMove = false;
                                usingMove = true;
                                StartCoroutine(UseMove(MoveState.super));
                            }
                        }
                        if (sBut)
                        {
                            canMove = false;
                            usingMove = true;
                            StartCoroutine(UseMove(MoveState.standard));
                        }
                    }
                }

                else
                {
                    stunTime -= Time.deltaTime;
                    if (stunTime <= 0)
                        Interrupted = false;
                }
            }
            else
            {
                if (!Interrupted)
                {
                    otrRange = Vector3.Distance(this.transform.position, otrPlayer.transform.position);
                    if (otrRange > meleeRange && !canMove && !usingMove)
                        canMove = true;
                    defeatedCount = manage.defeatedCount + 1;
                    if (otrPlayer == null)
                        otrPlayer = manage.player1;
                    if (canMove)
                    {
                        transform.localScale = new Vector3(movement, 1, 1);
                        if (otrRange >= meleeRange)
                        {
                            anim.SetBool("Move", true);
                            transform.Translate(speed * Time.deltaTime * movement, 0, 0);
                        }
                        else if (otrRange < meleeRange)
                        {
                            if (superMeter >= superMeterMax && charChoice != Character.Roxy)
                            {
                                canMove = false;
                                usingMove = true;
                                StartCoroutine(UseMove(MoveState.super));
                                superMeter -= superMeter;
                            }
                            anim.SetBool("Move", false);
                            moveCheck = Random.Range(0, 21);
                            if (moveCheck >= 18 - defeatedCount)
                            {
                                if (superMeter <= 20 && charChoice == Character.Roxy)
                                {
                                    canMove = false;
                                    usingMove = true;
                                    StartCoroutine(UseMove(MoveState.super));
                                }
                                else
                                {
                                    int moveSelect = Random.Range(1, 6);
                                    switch (moveSelect)
                                    {
                                        case 1:
                                            canMove = false;
                                            usingMove = true;
                                            StartCoroutine(UseMove(MoveState.standard));
                                            break;
                                        case 2:
                                            canMove = false;
                                            usingMove = true;
                                            StartCoroutine(UseMove(MoveState.special));
                                            break;
                                        case 3:
                                            canMove = false;
                                            usingMove = true;
                                            StartCoroutine(UseMove(MoveState.downSpecial));
                                            break;
                                        case 4:
                                            if (charChoice != Character.Cond || charChoice != Character.English)
                                            {
                                                canMove = false;
                                                usingMove = true;
                                                StartCoroutine(UseMove(MoveState.upSpecial));
                                            }
                                            else
                                            {
                                                canMove = true;
                                                usingMove = false;
                                            }
                                            break;
                                        case 5:
                                            canMove = false;
                                            usingMove = true;
                                            StartCoroutine(UseMove(MoveState.sideSpecial));
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                canMove = true;
                            }
                        }
                    }
                }

                else
                {
                    stunTime -= Time.deltaTime;
                    if (stunTime <= 0)
                        Interrupted = false;
                }
            }
        }
    }
    #region gameplay functions
    public void HasDied(bool isAITrue)
    {
        CharacterController[] players = FindObjectsOfType<CharacterController>();
        players[0].Interrupted = true;
        players[1].Interrupted = true;
        if (isAITrue)
            manage.DefeatedEnemy(this);
        else
        {
            CharacterController otrPlayer = manage.player2.GetComponent<CharacterController>();
            if (otrPlayer.charChoice == Character.English)
            {
                PlayerPrefs.SetFloat("EnglishHealth", otrPlayer.health);
                if (!manage.EngKilled.Contains(this))
                    manage.EngKilled.Add(this);
            }
            SceneManager.LoadScene("Map");
        }
    }
    public void takeDamage(int damage)
    {
        if (canTakeDamage)
        {
            health -= damage;
            if (health <= 0)
                HasDied(isAI);
        }
    }
    public IEnumerator UseMove(MoveState input)
    {
        int dam = 0;
        inputState = input;
        CharacterController conter;
        if (isAI)
            conter = manage.player1.GetComponent<CharacterController>();
        else
            conter = manage.player2.GetComponent<CharacterController>();
        float cooldown = .001f;
        switch (charChoice)
        {
            case Character.Karkat:
                switch (input)
                {
                    case MoveState.standard:
                        cooldown = .501f;
                        anim.SetBool("StandardAtk", true);
                        break;
                    case MoveState.special:
                        cooldown = .501f;
                        damageModifier++;
                        health -= 20;
                        anim.SetBool("SpecialAtk", true);
                        break;
                    case MoveState.sideSpecial:
                        cooldown = .501f;
                        anim.SetBool("SideSpecial", true);
                        break;
                    case MoveState.downSpecial:
                        cooldown = .501f;
                        canTakeDamage = false;
                        anim.SetBool("DownSpecial", true);
                        break;
                    case MoveState.upSpecial:
                        cooldown = .517f;
                        anim.SetBool("Jump", true);
                        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 3, ForceMode2D.Impulse);
                        break;
                    case MoveState.super:
                        cooldown = .501f;
                        GameObject Lusus = Instantiate(Lusu, transform.position + new Vector3(0, 1, 0), transform.rotation);
                        Lusus.GetComponent<Lusu>().health = 20;
                        Lusus.GetComponent<Lusu>().cont = this.gameObject.GetComponent<CharacterController>();
                        superMeter -= superMeter;
                        anim.SetBool("Super", true);
                        break;
                }
                break;
            case Character.Cond:
                switch (input)
                {
                    case MoveState.standard:
                        cooldown = .501f;
                        anim.SetBool("StandardAtk", true);
                        break;
                    case MoveState.special:
                        cooldown = .501f;
                        canTakeDamage = false;
                        anim.SetBool("SpecialAtk", true);
                        break;
                    case MoveState.sideSpecial:
                        cooldown = .501f;
                        canTakeDamage = false;
                        transform.position += new Vector3(speed * Time.deltaTime * movement, 0f, 0f);
                        anim.SetBool("SideSpecial", true);
                        break;
                    case MoveState.downSpecial:
                        cooldown = .501f;
                        startHeal = true;
                        healRate = 1f;
                        anim.SetBool("DownSpecial", true);
                        break;
                    case MoveState.upSpecial:
                        cooldown = .517f;
                        anim.SetBool("Jump", true);
                        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 3, ForceMode2D.Impulse);
                        break;
                    case MoveState.super:
                        cooldown = 1.001f;
                        conter.damageModifier++;
                        health += maxHealth - health;
                        anim.SetBool("Super", true);
                        break;
                }
                break;
            case Character.English:
                switch (input)
                {
                    case MoveState.standard:
                        cooldown = .501f;
                        canTakeDamage = false;
                        anim.SetBool("StandardAtk", true);
                        break;
                    case MoveState.special:
                        cooldown = .501f;
                        anim.SetBool("SpecialAtk", true);
                        break;
                    case MoveState.sideSpecial:
                        cooldown = .501f;
                        anim.SetBool("SideSpecial", true);
                        break;
                    case MoveState.downSpecial:
                        cooldown = .501f;
                        anim.SetBool("DownSpecial", true);
                        break;
                    case MoveState.upSpecial:
                        cooldown = .517f;
                        anim.SetBool("Jump", true);
                        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 3, ForceMode2D.Impulse);
                        break;
                    case MoveState.super:
                        cooldown = 1.001f;
                        startHeal = true;
                        damageModifier += 3;
                        healRate = 5;
                        anim.SetBool("Super", true);
                        break;
                }
                break;
            case Character.Vriska:
                switch (input)
                {
                    case MoveState.standard:
                        cooldown = .501f;
                        anim.SetBool("StandardAtk", true);
                        break;
                    case MoveState.special:
                        cooldown = .501f;
                        dam = (Random.Range(1, 5) * damageModifier);
                        conter.takeDamage(dam);
                        superMeter += dam;
                        anim.SetBool("SpecialAtk", true);
                        break;
                    case MoveState.sideSpecial:
                        cooldown = .501f;
                        anim.SetBool("SideSpecial", true);
                        break;
                    case MoveState.downSpecial:
                        cooldown = .501f;
                        if (VriskaCharge < 8)
                            if (VriskaDicePool != 0)
                            {
                                VriskaDicePool = VriskaDicePool * (Random.Range(1, 3));
                                VriskaCharge++;
                            }
                            else
                                VriskaDicePool = (Random.Range(1, 3));
                        anim.SetBool("DownSpecial", true);
                        break;
                    case MoveState.upSpecial:
                        cooldown = .517f;
                        anim.SetBool("Jump", true);
                        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 3, ForceMode2D.Impulse);
                        break;
                    case MoveState.super:
                        cooldown = 1.001f;
                        conter.takeDamage(Mathf.FloorToInt(VriskaDicePool) * damageModifier);
                        VriskaDicePool = 0;
                        VriskaCharge = 0;
                        anim.SetBool("Super", true);
                        break;
                }
                break;
            case Character.Kanaya:
                switch (input)
                {
                    case MoveState.standard:
                        cooldown = .501f;
                        anim.SetBool("StandardAtk", true);
                        superMeter += 1;
                        break;
                    case MoveState.special:
                        cooldown = .501f;
                        anim.SetBool("SpecialAtk", true);
                        superMeter += 1;
                        break;
                    case MoveState.sideSpecial:
                        cooldown = .501f;
                        anim.SetBool("SideSpecial", true);
                        superMeter += 1;
                        break;
                    case MoveState.downSpecial:
                        cooldown = .501f;
                        startHeal = true;
                        healRate = .45f;
                        anim.SetBool("DownSpecial", true);
                        break;
                    case MoveState.upSpecial:
                        cooldown = .517f;
                        anim.SetBool("Jump", true);
                        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 3, ForceMode2D.Impulse);
                        break;
                    case MoveState.super:
                        cooldown = 1.001f;
                        if ((maxHealth - health) < (maxHealth / 4))
                            health += 50;
                        else
                            health = maxHealth;
                        anim.SetBool("Super", true);
                        break;
                }
                break;
            case Character.Dad:
                switch (input)
                {
                    case MoveState.standard:
                        cooldown = .501f;
                        anim.SetBool("StandardAtk", true);
                        break;
                    case MoveState.special:
                        cooldown = .501f;
                        anim.SetBool("SpecialAtk", true);
                        break;
                    case MoveState.sideSpecial:
                        cooldown = .501f;
                        anim.SetBool("SideSpecial", true);
                        break;
                    case MoveState.downSpecial:
                        cooldown = .501f;
                        startHeal = true;
                        healRate = 1.5f;
                        anim.SetBool("DownSpecial", true);
                        break;
                    case MoveState.upSpecial:
                        cooldown = .517f;
                        anim.SetBool("Jump", true);
                        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 3, ForceMode2D.Impulse);
                        break;
                    case MoveState.super:
                        cooldown = .501f;
                        anim.SetBool("Super", true);
                        break;
                }
                break;
            case Character.Jade:
                switch (input)
                {
                    case MoveState.standard:
                        cooldown = .501f;
                        anim.SetBool("StandardAtk", true);
                        dam = (Random.Range(1, 5));
                        conter.takeDamage(dam);
                        superMeter += 1;
                        break;
                    case MoveState.special:
                        cooldown = .501f;
                        float quickCheck = Random.Range(1, 21);
                        if (quickCheck % 2 == 0)
                        {
                            conter.Interrupted = true;
                            conter.stunTime = 1.25f;
                        }
                        superMeter += 1;
                        anim.SetBool("SpecialAtk", true);
                        break;
                    case MoveState.sideSpecial:
                        cooldown = .501f;
                        canTakeDamage = false;
                        isMoveSpecial = true;
                        dashDir = transform.localScale.x;
                        anim.SetBool("SideSpecial", true);
                        superMeter += 1;
                        break;
                    case MoveState.downSpecial:
                        cooldown = .501f;
                        anim.SetBool("DownSpecial", true);
                        superMeter += 1;
                        break;
                    case MoveState.upSpecial:
                        cooldown = .517f;
                        anim.SetBool("Jump", true);
                        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 3, ForceMode2D.Impulse);
                        break;
                    case MoveState.super:
                        cooldown = .501f;
                        conter.takeDamage(20 * damageModifier);
                        GetComponentInChildren<MoveLogic>().Teleport(100, conter, 10);
                        anim.SetBool("Super", true);
                        break;
                }
                break;
            case Character.Roxy:
                int cost = 0;
                switch (input)
                {
                    case MoveState.standard:
                        cooldown = .501f;
                        anim.SetBool("StandardAtk", true);
                        break;
                    case MoveState.special:
                        cooldown = .501f;
                        anim.SetBool("SpecialAtk", true);
                        break;
                    case MoveState.sideSpecial:
                        cooldown = .501f;
                        cost = 20;
                        if (cost <= superMeter)
                        {
                            superMeter -= cost;
                            anim.SetBool("SideSpecial", true);
                        }
                        else
                        {
                            anim.SetBool("FailedAtk", true);
                        }
                        break;
                    case MoveState.downSpecial:
                        cooldown = .501f;
                        anim.SetBool("DownSpecial", true);
                        break;
                    case MoveState.upSpecial:
                        cooldown = .517f;
                        anim.SetBool("Jump", true);
                        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 3, ForceMode2D.Impulse);
                        break;
                    case MoveState.super:
                        cooldown = 1.001f;
                        anim.SetBool("Super", true);
                        startHeal = true;
                        healRate = 10;
                        break;
                }
                break;
        }
        yield return new WaitForSeconds(cooldown);
        canTakeDamage = true;
        canMove = true;
        usingMove = false;
        startHeal = false;
        isMoveSpecial = false;
        anim.SetBool("StandardAtk", false);
        anim.SetBool("SpecialAtk", false);
        anim.SetBool("SideSpecial", false);
        anim.SetBool("DownSpecial", false);
        anim.SetBool("Jump", false);
        anim.SetBool("Super", false);
        inputState = MoveState.nully;
        if (charChoice == Character.Roxy)
            anim.SetBool("FailedAtk", false);
    }
    public IEnumerator Setup()
    {
        yield return new WaitForSeconds(.001f);
        if (!DebugMode)
        {
            switch (charChoice)
            {
                case Character.Karkat:
                    if (otrPlayer == null)
                        otrPlayer = manage.player2;
                    if (otrPlayer.GetComponent<CharacterController>().health == otrPlayer.GetComponent<CharacterController>().maxHealth)
                    {
                        otrPlayer.GetComponent<CharacterController>().health -= otrPlayer.GetComponent<CharacterController>().maxHealth / 10;
                        damageDealt += otrPlayer.GetComponent<CharacterController>().maxHealth / 10;
                    }
                    break;
                case Character.Cond:
                    maxHealth = 5000;
                    if (manage.player1.GetComponent<CharacterController>().charChoice == Character.Karkat)
                        health = 4500;
                    else
                        health = maxHealth;
                    break;
                case Character.English:
                    maxHealth = 100000;
                    health = PlayerPrefs.GetFloat("EnglishHealth");
                    if (health <= 0)
                    {
                        if (manage.player1.GetComponent<CharacterController>().charChoice == Character.Karkat)
                            health = 90000;
                        else
                            health = maxHealth;
                    }
                    break;
                case Character.Roxy:
                    superMeter = 100;
                    break;
            }
        }
    }
    public void StartMove()
    {
        MoveLogic mL = GetComponentInChildren<MoveLogic>();
        mL.BeginMove(inputState);
    }
    public void EndMove()
    {
        canTakeDamage = true;
        canMove = true;
        usingMove = false;
        startHeal = false;
        MoveLogic mL = GetComponentInChildren<MoveLogic>();
        mL.EndMove();
    }
    #region enums
    public enum JoystickState
    {
        down,
        left,
        right,
        up,
        resting
    };
    public enum MoveState
    {
        standard,
        special,
        sideSpecial,
        downSpecial,
        upSpecial,
        super,
        nully
    };
    public enum Character
    {
        Karkat,
        Cond,
        English,
        Vriska,
        Kanaya,
        Dad,
        Jade,
        Roxy
    };
    public enum MoveType
    {
        Ranged,
        Effect,
        Melee
    };
    public enum ImmunityType
    {
        None,
        Omni,
        Ranged,
        Effect,
        Melee
    };
    #endregion
    #endregion
}