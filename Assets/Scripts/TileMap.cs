using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    Vector3 posadd;
    public LayerMask wall;
    Movement Player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            //爆炸范围半径1M
            float r = 2.5f;
            //得到圆心为collision.contacts[0].point，半径为r的圆中间所有的碰撞体
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, r);
            //遍历碰撞体
            foreach (Collider2D collider in colliders)
            {
                //判断是不是可以被击飞的箱子
                if (collider.tag == "PlayerTag")
                {
                    //posadd为一个带方向的单位向量
                    posadd = (collider.transform.position - transform.position)
                        / ((collider.transform.position - transform.position).magnitude);
                        Player.Bang(posadd);
                    // collider.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3, 3), ForceMode2D.Force);
                }
            }
        }
    }
    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Movement>();
    }
}
