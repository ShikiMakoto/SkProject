using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour
{
    // Start is called before the first frame update
    Movement Player;

    bool TouchStrike= false;
    void Start()
    {
        Invoke("StrikeWhile", 1f);
        Player = GameObject.Find("Player").GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StrikeWhile()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        Invoke("StrikeWhile", 1f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "PlayerTag"&&!TouchStrike)
        {
            Player.HPoff(2);
          //  TouchStrike = true;
           // Invoke("SetStrike", 1f);
        }
    }
    void SetStrike()
    {
        TouchStrike = false;
    }
}
