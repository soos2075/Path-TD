using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Thunder : Bullet_Base
{



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_HitCheck)
        {
            return;
        }

        if (_Target == collision.gameObject)
        {
            collision.GetComponent<EnemyMove>().Subtract_SP(_damage);

            if (gameObject.GetComponent<StayParticle>() != null)
            {
                gameObject.GetComponent<StayParticle>().Off_Col();
            }
            else if (gameObject.GetComponent<Poolable>() != null)
            {
                GameManager.m_Resource.Disable_Prefab(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(_effect))
            {
                GameManager.m_Resource.Instant_Effect(_effect, transform.position);
                SoundManager.Instance.PlaySound($"Effect/Explosion/{_effect}");
            }

            _HitCheck = true;
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{

    //}
}
