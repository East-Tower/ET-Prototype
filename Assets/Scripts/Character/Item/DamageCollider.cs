using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    //public EnemyManager enemyManager;
    public PlayerManager playerManager;
    Collider damageCollider;

    int curDamage = 10;

    public int duration;

    private void Awake()
    {
        //enemyManager = GetComponentInParent<EnemyManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void EnableDamageCollider() 
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider() 
    {
        damageCollider.enabled = false;
        playerManager.isHitting = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Vector3 hitDirection = enemyManager.enemyRig.transform.position - collision.transform.position;
        //hitDirection.Normalize();

        if (collision.tag == "Player") 
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            if (playerStats != null) 
            {
                if (!playerStats.GetComponent<PlayerManager>().damageAvoid)
                {
                    playerStats.TakeDamage(curDamage, Vector3.zero, true);
                }
            }
        }

        if (collision.tag == "Enemy") 
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if (enemyStats != null) 
            {
                enemyStats.TakeDamage(curDamage, playerManager.GetComponent<PlayerStats>());  
                HitPause(duration);
                playerManager.isHitting = true;
            }
        }

        if (collision.tag == "DestructibleObject") 
        {
            DestructibleObject destructibleObject = collision.GetComponent<DestructibleObject>();

            if (destructibleObject != null) 
            {
                destructibleObject.ObjectDestroy();
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                playerManager.isHitting = false;
            }
        }
    }

    public void HitPause(int dur) 
    {
        StartCoroutine(Hitted(dur));
    }

    IEnumerator Hitted(int dur) 
    {
        float pauseTime = dur / 60f;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1f;
    }
}
