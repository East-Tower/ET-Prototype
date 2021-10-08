using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public PlayerManager playerManager;
    Collider damageCollider;

    int curDamage = 10;

    public int duration;

    private void Awake()
    {
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
        if (collision.tag == "Player") 
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            if (playerStats != null) 
            {
                playerStats.TakeDamage(curDamage);
            }
        }

        if (collision.tag == "Enemy") 
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if (enemyStats != null) 
            {
                enemyStats.TakeDamage(curDamage);
                HitPause(duration);
                playerManager.isHitting = true;
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
