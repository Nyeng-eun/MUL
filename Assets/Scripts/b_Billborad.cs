using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class b_Billborad : MonoBehaviour
{
    private Transform _cam;
    public WitchCtrl witchCtrl;
    [SerializeField] private Image barImage;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main.transform;
        witchCtrl = GetComponentInParent<WitchCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + _cam.rotation * Vector3.forward, _cam.rotation * Vector3.up);

        float hpRatio = (float)witchCtrl.hp / witchCtrl.b_maxHp;
        barImage.fillAmount = hpRatio;
    }
}
