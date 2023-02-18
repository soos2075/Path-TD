using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpining : MonoBehaviour
{
    public Vector3 rotateVector = Vector3.zero;

    public enum spaceEnum { Local, World };
    public spaceEnum rotateSpace;


    public ParticleSystem respawn;



    void Awake()
    {
        respawn = transform.GetChild(0).GetComponent<ParticleSystem>();
        StopRespawn();
    }

    private void Start()
    {
        StageManager.Instance.Event_WaveStart += PlayRespawn;
        StageManager.Instance.Event_WaveOver += StopRespawn;
    }



    void PlayRespawn()
    {
        respawn.Play();
    }
    void StopRespawn()
    {
        respawn.Stop();
    }



    void Update()
    {
        if (rotateSpace == spaceEnum.Local)
            transform.Rotate(rotateVector * Time.unscaledDeltaTime);
        if (rotateSpace == spaceEnum.World)
            transform.Rotate(rotateVector * Time.unscaledDeltaTime, Space.World);
    }
}
