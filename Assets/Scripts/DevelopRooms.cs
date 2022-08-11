using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevelopRooms : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction direction;
    public List<Room> rooms = new List<Room>();
    public  GenerateMap GMap;
    float MaxStep;
    List<GameObject> FarRooms = new List<GameObject>();
    public WallType wallType;
    public FightRoomPrefab FRP;

    [Header("房间信息")]

    public GameObject RoomPrefab;
    public int RoomNumber;
    public Color StartColor, EndColor;
    GameObject EndRoom;

    [Header("位置信息")]

    public Transform Point;
    public float xoffset, yoffset;


    void Start()
    {
        //生成房间
        rooms.Add(Instantiate(RoomPrefab, Point.position, Quaternion.identity).GetComponent<Room>());
        for (int i = 1; i < RoomNumber; i++)
        {
            //生成9个房间，i=1是因为生成初始房间
            SwitchRoom();
            rooms.Add(Instantiate(RoomPrefab,Point.position,Quaternion.identity).GetComponent<Room>());
            //克隆房间并安排位置
        }

        rooms[0].GetComponent<SpriteRenderer>().color = StartColor;
        EndRoom = rooms[0].gameObject;
        MaxStep = 0;
            //初始化

        //获取数据,在非最终房间时进行(由于最终房间是在获取之后产生，不需要限制条件)
        foreach (var room in rooms)
        {
            room.UpdateRoom(xoffset,yoffset);
        }

        foreach (var room in rooms)
        {
                if (room.Step > MaxStep)
                {
                    MaxStep = room.Step;
                    //寻找最大的物理距离
                }
                ManageDoor(room, room.gameObject.transform.position);
            //获取旁边是否有房间的数据
        }
        foreach (var room in rooms)
        {
                if (room.Step == MaxStep)
                {
                    FarRooms.Add(room.gameObject);
                    //寻找物理距离最远的房间
                }
        }


        EndRoom = FarRooms[Random.Range(0, FarRooms.Count)];  //中转站 获取物理距离最远的房间

        //生成最终房间,走廊
        GenerateLastRoom(EndRoom); // 在生成最后一个房间后一起生成走廊和墙壁，否则可能错位

        int a = 0;
        //临时变量
        foreach (var room in rooms)
        {
            room.RoomXuhao = a;
            a++;  //获取房间的序号
            Point.position = new Vector3(room.transform.position.x, room.transform.position.y, 0);
            GenerateWalls(room.IsUpRoom, room.IsDownRoom, room.IsLeftRoom, room.IsRightRoom, Point.position);
            //Generate walls
            GMap.GenerateWallMap(Point.position,1);//生成小地图
            GenerateCorridors(room);
        }
    }



    // Update is called once per frame
    void Update()
    {
      //  if (Input.anyKeyDown)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       // }
    }
    public void GenerateCorridors(Room room)
    //根据相对原理，只要检测两个方向有无门就能生成足够的走廊
    {
        if (room.IsRightRoom)
        {
            Instantiate(wallType.CorridorLR, room.transform.position + new Vector3(xoffset / 2, 0, 0), Quaternion.identity);
            GMap.GenerateWallMap(room.transform.position + new Vector3(xoffset / 2, 0, 0),2);
        }
        if (room.IsUpRoom)
        {
            Instantiate(wallType.CorridorUD, room.transform.position + new Vector3(0, yoffset / 2, 0), Quaternion.identity);
            GMap.GenerateWallMap(room.transform.position + new Vector3(0, yoffset / 2, 0), 3);
        }
    }
    public void GenerateWalls(bool Up,bool Down,bool Left,bool Right,Vector3 Point)
    {
        int WallCode =0;  //WallCode为房间代码，为伪二进制数
        if (Up) WallCode += 1000;
        if (Down) WallCode += 100;
        if (Left) WallCode += 10;
        if (Right) WallCode += 1;
        switch (WallCode)
        {
            case 1000: 
                Instantiate(wallType.U,Point,Quaternion.identity);
                break;
            case 1100:
                Instantiate(wallType.UD, Point, Quaternion.identity);
                break;
            case 1110:
                Instantiate(wallType.ULD, Point, Quaternion.identity);
                break;
            case 1101:
                Instantiate(wallType.URD, Point, Quaternion.identity);
                break;
            case 1001:
                Instantiate(wallType.UR, Point, Quaternion.identity);
                break;
            case 1010:
                Instantiate(wallType.UL, Point, Quaternion.identity);
                break;
            case 1011:
                Instantiate(wallType.ULR, Point, Quaternion.identity);
                break;
            case 100:
                Instantiate(wallType.D, Point, Quaternion.identity);
                break;
            case 110:
                Instantiate(wallType.DL, Point, Quaternion.identity);
                break;
            case 101:
                Instantiate(wallType.DR, Point, Quaternion.identity);
                break;
            case 111:
                Instantiate(wallType.DLR, Point, Quaternion.identity);
                break;
            case 10:
                Instantiate(wallType.L, Point, Quaternion.identity);
                break;
            case 11:
                Instantiate(wallType.LR, Point, Quaternion.identity);
                break;
            case 1:
                Instantiate(wallType.R, Point, Quaternion.identity);
                break;
            case 1111:
                Instantiate(wallType.ALL, Point, Quaternion.identity);
                break;
        }
    }
    public void ChangeRoomPos(Direction direction)
    {
        switch (direction) 
        {
            case Direction.up:
            {
                 Point.position += new Vector3(0, yoffset, 0);             
                    break;
            }
            case Direction.down:
                {
                    Point.position += new Vector3(0, -yoffset, 0);
                    break;
                }
            case Direction.left:
                {
                    Point.position += new Vector3(-xoffset, 0, 0);
                    break;
                }
            case Direction.right:
                {
                    Point.position += new Vector3(xoffset, 0, 0);
                    break;
                }
        }

    } //SwitchRoom的子函数，移动房间位置
    public void SwitchRoom()
    {
        //生成房间时防止房间重叠

        bool SamePos;
        do
        {
            SamePos = false;
            ChangeRoomPos((Direction)Random.Range(0, 3));
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].transform.position.x == Point.position.x && rooms[i].transform.position.y == Point.position.y)
                {
                    SamePos = true;
                }
            }
        } while (SamePos);
    } //生成房间
    public void GenerateLastRoom(GameObject EndRoom) 
    {
        int direction=0;
        Point.position = new Vector3(EndRoom.transform.position.x, EndRoom.transform.position.y);
        Room b = EndRoom.GetComponent<Room>();
        if (b.IsUpRoom == false)
        {
            ChangeRoomPos(Direction.up);
            b.IsUpRoom = true;
            direction = 1;
        }else
        if (b.IsDownRoom == false)
        {
            ChangeRoomPos(Direction.down);
            b.IsDownRoom = true;
            direction = 2;
        }else
        if (b.IsLeftRoom == false)
        {
            ChangeRoomPos(Direction.left);
            b.IsLeftRoom = true;
            direction = 3;
        }else
        if (b.IsRightRoom == false)
        {
            ChangeRoomPos(Direction.right);
            b.IsRightRoom = true;
            direction = 4;
        }
        rooms.Add(Instantiate(RoomPrefab, Point.position, Quaternion.identity).GetComponent<Room>());
        Room a = rooms[RoomNumber];
        EndRoom = a.gameObject;
        if (direction==1)
            a.IsDownRoom =true;
        if (direction==2)
            a.IsUpRoom = true;
        if (direction==3)
            a.IsRightRoom = true;
        if (direction==4)
            a.IsLeftRoom = true;
        EndRoom.GetComponent<SpriteRenderer>().color = EndColor;
        EndRoom.GetComponent<Room>().UpdateRoom(xoffset,yoffset);

        GenerateWalls(a.IsUpRoom,a.IsDownRoom,a.IsLeftRoom,a.IsRightRoom,EndRoom.transform.position);


    }  //生成最终房间 有待完善
    public void ManageDoor(Room room,Vector3 Position)
    {
        float Updigit, Downdigit, Leftdigit, Rightdigit;

        Updigit = Position.y + yoffset;
        Downdigit = Position.y - yoffset;
        Leftdigit = Position.x - xoffset;
        Rightdigit = Position.x + xoffset;
        room.IsUpRoom = false;
        room.IsDownRoom = false;
        room.IsRightRoom = false;
        room.IsLeftRoom = false;
        for (int i = 0; i < RoomNumber; i++)
        {
            if (rooms[i].transform.position.y == Position.y)
            {
                if (rooms[i].transform.position.x == Leftdigit)
                {
                    room.IsLeftRoom = true;
                }
                if (rooms[i].transform.position.x == Rightdigit)
                {
                    room.IsRightRoom = true;
                }
            }
            if (rooms[i].transform.position.x == Position.x)
            {
                if (rooms[i].transform.position.y == Updigit)
                {
                    room.IsUpRoom = true;
                }
                if (rooms[i].transform.position.y == Downdigit)
                {
                    room.IsDownRoom = true;
                }
            }

        }
    }  //获取各方位是否有房间的数据（布置门
}

[System.Serializable]
public class WallType
{
    public GameObject U, D, L, R, UL, UR, UD, ULR, ULD, URD,
                      DL, DR,DLR, LR,ALL,CorridorLR,CorridorUD;
}
[System.Serializable]
public class FightRoomPrefab
{
    public GameObject FR1;
}

