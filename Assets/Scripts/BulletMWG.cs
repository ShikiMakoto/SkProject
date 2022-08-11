using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMWG : MonoBehaviour
{
    const float r = 0.04212f;
    GameObject Player;
    public LayerMask wall;
    // Start is called before the first frame update
    private void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Physics2D.OverlapCircle(transform.position, 0.25f, wall))
            {
                Destroy(gameObject);
            }
            else transform.Translate(new Vector3(speed / 10, 0, 0));
        }
    }
    void Start()
    {
        Player = GameObject.Find("Player");
        // transform.Translate(new Vector3(0, 0.1f, 0));
        //调整位置以至美观
    }
    Vector3 point = new Vector3(r, r, 0);
    float speed = 0.3f;
    int Direction;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerTag")
        {
            Player.GetComponent<Movement>().HPoff(3);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "BulletTouchDestry")
        {
            Destroy(gameObject);
        }
    }
}
