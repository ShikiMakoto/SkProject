using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFollowCamera : MonoBehaviour
{
    public int Xuhao;
    Movement Move;
    public GameObject Camera;
    public Color BtColor, DkColor;
    Vector3 pos = new Vector3(114514,1919810,0);
    Vector3 AddPos = new Vector3(6,2,0);
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        Move = GameObject.Find("Player").GetComponent<Movement>();
        Camera = GameObject.Find("Main Camera").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (Move.PlayerPlace == Xuhao)
        {
            GetComponent<SpriteRenderer>().color = BtColor;
        }
        else 
        GetComponent<SpriteRenderer>().color = DkColor;
    }
    private void FixedUpdate()
    {
        if (pos != new Vector3(114514, 1919810, 0))
        {
           transform.position = new Vector3(Camera.transform.position.x + pos.x+AddPos.x, Camera.transform.position.y + pos.y+AddPos.y, 0);
        }
    }
}
