using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sp : MonoBehaviour
{
    [SerializeField] Image spbar;
    [SerializeField] EnemyMove host;
    [SerializeField] int sp_default;

    [SerializeField] GameObject par;
    [SerializeField] Canvas canvas;

    [SerializeField] float offset;

    private void Awake()
    {
        spbar = gameObject.GetComponent<Image>();
        host = GetComponentInParent<EnemyMove>();
        par = transform.parent.gameObject;
    }

    void Start()
    {
        if (host.Shield)
        {
            sp_default = host.SP;
            canvas = GameObject.FindGameObjectWithTag("Canvas_World").GetComponent<Canvas>();
            gameObject.transform.SetParent(canvas.transform);


            offset = 0.3f * spbar.rectTransform.localScale.x;
            //spbar.rectTransform.localScale = Vector3.one;
        }
    }
    void Update()
    {
        if (!host.Shield)
        {
            gameObject.SetActive(false);
            return;
        }

        if (par.activeSelf)
        {
            spbar.rectTransform.position = new Vector3(par.transform.position.x, par.transform.position.y - offset + (offset / 8), 0);
            //spbar.rectTransform.position = Camera.main.WorldToScreenPoint
            //    (new Vector3(par.transform.position.x, par.transform.position.y + 0.55f, 0));
            spbar.fillAmount = (float)host.SP / sp_default;
        }
        else
            gameObject.SetActive(false);
    }
}
