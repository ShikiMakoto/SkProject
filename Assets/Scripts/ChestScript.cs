using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public LayerMask PlayerL;
    public GameObject Into, Down, Left, Right,TextCanvas;
    bool IsOpenChest = false;
    public FloorWeapon floorWeapon;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        TextCanvas.SetActive(false);
        //Left.transform.position = new Vector3(-0.2198f, -0.1683f, 0);
        //Right.transform.position = new Vector3(0.2059f, -0.1683f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOpenChest)
        {
            if (Physics2D.OverlapCircle(transform.position, 1f, PlayerL))
            {
                TextCanvas.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    audio.PlayOneShot(audio.clip);
                    StartCoroutine(OpenChest());
                }
            }
            else TextCanvas.SetActive(false);
        }
    }
    IEnumerator OpenChest()
    {
        floorWeapon.ChestOpen();
        floorWeapon.SwitchSprite();
        for (int i = 0; i < 20; i++)
        {
            Left.transform.position -= new Vector3(0.36f/20, 0 , 0);
            Right.transform.position += new Vector3(0.36f / 20, 0, 0);
            yield return new WaitForSeconds(0.005f);
        }
        // Left.transform.position = new Vector3(-0.489f, -0.1683f, 0);
        //Right.transform.position = new Vector3(0.477f, -0.1683f, 0);
        IsOpenChest = true;
        TextCanvas.SetActive(false);
    }
}
