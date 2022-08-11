using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public GameObject DkRoom, DkCorridorUD,DkCorridorLR;
    public List<MapFollowCamera> GmapList = new List<MapFollowCamera>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GmapList.Count == 10)
        {
            for (int i = 0; i < 10; i++)
            {
                GmapList[i].Xuhao = i;
            }
        }
    }
    public void GenerateWallMap(Vector3 position, int code)
    {
        //Code: LR 2 UD 3 Room 1
        switch (code)
        {
            case 1:
            GmapList.Add(Instantiate(DkRoom, position * 0.02f, Quaternion.identity).GetComponent<MapFollowCamera>());
            break;
            case 2:
            Instantiate(DkCorridorLR, position * 0.02f, Quaternion.identity).GetComponent<MapFollowCamera>();
            break;
            case 3:
            Instantiate(DkCorridorUD, position * 0.02f, Quaternion.identity).GetComponent<MapFollowCamera>();
            break;
        }

    }
    
}
