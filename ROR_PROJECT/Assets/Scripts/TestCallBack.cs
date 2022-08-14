using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCallBack : MonoBehaviour
{
    private Transform shooter;
    private Transform enemy;

    public void SetShooter(Transform player)
    {
        shooter = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            enemy = other.transform;
            shooter.BroadcastMessage("damageDealt", enemy);
        }
    }
}
