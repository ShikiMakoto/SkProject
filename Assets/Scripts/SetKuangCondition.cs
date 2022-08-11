using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetKuangCondition : MonoBehaviour
{
    public Text Attack_Get, Energy_Get, offset_Get, CrinalHit_Get;
    public bool Appear = false;
    public void SetKuang(float Attack,float Energy,float offset,float CrinalHit)
    {
        Attack_Get.text = Attack.ToString();
        Energy_Get.text = Energy.ToString();
        offset_Get.text = offset.ToString();
        CrinalHit_Get.text = CrinalHit.ToString();
    }
    public void BoxAppear()
    {
        gameObject.SetActive(true);
        Appear = true;
    }
    public void BoxDisappear()
    {
        gameObject.SetActive(false);
        Appear = false;
    }
}
