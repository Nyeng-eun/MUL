using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billborad : MonoBehaviour
{
    private Transform _cam;
    public MonsterCtrl monsterCtrl;
    [SerializeField] private Image barImage;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main.transform;
        monsterCtrl = GetComponentInParent<MonsterCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + _cam.rotation * Vector3.forward, _cam.rotation * Vector3.up);

        float hpRatio = (float)monsterCtrl.hp / monsterCtrl.maxHp;
        barImage.fillAmount = hpRatio;
    }
}
