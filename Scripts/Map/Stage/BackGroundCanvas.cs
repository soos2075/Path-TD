using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundCanvas : MonoBehaviour
{

    public Texture stage_1_5;
    public Texture stage_6_10;
    public Texture stage_11_15;

    public Image MainBackground;


    private void Awake()
    {
        if (SelectStage.Instance.Stage < 6)
        {
            MainBackground.material.mainTexture = stage_1_5;
        }
        else if (SelectStage.Instance.Stage > 5 && SelectStage.Instance.Stage < 11)
        {
            MainBackground.material.mainTexture = stage_6_10;
        }
    }


    void Start()
    {
        MainBackground.material.mainTextureOffset = Vector2.zero;
    }


    public Vector2 offset_Main;


    void Update()
    {
        MainBackground.material.mainTextureOffset += offset_Main * 0.01f;
    }
}
