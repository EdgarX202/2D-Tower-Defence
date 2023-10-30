using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject basicEnemy;

    public float timeBetweenWaves;
    public float timeBeforeRoundStart;
    public float time;

    public bool isRoundGoing;
    public bool isIntermission;
    public bool isStarOfRound;

    public int round;

    private void Start()
    {
        isRoundGoing = false;
        isIntermission = false;
        isStarOfRound = true;

        time = Time.time + timeBeforeRoundStart;

        round = 1;
    }

    private void SpawnEnemies()
    {
        StartCoroutine("ISpawnEnemies");
    }

    IEnumerator ISpawnEnemies()
    {
        for (int i = 0; i < round; i++)
        {
            GameObject newEnemy = Instantiate(basicEnemy, MapGenerator.startTile.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update()
    {
        // State machine
        if(isStarOfRound)
        {
            if(Time.time >= time)
            {
                isStarOfRound = false;
                isRoundGoing = true;

                SpawnEnemies();
                return;
            }
        }
        else if (isIntermission)
        {
            if(Time.time >= time)
            {
                isIntermission = false;
                isRoundGoing = true;

                SpawnEnemies();
                return;
            }
        }
        else if (isRoundGoing)
        {
            if(Enemies.enemies.Count > 0)
            {
                
            }
            else
            {
                isIntermission = true;
                isRoundGoing = false;

                time = Time.time + timeBetweenWaves;
                round++;
                return;
            }
        }
    }
}
