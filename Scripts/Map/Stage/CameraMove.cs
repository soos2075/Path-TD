using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Camera myCam;

    public float speed;

    [SerializeField] Vector3 startPos;
    [SerializeField] bool push;


    const float Offset = 0.5f;
    const float MaxCameraSize = 5.5f;

    float sizeX;
    float sizeY;

    public float screen_ratio;

    Vector3 cameraPos;

    float timer;

    bool overSize = false;

    float dis;

    float size;
    float size_Compensation;

    void Start()
    {
        myCam = Camera.main;

        size = PersonalCamaraSize();


        if (size > MaxCameraSize)
        {
            size = MaxCameraSize;
        }

        if (size * screen_ratio * 2 < sizeX || size * 2 < sizeY)
        {
            overSize = true;
        }



        float startPosX = (size * screen_ratio) - Offset;
        float startPosY = size - Offset;
        cameraPos = new Vector3(startPosX, startPosY, -10);

        myCam.orthographicSize = size;
        myCam.transform.position = cameraPos;
        size_Compensation = size;

        //? 맵크기만큼 구역 정해주기 (카메라 줌인 줌아웃때문에 추가함)
        var boundary = GameManager.m_Resource.Instant_Prefab("Tile/MapBoundary");
        boundary.GetComponent<SpriteRenderer>().size = new Vector2(sizeX, sizeY);
        boundary.transform.position = new Vector3((sizeX / 2) - Offset, (sizeY / 2) - Offset, 0); 
    }
    void Update()
    {
        size_Compensation = myCam.orthographicSize;
        if (size_Compensation * screen_ratio * 2 < sizeX || size_Compensation * 2 < sizeY)
        {
            overSize = true;
        }
        else
        {
            myCam.transform.position = cameraPos;
            overSize = false;
        }


        if (FindObjectOfType<UI_Tutorial>() != null)
        {
            return;
        }


#if UNITY_EDITOR
        if (overSize)
        {
            if (Input.GetMouseButton(0))         //? 드래그로 위치이동
            {
                timer += Time.unscaledDeltaTime;

                if (timer > 0.1f)
                {
                    if (!push)
                    {
                        startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        push = true;
                    }
                    //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                    //myCam.transform.position += (startPos - Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    Vector3 pos = (startPos - Camera.main.ScreenToWorldPoint(Input.mousePosition)) * speed;
                    //Debug.Log(pos);

                    //float moveX = Mathf.Clamp((myCam.transform.position.x + pos.x), cameraPos.x, (cameraPos.x + sizeX - (myCam.orthographicSize * (ratio * 2))));
                    //float moveY = Mathf.Clamp((myCam.transform.position.y + pos.y), (cameraPos.y + sizeY - (myCam.orthographicSize * 2)), cameraPos.y);
                    //myCam.transform.position = new Vector3(moveX, moveY, -10);

                    float mX = Mathf.Min((size_Compensation * screen_ratio) - Offset, (size_Compensation * screen_ratio) - Offset + (sizeX - (size_Compensation * (screen_ratio * 2))));
                    float MX = Mathf.Max((size_Compensation * screen_ratio) - Offset, (size_Compensation * screen_ratio) - Offset + (sizeX - (size_Compensation * (screen_ratio * 2))));

                    float mY = Mathf.Min(size_Compensation - Offset, size_Compensation - Offset + (sizeY - (myCam.orthographicSize * 2)));
                    float MY = Mathf.Max(size_Compensation - Offset, size_Compensation - Offset + (sizeY - (myCam.orthographicSize * 2)));

                    float moveX = Mathf.Clamp((myCam.transform.position.x + pos.x), mX, MX);
                    float moveY = Mathf.Clamp((myCam.transform.position.y + pos.y), mY, MY);
                    myCam.transform.position = new Vector3(moveX, moveY, -10);

                    Debug.Log(mX + "X이동범위" + MX);
                    Debug.Log(mY + "Y이동범위" + MY);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                push = false;
                timer = 0;
            }
        }
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel != 0)
        {
            float reSize = myCam.orthographicSize += (wheel * speed * Time.unscaledDeltaTime * -1000);

            size_Compensation = Mathf.Clamp(reSize, 3, MaxCameraSize);
            myCam.orthographicSize = size_Compensation;
        }
        return;
#endif

#if UNITY_ANDROID
        if (overSize)
        {
            if (Input.touchCount > 0)  //? 드래그로 위치이동
            {
                timer += Time.unscaledDeltaTime;

                if (timer > 0.1f)
                {
                    if (!push)
                    {
                        startPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        push = true;
                    }

                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        Vector3 pos = (startPos - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)) * speed;

                        float mX = Mathf.Min((size_Compensation * screen_ratio) - Offset, (size_Compensation * screen_ratio) - Offset + (sizeX - (size_Compensation * (screen_ratio * 2))));
                        float MX = Mathf.Max((size_Compensation * screen_ratio) - Offset, (size_Compensation * screen_ratio) - Offset + (sizeX - (size_Compensation * (screen_ratio * 2))));

                        float mY = Mathf.Min(size_Compensation - Offset, size_Compensation - Offset + (sizeY - (myCam.orthographicSize * 2)));
                        float MY = Mathf.Max(size_Compensation - Offset, size_Compensation - Offset + (sizeY - (myCam.orthographicSize * 2)));

                        float moveX = Mathf.Clamp((myCam.transform.position.x + pos.x), mX, MX);
                        float moveY = Mathf.Clamp((myCam.transform.position.y + pos.y), mY, MY);
                        myCam.transform.position = new Vector3(moveX, moveY, -10);
                    }
                }
            }

            if (Input.touchCount == 0)
            {
                push = false;
                timer = 0;
            }
        }

        if (Input.touchCount >= 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                dis = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }

            if (Input.GetTouch(1).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                float change_dis = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

                float reSize = myCam.orthographicSize += ((dis - change_dis) * speed * Time.unscaledDeltaTime * 0.1f);

                size_Compensation = Mathf.Clamp(reSize, 3, MaxCameraSize);
                myCam.orthographicSize = size_Compensation;
            }
        }
#endif
    }




    float PersonalCamaraSize ()
    {
        float w = Screen.width;
        float h = Screen.height;
        screen_ratio = w / h;

        sizeX = GameManager.m_Map.nowMap.GetLength(0);
        sizeY = GameManager.m_Map.nowMap.GetLength(1);
        Debug.Log("Map Size = " + sizeX + ":" + sizeY);

        float size = 0;

        if (screen_ratio == 2) //? 18:9사이즈
        {
            size = (sizeX / sizeY) >= 2 ? sizeY / 2 : sizeX / 4;
        }
        else if (screen_ratio > 2) //? 18:9보다 x가 클경우
        {
            size = sizeY / 2;
        }
        else if (screen_ratio < 2) //? 18:9보다 x가 작을경우
        {
            size = sizeX / (screen_ratio * 2);
        }

        return size;
    }
}
