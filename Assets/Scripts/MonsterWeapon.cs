using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    public GameObject Player;
    public GameObject Monster;
    public BoxCollider2D colli;
    public LayerMask PlayerLayer;
    public GameObject BulletPre;
    bool AttOver = true;
    int Direct;
    int RoomNum;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        StartCoroutine(Move());
        if (name == "Gun")
        {
            Invoke("GunAttack", 2f);
        }
        RoomNum = Monster.GetComponent<MonsterMove>().RoomNum;
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Monster.transform.localScale.x;
        if (direction > 0)
        {
            transform.localScale = new Vector2(2.855f, 2.855f); Direct = 1;
        }
        else { transform.localScale = new Vector2(-2.855f, 2.855f); Direct = -1; }
    }
    IEnumerator Move()
    {
        float angle = Mathf.Atan2(Player.transform.position.y - transform.position.y,
 Player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        for (int i = 0; i < 10; i++)
        {
            if (Mathf.Abs( angle -Change( Mathf.Atan2(Player.transform.position.y - transform.position.y,
 Player.transform.position.x - transform.position.x) * Mathf.Rad2Deg)) > 300)
            {
                transform.eulerAngles = new Vector3(0,0, Change(Mathf.Atan2(Player.transform.position.y - transform.position.y,
                Player.transform.position.x - transform.position.x) * Mathf.Rad2Deg));
                break;
            }
            else
            {
                transform.eulerAngles += new Vector3(0, 0, (-transform.eulerAngles.z + angle) / 10);
            }
            yield return new WaitForSeconds(0.005f);

        }
        StartCoroutine(Move());
    }
    float Change(float a)
    {
        if (a > 0) return a;
        else return a + 360;
    }
    public void Attact()
    {
        if (AttOver)
        {
            AttOver = false;
            StartCoroutine(Aattact(Direct));
        }
    }
    IEnumerator Aattact(int Direct)
    {
        yield return new WaitForSeconds(0.8f);
        Vector3 Eular = transform.eulerAngles;
        Vector3 EularNow = transform.eulerAngles;
        for (int i = 0; i < 2; i++)
        {
            EularNow = transform.eulerAngles;
            transform.eulerAngles = Eular;
            transform.Translate(new Vector3(0.5f/2 * Direct, 0, 0));
            transform.eulerAngles = EularNow;
            if(colli.IsTouchingLayers(PlayerLayer))
            {
                // Player.GetComponent<Movement>().TouchBullet
                Player.GetComponent<Movement>().HPoff(2);
            }
            yield return new WaitForEndOfFrame();
        }
        Eular = transform.eulerAngles;
        for (int i = 0; i < 30; i++)
        {
            EularNow = transform.eulerAngles;
            transform.eulerAngles = Eular;
            transform.Translate(new Vector3(-0.5f / 30*Direct,0, 0));
            transform.eulerAngles = EularNow;
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(0.46f* 0.166953f, -1.1f* 0.166953f, 0)+Monster.transform.position;
        AttOver = true;
    }
    void GunAttack()
    {
        if (Player.GetComponent<Movement>().PlayerPlace == RoomNum)
        {
            Instantiate(BulletPre, transform.position, transform.rotation);
            if (Random.Range(0, 2) == 1)
            {
                Invoke("GunAttack", Random.Range(4,6));
            }
            else Invoke("GunAttack", 0.2f);
        }
        else Invoke("GunAttack", 1f);
        //每1s侦测一次角色位置
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PlayerTag"&&name=="Mao")
        {
            Attact();
        }
    }
}
