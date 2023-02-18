using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Poison : Bullet_Base
{

    //GameObject _Target;
    GameObject tower;

    public bool penetrate_Check = true; //? 관통 유무
    [SerializeField] float cycleTime = 0.5f;
    [SerializeField] int count = 5;

    AudioSource audioSource;

    public void Init_DOT (float cycle, int ct, int damage, GameObject tw)
    {
        cycleTime = cycle;
        count = ct;
        _damage = damage;
        tower = tw;
    }


    ParticleSystem[] par = new ParticleSystem[4];

    private void Awake()
    {
        par = transform.GetChild(0).GetComponentsInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = SoundManager.Instance.GetVolume(Define.AudioType.Effect) * 0.2f;
    }

    public override void Start()
    {
        base.Start();
    }
    void Update()
    {
        if (Time.timeScale == 0)
        {
            audioSource.Pause();
        }

        if (Time.timeScale != 0 && gameObject.activeSelf && GetComponent<CapsuleCollider2D>().enabled && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (tower == null || !tower.activeSelf)
        {
            gameObject.SetActive(false);
        }


        OnceAngle();

        if (_Target.activeSelf) //?
        {
            _Destination = _Target.transform.position - transform.position;
            Quaternion qa = Quaternion.Euler(0, 0, Mathf.Atan2(_Destination.x, _Destination.y) * -Mathf.Rad2Deg);
            transform.rotation = Quaternion.Slerp(transform.rotation, qa, Time.deltaTime * _speed_rote);
        }

        if (_Target == null)
        {
            gameObject.SetActive(false);
        }
    }

    Coroutine c_fadeOut;
    public void StopParticle ()
    {
        //audioSource.Stop();
        c_fadeOut = StartCoroutine(AudioFadeOut(1));
        GetComponent<CapsuleCollider2D>().enabled = false;
        for (int i = 0; i < par.Length; i++)
        {
            par[i].Stop();
        }
    }

    IEnumerator AudioFadeOut(float time)
    {
        float v = audioSource.volume;
        for (int i = 0; i < time * 5; i++)
        {
            audioSource.volume -= v / 5;
            yield return new WaitForSeconds(time / 5);
        }
        audioSource.Stop();
        audioSource.volume = SoundManager.Instance.GetVolume(Define.AudioType.Effect) * 0.2f;
    }

    public void PlayParticle(GameObject target)
    {
        if (c_fadeOut != null)
        {
            StopCoroutine(c_fadeOut);
        }
        audioSource.volume = SoundManager.Instance.GetVolume(Define.AudioType.Effect) * 0.2f;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        //audioSource.Play();
        GetComponent<CapsuleCollider2D>().enabled = true;
        _Target = target;
        for (int i = 0; i < par.Length; i++)
        {
            par[i].Play();
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        //EnemyMove em;
        //if (collision.TryGetComponent<EnemyMove>(out em))
        //{
        //    em.DOT(cycleTime, count, _damage);
        //    if (!penetrate_Check)
        //    {
        //        gameObject.SetActive(false);
        //    }
        //}

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        EnemyMove em;
        if (collision.TryGetComponent<EnemyMove>(out em))
        {
            if (em != null && em.move_State != EnemyMove.Move_State.Air)
            {
                em.DOT(cycleTime, count, _damage);
                if (!penetrate_Check)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
