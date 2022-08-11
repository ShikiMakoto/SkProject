using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Animator anim;
    int[] P = new int[MAXVEX];
    int[] D = new int[MAXVEX];
    const int INFNITY = 32767;
    const int MAXVEX = 25;
    public LayerMask layer;
    char[] vexs = new char[MAXVEX];
    int[,] arc = new int[MAXVEX, MAXVEX];
    List<Vector3> lujing = new List<Vector3>();
    Vector3[] pos = new Vector3[MAXVEX];
    Vector3 Destination;
    GameObject Player;
    public GameObject Weapon;
    public float MonsterHP = 30;
    bool IsLoadOver= false;
    bool MoveRule= true; //规定能否运动
    public int RoomNum;//所在房间编号
    private void Start()
    {
        Destination = transform.position;
        Player = GameObject.Find("Player");
        GraphStart();
        createPos();
        LoadWall();
        IsLoadOver = true;
        // Dijkstraa(0, P, D);
        //ShowShortestPath(0, P, D);
        //StartCoroutine("Dijkstra");
        anim = GetComponent<Animator>();
        GameObject Parent = this.transform.parent.gameObject;
        RoomNum = Parent.GetComponent<MonsterGetRoomIndex>().RoomIndex;
         StartCoroutine(NewMove(Random.Range(0, 360)));

    }
    private void Update()
    {
        if (Player.GetComponent<Movement>().PlayerPlace == RoomNum)
            MoveRule = true;
        else 
            MoveRule = false;
        
        // if(Physics2D.Raycast())
    }
    float Change(float a)
    {
        if (a > 0) return a;
        else return a + 360;
    }
    IEnumerator NewMove(float MoveDirect)
    {
        for (int j = 0; j < Random.Range(0, 6); j++)
        {
            if (MoveRule)
            {
                int RamdomNum = Random.Range(0, 3);
                anim.SetBool("IsMove", true);
                if (RamdomNum == 1)
                {
                    float angle = Mathf.Atan2(Player.transform.position.y - transform.position.y,
        Player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                    if (angle < 0) angle += 360;
                    MoveDirect = angle;
                }
                if (MoveRule)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, MoveDirect);
                    transform.Translate(new Vector3(0.07f, 0, 0));
                    if (Physics2D.OverlapCircle(transform.position, 0.4f, layer))
                    {
                        transform.Translate(new Vector3(-0.07f, 0, 0));
                    }
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                    yield return new WaitForEndOfFrame();
                }
                for (int i = 0; i < Random.Range(20, 50); i++)
                {
                    if (!MoveRule)
                    {
                        continue;
                    }
                    if (!Physics2D.OverlapCircle(transform.position, 0.5f, layer) && MoveRule)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, MoveDirect);
                        transform.Translate(new Vector3(0.07f, 0, 0));
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                        yield return new WaitForEndOfFrame();
                    }
                }
                anim.SetBool("IsMove", false);
            }
        }
        if(!MoveRule)
            Invoke("startnewmove", Random.Range(1.5f, 2f));
        else
            Invoke("startnewmove", Random.Range(0,0.3f));
    }
    int PlayerPos = -2;
    void startnewmove()
    {
        StartCoroutine(NewMove(Random.Range(0, 360)));
    }
    IEnumerator Dijkstra()
    {
        if (IsLoadOver)
        {
            LoadWall();
            GraphStart();
            createPos();
            int xuhao = -1;
            float min = 11454;
            for (int i = 0; i < MAXVEX; i++)
            {
                if (min > (Player.transform.position - pos[i]).magnitude)
                {
                    min = (Player.transform.position - pos[i]).magnitude;
                    xuhao = i;
                }
            }
            Dijkstraa(0, P, D);
            PlayerPos = xuhao;
            if (Mathf.Abs( transform.position.x - Player.transform.position.x)/1 <2.1f&& 
                Mathf.Abs(transform.position.y - Player.transform.position.y) / 1 < 2.1f)
            {
                Dijkstraa(0, P, D);
                // =========================================
                int i = 0, j = 0;
                lujing.Clear();
                if (PlayerPos > -1)
                {
                    i = PlayerPos;
                    j = i;
                    while (j != 0)
                    {
                        j = (P)[j];
                        lujing.Add(pos[j]);
                    }
                    for (int m = 0; m < lujing.Count; m++)
                    {
                         //if (transform.position.x - 0.5 % 1 == 0 || transform.position.y - 0.5 % 1 == 0)
                        {
                            if (lujing.Count > 1)
                            {
                                Destination = lujing[lujing.Count - 2];
                            }
                            else Destination = transform.position;

                        }
                        //确保怪物走完一整格再改变路径
                    }
                    Vector3 route = (Destination - transform.position) / 10;
                    //print(route);
                    if (route.x > 0.05f)
                        transform.localScale = new Vector3(0.166953f, transform.localScale.y);
                    else if (route.x < -0.05f)
                        transform.localScale = new Vector3(-0.166953f, transform.localScale.y);
                    for (int m = 0; m < 10; m++)
                    {
                        if (transform.position != Destination)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                yield return new WaitForSeconds(0.001f);
                            }
                            transform.position += route;
                            anim.SetBool("IsMove", true);
                        }
                        else break;
                    }
                    anim.SetBool("IsMove", false);
                }
                // =================================================================
            }
        }
        yield return null;
        StartCoroutine("Dijkstra");
    }
    void GraphStart()
    {
        for (int i = 0; i < MAXVEX; i++)
            for (int j = 0; j < MAXVEX; j++)
                arc[i, j] = INFNITY;
        for (int i = 0; i < MAXVEX; i++)
        {
            for (int j = 0; j < 25; j++)
            {
                if (i < 9 && j < 9)
                {
                    if ((i - i % 3) / 3 == (j - j % 3) / 3 && Mathf.Abs(j - i) == 1)
                    {
                        arc[i, j] = Mathf.Abs(j - i);
                    }
                    if (i % 3 == j % 3 && Mathf.Abs((j - i) / 3) == 1)
                    {
                        arc[i, j] = Mathf.Abs((j - i) / 3);
                    }
                }
                if (i >= 9 && i < 15 && j >= 9 && j < 15)
                {
                    if ((i - i % 3) / 3 == (j - j % 3) / 3 && Mathf.Abs(j - i) == 1)
                    {
                        arc[i, j] = Mathf.Abs(j - i);
                    }
                    if (i % 3 == j % 3 && Mathf.Abs((j - i) / 3) == 1)
                    {
                        arc[i, j] = Mathf.Abs((j - i) / 3);
                    }
                }
                if (i > 14)
                {
                    if ((i - 1 - (i - 1) % 2) / 2 == (j - (j - 1) % 2) / 2 && Mathf.Abs(j - i) == 1)
                    {
                        arc[i, j] = Mathf.Abs(j - i);
                    }
                    if ((j - i) % 2 == 0 && Mathf.Abs((j - i) / 2) == 1)
                    {
                        arc[i, j] = Mathf.Abs((j - i) / 2);
                    }
                }
                if (i == j)
                {
                    arc[i, j] = 0;
                }
            }
        }
        arc[0, 12] = 1;
        arc[1, 13] = 1;
        arc[2, 14] = 1;
        arc[0, 20] = 1;
        arc[3, 22] = 1;
        arc[6, 24] = 1;
        for (int i = 0; i < MAXVEX; i++)
            for (int j = 0; j < MAXVEX; j++)
                arc[j, i] = arc[i, j];
    }
    void createPos()
    {
        Vector3 StartPos = transform.position;
        for (int i = 0; i < MAXVEX; i++)
        {
            if (i < 9)
            {
                pos[i] = new Vector3(i % 3, -(i - i % 3) / 3) + StartPos;
            }
            if (i >= 9 && i < 15)
            {
                pos[i] = new Vector3(i % 3, 5 - (i - i % 3) / 3) + StartPos;
            }
            if (i > 14)
            {
                pos[i] = new Vector3((i - 1) % 2 - 2, 9 - (i - 1 - (i - 1) % 2) / 2) + StartPos;
            }
        }
    }
    void Dijkstraa(int V0, int[] P, int[] D)
    {
        int v, w, k = 0, min;
        int[] final = new int[MAXVEX];//final[w]==1,表示已经求得起点V0到w的最短路径
        for (v = 0; v < MAXVEX; v++)
        {
            final[v] = 0;//初始化，V0到所有的顶点的最短路径都没求到
            (D)[v] = arc[V0, v];//默认起点V0到V的最短路径相应邻接矩阵的权值
            (P)[v] = 0;//初始化路径数组p为0;
        }
    (D)[V0] = 0;//起点V0到V0的路径长度为0
        final[V0] = 1; //V0到V0的最短路径不需要求
        for (v = 1; v < MAXVEX; v++)
        {
            min = INFNITY;
            for (w = 0; w < MAXVEX; w++)
            {
                if (final[w] != 1 && (D)[w] < min)
                {
                    k = w;
                    min = (D)[w];
                }
            }
            final[k] = 1;//将目前找到的最近的顶点置1
            for (w = 0; w < MAXVEX; w++)
            {
                /*如果经过v的顶点的路径比现在这条路径长度短的话*/
                if (final[w] != 1 && (min + arc[k, w]) < (D)[w])
                {
                    /*说明找到了更短的路径，修改D[w]和P[w]*/
                    (D)[w] = min + arc[k, w];
                    (P)[w] = k;
                }
            }
        }
    }
    //打印最小路径信息
    void LoadWall()
    {
        bool IsWall = false;
        for (int i = 0; i < MAXVEX; i++)
        {
            IsWall = Physics2D.OverlapCircle(pos[i], 0.2f, layer);
            if (IsWall)
            {
                for (int j = 0; j < MAXVEX; j++)
                {
                    arc[i, j] = INFNITY;
                    arc[j, i] = INFNITY;
                }
            }
            arc[i, i] = 0;
            IsWall = false;
        }
    }
    float direction;
    int Direction;
    public void StartIETouchBullet(float directionC, int DirectionC)
    {
        //起承接量的作用
        direction = directionC;
        Direction = DirectionC;
        StartCoroutine(TouchBullet());
    }
    public IEnumerator TouchBullet()
    {
        MoveRule = false;
        if (! (MonsterHP > 0)) Destroy(gameObject);
        float r = 0.3f*Direction;
       // transform.eulerAngles = new Vector3(0, 0, direction);
        float x = Mathf.Cos(direction / Mathf.Rad2Deg) * r/5, y = Mathf.Sin(direction / Mathf.Rad2Deg) * r/5;
        for (int i = 0; i < 10; i++)
        {
            //transform.eulerAngles = new Vector3(0, 0, direction);
            transform.position += new Vector3(x, y);
            //transform.eulerAngles = new Vector3(0, 0, 0);
            yield return new WaitForEndOfFrame();

        }
        transform.eulerAngles = new Vector3(0,0,0);
        MoveRule = true;
    }
}
