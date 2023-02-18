using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hp : MonoBehaviour
{
    [SerializeField] Image hpbar;
    [SerializeField] EnemyMove hp;
    [SerializeField] int hp_default;

    [SerializeField] GameObject par;
    [SerializeField] Canvas canvas;

    [SerializeField] float offset;

    private void Awake()
    {
        hpbar = gameObject.GetComponent<Image>();
        hp = GetComponentInParent<EnemyMove>();
        par = transform.parent.gameObject;
    }
    void Start()
    {
        hp_default = hp.Hp;
        
        canvas = GameObject.FindGameObjectWithTag("Canvas_World").GetComponent<Canvas>();
        gameObject.transform.SetParent(canvas.transform);

        offset = 0.3f * hpbar.rectTransform.localScale.x;

        //hpbar.rectTransform.localScale = Vector3.one;
    }
    void Update()
    {
        if (par.activeSelf)
        {
            hpbar.rectTransform.position = new Vector3(par.transform.position.x, par.transform.position.y - offset, 0);
            //hpbar.rectTransform.position = Camera.main.WorldToScreenPoint(new Vector3(par.transform.position.x, par.transform.position.y + 0.5f, 0));
            hpbar.fillAmount = (float)hp.Hp / hp_default;
        }
        else
            gameObject.SetActive(false);

    }
}
