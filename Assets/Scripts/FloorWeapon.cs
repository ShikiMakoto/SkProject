using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class FloorWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    bool IsAppear = false;
    public LayerMask PlayerL;
    AudioSource audio;
    public GameObject FloorWeapons;
    GameObject Player;
    Weapon Weapon;
    GameObject PlayerTouchObj;
    SetKuangCondition KuangScript;
    public GameObject TextCanvas;
    
    Sprite PlayerTouchWeapon;  //实际上这个变量没有意义，因为不是共有的，改日修改
    bool IsTouchPlayer  = false;

    string[] StrGun;
    Sprite[] _sprite = new Sprite[24];
    int index;
    bool IndexGet = false;
    private void Awake()
    {
        KuangScript = GameObject.Find("Kuang").gameObject.GetComponent<SetKuangCondition>();
        //transform.position = new Vector3(-0.009f, -0.075f,0);
        Color color = gameObject.GetComponent<SpriteRenderer>().color;
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0);
        Player = GameObject.Find("Player");
        Weapon = GameObject.Find("Weapon").GetComponent<Weapon>();
        audio = GetComponent<AudioSource>();
        TextCanvas.SetActive(false);
        TextAsset txtGun = Resources.Load("gun") as TextAsset;
        StrGun = txtGun.text.Split('\n');
        for (int i = 0; i < StrGun.Length; i++)
        {
            _sprite[i] = Resources.Load<Sprite>("weapons_" + i.ToString());
        }
    }
    void Start()
    {
       KuangScript.BoxDisappear();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsAppear)
        {
            if (!IndexGet)
                GetIndex();
            //检测角色
            if (Physics2D.OverlapCircle(transform.position, 1f, PlayerL))
            {
                //检测到就放出att信息
                TextCanvas.SetActive(true);
                if (PlayerTouchWeapon == null)
                {
                    //获取碰到的Sprite和gameobject
                    PlayerTouchWeapon = GetComponent<SpriteRenderer>().sprite;
                    PlayerTouchObj = gameObject;
                }
                IsTouchPlayer = true;
                //角色碰到武器
            }
            else if(TextCanvas.activeInHierarchy) //如果不活动就将信息隐藏
                TextCanvas.SetActive(false);
            //如果没有碰到角色就将IsTouchPlayer设置为false并清楚碰到的Sprite和gameobject
            if (IsTouchPlayer && !Physics2D.OverlapCircle(transform.position, 1f, PlayerL))
            {
                IsTouchPlayer = false;
                PlayerTouchWeapon = null;
                PlayerTouchObj = null;
                KuangScript.BoxDisappear();
            }
            if (IsTouchPlayer && PlayerTouchObj == gameObject)
            {
                if(KuangScript.Appear == false)
                {
                    KuangScript.SetKuang(Weapon.att_attack[index][0], Weapon.att_attack[index][1], 
                        Weapon.att_attack[index][2], Weapon.att_attack[index][3]);
                    KuangScript.BoxAppear(); //框出现
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Player.GetComponent<Movement>().FloorWeaponSourcePlay();
                    SwitchWeapon();
                }
            }
        }
    }
    public void SwitchSprite()
    {
        int index = Random.Range(0, StrGun.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = _sprite[index];
        TextCanvas.GetComponent<Text>().text = StrGun[index];
    }
    public void ChestOpen()
    {
        StartCoroutine(ProgressiveAppear());
    }
    IEnumerator ProgressiveAppear()
    {
        Color color = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0);
        float a = transform.position.y;
        for (int i = 0; i < 40; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255/40);
            transform.position += new Vector3(0,(0.25f-0.075f-(transform.position.y-a))/15,0);
            yield return new WaitForSeconds(0.005f);
        }
        IsAppear = true;
    }
    public void SwitchWeapon()
    {
        if(Weapon.SpWeapon[1] == null)
        {
            Weapon.SpWeapon[1] = Weapon.SpWeapon[0];
            Weapon.SpWeapon[0] = GetComponent<SpriteRenderer>().sprite;
            KuangScript.BoxDisappear();
            Destroy(gameObject);
        }
        else
        {
            Sprite a = GetComponent<SpriteRenderer>().sprite;
           // GetComponent<SpriteRenderer>().sprite = Weapon.SpWeapon[0];
            GameObject obj = Instantiate(FloorWeapons,Player.transform.position,Quaternion.identity);
            obj.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            obj.GetComponent<SpriteRenderer>().sprite = Weapon.SpWeapon[0];
            Weapon.SpWeapon[0] = a;
            obj.GetComponent<FloorWeapon>().IsAppear = true;
            obj.GetComponent<FloorWeapon>().TextCanvas.GetComponent<Text>().text =
               StrGun[GetSpriteIndex(obj.GetComponent<SpriteRenderer>().sprite.ToString())];
            print(GetSpriteIndex(obj.GetComponent<SpriteRenderer>().sprite.ToString()));
            print(obj.GetComponent<SpriteRenderer>().sprite.ToString());
            KuangScript.BoxDisappear();
            Destroy(gameObject);
        }
        Weapon.ResetAtt();
    }
    int GetSpriteIndex(string sprite)
    {
        int result = (int)Char.GetNumericValue(sprite[8]);
        //char强转int
        if ("0123456789".Contains(sprite[9].ToString()))
        {
            result = result*10 + (int)Char.GetNumericValue(sprite[9]);
        }
        return result;
    }
    void GetIndex()
    {
        index = GetSpriteIndex(GetComponent<SpriteRenderer>().sprite.ToString());
        IndexGet = true;
    }
}
