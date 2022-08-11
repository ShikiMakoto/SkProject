using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Room : MonoBehaviour
{
    public GameObject UpDoor, DownDoor, LeftDoor, RightDoor;
    public bool IsUpRoom, IsDownRoom, IsRightRoom, IsLeftRoom;
    public int Step;
    public Text text;
    Movement Player;
    public int RoomXuhao;
    public LayerMask PlayerLayer;
    public Monsters Monsters;
    public Obstacles Obstacles;

    public int FightRoomIndex;

    [Header("生成物品")]
    public GameObject Chest;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Movement>();
       /* DownDoor.SetActive(IsDownRoom);
        UpDoor.SetActive(IsUpRoom);
        LeftDoor.SetActive(IsLeftRoom);
        RightDoor.SetActive(IsRightRoom);*/
        SwitchRoomType();
    }
    void FightRoomSet()
    {
        FightRoomIndex = Random.Range(1, 5);
        switch (FightRoomIndex)
        {
            case 1:
                {
                    ObstacleClon(Obstacles.Wall1);
                    break;
                }
            case 2:
                {
                    ObstacleClon(Obstacles.Wall2);
                    break;
                }
            case 3:
                {
                    ObstacleClon(Obstacles.Wall3);
                    break;
                }
            case 4:
                {
                    ObstacleClon(Obstacles.Wall4);
                    break;
                }
        }

    }
    void SwitchRoomType()
    {
        if (Step > 2)
        {
            SetNpcRoom(transform.position);
        }
        else
        {
            if (Step == 0)
                SetHomeRoom();
            else
                FightRoomSet();
        }
    }
    void SetHomeRoom()
    {

    }
    void SetNpcRoom(Vector3 pos)
    {
        int a = Random.Range(0, 2);
        if (a == 1)
        {
            Instantiate(Chest, pos, Quaternion.identity);
        }
        else FightRoomSet();
    }
    public void UpdateRoom(float xoffset,float yoffset)
    {
        Step = (int)(Mathf.Abs(transform.position.x / xoffset) + Mathf.Abs(transform.position.y / yoffset));
        text.text = Step.ToString();
    }
    private void Update()
    {
        if(Physics2D.OverlapArea(transform.position - new Vector3(9, 5.6f), transform.position + new Vector3(9, 5.6f), PlayerLayer))
        {
            Player.PlayerPlace = RoomXuhao;
        }
    }
    void ObstacleClon(GameObject obj)
    {
        //Code rule: 表示Vector3类的pos  Ex: 2(1,2)(3,4)
         GameObject obja =  Instantiate(obj, transform.position,Quaternion.identity);
        obja.GetComponent<MonsterGetRoomIndex>().RoomIndex = RoomXuhao;
    }
  /*  void GoblinClon(string Code,GameObject obj)
    {
        //Code rule: 第一项表示项目数，之后的表示Vector3类的pos  Ex: 2(1,2)(3,4)
        for (int i = 0; i < Code[0]-48; i++)
        {
            GameObject obje =  Instantiate(obj,ParseCodePos(Code,i)+transform.position, Quaternion.identity);
            obje.GetComponent<MonsterMove>().RoomNum = RoomXuhao;
        }
    }*/
    Vector3 ParseCodePos(string Code,int num)
    {
        Vector3 [] pos=new Vector3 [10];
        int MonsterNum =0; 
        float x=0, y=0;
        bool IsFindPosX=false,IsFindPosY=false,IsPoint = false;
        int PointNum = 0;
        //找到x坐标 找到y坐标 遇到小数点
        for (int i = 0; i < Code.Length; i++)
        {
            if(Code[i]=='('|| Code[i]==')'|| Code[i] == ','||Code[i]=='-')
            {
                if (Code[i] == '(')
                {
                    IsFindPosX = true;
                }
                if (Code[i] == ',')
                {
                    IsFindPosX = false;
                    IsFindPosY = true;
                    IsPoint = false;
                    x = x / Mathf.Pow(10, PointNum);
                    PointNum = 0;
                }
                if (Code[i] == ')')
                {
                    IsFindPosY = false;
                    IsPoint = false;
                    y = y / Mathf.Pow(10, PointNum);
                    PointNum = 0;
                    pos[MonsterNum] = new Vector3(x, y, 0);
                    MonsterNum++;
                    x = 0;
                    y = 0;
                }
            }
            else
            {
                if (Code[i] != '.')
                {
                    if (IsPoint)
                    {
                        PointNum++;
                    }
                    if (IsFindPosX)
                    {
                        x = x * 10 + Code[i] - 48;
                    }
                    if (IsFindPosY)
                    {
                        y = y * 10 + Code[i] - 48;
                    }
                    //获取position
                }
                else
                {
                    IsPoint = true;
                }
            }
        }
        //1的ASCALL码是48
        return pos[num];
    }
}
[System.Serializable]
public class Monsters
{
    public GameObject M_Goblin;
    public GameObject G_Goblin;
}
[System.Serializable]
public class Obstacles
{
    public GameObject Wall1;
    public GameObject Wall2;
    public GameObject Wall3;
    public GameObject Wall4;
    public GameObject Wall5;
    public GameObject Wall6;
}
