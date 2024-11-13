using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

    // Event
    public Action MonsterSpawn;


	
    [field: Header("Player")]
    [field: SerializeField] public GameObject player;

    [field: Header("Monster")]
    [field: SerializeField] public GameObject grayZombie;
    [field: SerializeField] public GameObject redZombie;
    [field: SerializeField] public GameObject venom;
    [field: SerializeField] public GameObject creep;

    private List<GameObject> spawnedMonsters = new List<GameObject>();
    public List<GameObject> SpawnedMonsters {

        get {
            bool existNull = true;
            while (spawnedMonsters.Count != 0 && existNull) {
                existNull = false;
                foreach (GameObject spawnMonster in spawnedMonsters) {
                    if (spawnMonster == null) {
                        spawnedMonsters.Remove(spawnMonster);
                        existNull = true;
                        break;
                    }
                }
            }

            return spawnedMonsters;
        }

        set {
            spawnedMonsters = value;
        }

    }

    public Transform[] spawnTransform { get; set; }



    private void Start() {
        int childCount = transform.childCount;
        spawnTransform = new Transform[childCount];
        for (int i = 0; i < childCount; i++) {
            spawnTransform[i] = transform.GetChild(i);
        }
    }

    public void SpawnMonster(int spawnerNumber, int monsterNumber, int count = 1) {
        // 소환될 몬스터 종류 지정
        GameObject monster;
        switch (monsterNumber) {
            case 0:
                monster = grayZombie;
                break;
            case 1:
                monster = redZombie;
                break;
            case 2:
                monster = venom;
                break;
            case 3:
                monster = creep;
                break;
            default:
                monster = grayZombie;
                break;
        }

        for (int i = 0; i < count; i++) {
            GameObject spawnedMonster = Instantiate(monster, spawnTransform[spawnerNumber].position, spawnTransform[spawnerNumber].rotation); // 소환될 몬스터 위치 지정
            spawnedMonster.GetComponent<IMonster>().Target = player; // 소환된 몬스터의 타겟 지정

            SpawnedMonsters.Add(spawnedMonster); // 소환된 몬스터 목록에 추가
        }

        MonsterSpawn?.Invoke();
    }
}
