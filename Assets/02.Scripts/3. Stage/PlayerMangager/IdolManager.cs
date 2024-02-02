using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolManager : PlayerManager
{
    [SerializeField] protected Transform spawnPosition;
    public override void SpawnPlayer()
    {
        //temp code
        transform.position = spawnPosition.position;
        transform.rotation = spawnPosition.rotation;
    }
}
