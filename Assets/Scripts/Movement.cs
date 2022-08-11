using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Vector2 movement;
    public float speed;
    public Text HpText, HdText, EnerText;
    public GameObject RectHP, RectHD, RectEner;
    const int PlayerHPMax = 6;
    const int PlayerHDMax = 5;
    const int PlayerEnergyMax = 180;
    [Header("角色状态")]
    public int PlayerHP = 6;
    public int PlayerHD = 5;
    public float PlayerEnergy = 180;
    public int PlayerPlace;
    bool HpOff =false;
    int HDAddFrame=0;

    Vector3 posadd;
    public LayerMask wall;
    public AudioSource FloorWeapon;

    public bool PlayerDie = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerDie)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (movement.x != 0)
            {
                transform.localScale = new Vector3(movement.x * 0.166953f, transform.localScale.y, 0);
            }
            HDAddSet();
        }
        HpText.text = PlayerHP.ToString() + "/" + PlayerHPMax.ToString();
        HdText.text = PlayerHD.ToString() + "/" + PlayerHDMax.ToString();
        EnerText.text = PlayerEnergy.ToString() + "/" + PlayerEnergyMax.ToString();
        RectHP.GetComponent<RectTransform>().sizeDelta = new Vector2(90.58f / PlayerHPMax * PlayerHP, 10.943f);
        RectHD.GetComponent<RectTransform>().sizeDelta = new Vector2(90.58f / PlayerHDMax * PlayerHD, 10.943f);
        RectEner.GetComponent<RectTransform>().sizeDelta = new Vector2(90.58f / PlayerEnergyMax * PlayerEnergy, 10.943f);
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        anim.SetFloat("Speed", movement.magnitude);
    }
    //被room调用的模块
    public void Bang(Vector3 posadda)
    {
        posadd = posadda;
        HPoff(4);
        StartCoroutine(TouchBullet());
    }
    public IEnumerator TouchBullet()
    {
        Vector3 GoalPos = transform.position + 3 * posadd;
        Vector3 NowPos = transform.position;
        //NowPos的设置是为了防止角色移动产生的影响
        float Direction = 1;
        Vector3 x, y;
        for (int i = 0; i < 15; i++)
        {
            x = new Vector3 ((GoalPos - NowPos).x / 4 * Direction,0,0);
            y = new Vector3(0,(GoalPos - NowPos).y / 4 * Direction,0);
            transform.position += x;
            if (Physics2D.OverlapCircle(transform.position, 0.1f, wall))
            {
                transform.position -= x;
                Direction *= 0.75f;
            }
            transform.position += y;
            if (Physics2D.OverlapCircle(transform.position, 0.1f, wall))
            {
                transform.position -= y;
                Direction *= 0.75f;
            }
            NowPos += (GoalPos - NowPos)/4 ;
            yield return new WaitForEndOfFrame();
        }
    }
    void HDAddSet()
    {
        if(HDAddFrame<120)
            HDAddFrame++;
        else
        {
            if (HDAddFrame >= 180&&PlayerHD<PlayerHDMax)
            {
                PlayerHD++;
                HDAddFrame -= 60;
            }
            else HDAddFrame++;
        }
    }
    public void HPoff(int num)
    {
        if (!HpOff)
        {
            HpOff = true;
            HDAddFrame = 0;
            if (PlayerHD >= num)
            {
                PlayerHD -= num;
            }
            else if (PlayerHP > num)
            {
                PlayerHP -= num - PlayerHD;
                PlayerHD = 0;
            }
            else
            {
                PlayerHP = 0;
                Dead();
            }
            Invoke("HPOffOn", 0.8f);
        }
    }
    void HPOffOn()
    {
        HpOff = false;
    }
    void Dead()
    {
        anim.SetTrigger("Death");
        PlayerDie = true;
    }
    public void FloorWeaponSourcePlay()
    {
        FloorWeapon.Play();
    }
}
