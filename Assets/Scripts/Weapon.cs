using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Weapon : MonoBehaviour
{
    public Transform Player;
    const float StartX = 1.8f, StartY = -0.42f;
    public GameObject Bullet;
    public Sprite[] SpWeapon = new Sprite[2];
    Vector3 StartPos = new Vector3(StartX, StartY,0);
    AudioSource audio;
    public float WeaponAttact;
    public float WeaponEnergy;
    string[] Att;
    Sprite[] _sprite = new Sprite[24];
    public float[][] att_attack = new float[50][];

    [Header("music")]
    public AudioClip gun1;
    public AudioClip gun2;
    public AudioClip gun3;
    public AudioClip gun4;
    public AudioClip gun5;
    public AudioClip gun6;
    public AudioClip switchmu;

    void Start()
    {
        SpWeapon[0] = GetComponent<SpriteRenderer>().sprite;
        audio = GetComponent<AudioSource>();
        Att = Resources.Load<TextAsset>("att").text.Split('\n');
        for (int i = 0; i < Att.Length; i++)
        {
            att_attack[i] = SplitString(Att[i]);
        }
        ResetAtt();
        for (int i = 1; i < Att.Length; i++)
        {
            _sprite[i-1] = Resources.Load<Sprite>("Bullets_" + i.ToString());
        }
    }
    int Direction;
    bool IsMouseDown = false;
    Color BulletColor = new Color(255, 255, 255, 255); //用于SwitchBullet返回值
    float[] SplitString(string str) 
        //自制的split方法
    {
        string a = null;
        float[] result = new float[10];
        int franktime = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '|')
            {
                result[franktime] = StrToFloat(a);
                franktime++;
                a = null;
            }
            else a = a + str[i];
        }
        return result;
    }
    void FixedUpdate()
    {
        if (!Player.GetComponent<Movement>().PlayerDie)
        {
            // 获取鼠标位置相对移动向量
            Vector2 translation = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            // 鼠标坐标默认是屏幕坐标，首先要转换到世界坐标
            // 物体坐标默认就是世界坐标
            // 两者取差得到方向向量
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.transform.position;
            // 方向向量转换为角度值
            //========================================================================

            float angle = 360 - Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            //========================================================================
            // 将当前物体的角度设置为对应角度
            if (Player.transform.localScale.x > 0)
                Direction = 1;
            else Direction = -1;
            transform.Translate(new Vector3(-0.5f, 0, 0));
            transform.eulerAngles = new Vector3(0, 0, angle + 90 * Direction);
            transform.Translate(new Vector3(0.5f, 0, 0));
            if (Input.GetMouseButton(0) && !IsMouseDown && Player.gameObject.GetComponent<Movement>().PlayerEnergy > 0)
            {
                IsMouseDown = true;
                StartCoroutine(MouseDown(10));
                //等待，固定时间发出子弹
                GameObject obj = Instantiate(Bullet, transform.position, transform.rotation);
                obj.GetComponent<SpriteRenderer>().sprite =
                    _sprite[SwitchBullet(GetSpriteIndex(GetComponent<SpriteRenderer>().sprite.ToString()))];
                obj.GetComponent<SpriteRenderer>().color = BulletColor;
                PlayerEnergyDown();
            }
            GetComponent<SpriteRenderer>().sprite = SpWeapon[0];
            if (Input.GetKeyDown(KeyCode.R) && SpWeapon[1] != null)
            {
                SwapWeapon();
            }
        }
    }
    int SwitchBullet(int index)
    {
        SwitchGunSF(index);
        int result =0;
        if (index == 0 || index == 3||index == 6||index==15||index==11)
        {
            result = 14;
            if (index == 0)
                BulletColor = new Color(0, 233, 255, 255);
            if (index == 3||index==11)
                BulletColor = new Color(255, 0, 0, 255);
            if(index==15)
                BulletColor = new Color(255, 255, 0, 255);
            if (index == 6)
                BulletColor = new Color(0, 255, 0, 255);
            // BulletColor = new Color(16, 234, 10, 255);
        }
        else BulletColor = new Color(255,255,255,255);
        if (index == 1 || index == 2||index==5||index==4)
        {
            result = 12;
        }
        if (index == 10||index==23||index==7)
        {
            result = 6;
        }
        if (index == 21||index==8)
        {
            result = 5;
        }
        return result;
    }
    void SwitchGunSF(int index)
    {
        AudioClip result = gun1;
        if (index >= 7 && index <= 10) //7,8,9,10
        {
            result = gun3;
        }
        if (index == 0 || index == 3 || index == 6||index ==11)
        {
            result = gun4;
        }
        if(index == 19 ||index == 17)
        {
            result = gun5;
        }
        audio.PlayOneShot(result);
    }
    public void ResetAtt()
    {
        WeaponAttact = att_attack[GetSpriteIndex(SpWeapon[0].ToString())][0];
        WeaponEnergy = att_attack[GetSpriteIndex(SpWeapon[0].ToString())][1];
    }
    public float StrToFloat(object FloatString)
    {
        float result;
        if (FloatString != null)
        {
            if (float.TryParse(FloatString.ToString(), out result))
                return result;
            else
            {
                return (float)0.00;
            }
        }
        else
        {
            return (float)0.00;
        }
    }
    void PlayerEnergyDown()
    {
        Player.gameObject.GetComponent<Movement>().PlayerEnergy-=WeaponEnergy;
    }
    IEnumerator MouseDown(int frame)
    {
        for (int i = 0; i < frame; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        IsMouseDown = false;
    }
    void SwapWeapon()
    {
        audio.PlayOneShot(switchmu);
        Sprite a = SpWeapon[0];
        SpWeapon[0] = SpWeapon[1];
        SpWeapon[1] = a;
        ResetAtt();
    }
    int GetSpriteIndex(string sprite)
    {//weapons_14
        int result = (int)Char.GetNumericValue(sprite[8]);
        //char强转int
        if ("0123456789".Contains(sprite[9].ToString()))
        {
            result = result * 10 + (int)Char.GetNumericValue(sprite[9]);
        }
        print(result);
        return result;
    }
    // Update is called once per frame
}
