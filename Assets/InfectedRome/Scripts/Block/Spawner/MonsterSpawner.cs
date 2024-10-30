using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
	
    [field: Header("Object")]
    [field: SerializeField] public GameObject player;
    [field: SerializeField] public GameObject[] monsters;



    public Transform[] spawnTransform { get; private set; }





    private void Start() {
        int childCount = transform.childCount;
        spawnTransform = new Transform[childCount];
        for (int i = 0; i < childCount; i++) {
            spawnTransform[i] = transform.GetChild(i);
        }



        StartCoroutine(CRunSpawner());
    }





    private IEnumerator CRunSpawner() {
        ILivingEntity playerLivingEntity = player.GetComponent<ILivingEntity>();
        while (!playerLivingEntity.IsDead) {
            for (int i = 0; i < 1; i++) {
                SpawnMonster();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }



    public void SpawnMonster() {
        SpawnMonster(Random.Range(0, spawnTransform.Length));
    }
    
    public void SpawnMonster(int spawnerNumber) {
        GameObject monster = Instantiate(monsters[Random.Range(0, monsters.Length)], spawnTransform[spawnerNumber].position, spawnTransform[spawnerNumber].rotation);
        ILivingEntity zombieLivingEntity = monster.GetComponent<ILivingEntity>();
        monster.GetComponent<IMonster>().Target = GameObject.Find("Player");
    }
}
