using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour {
	
    [field: Header("Object")]
    [field: SerializeField] public GameObject player;
    [field: SerializeField] public GameObject[] zombies;



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
                SpawnZombie();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }



    public void SpawnZombie() {
        SpawnZombie(Random.Range(0, spawnTransform.Length));
    }
    
    public void SpawnZombie(int spawnerNumber, float maxHealth = 50.0f, float moveSpeed = 3.5f) {
        GameObject zombie = Instantiate(zombies[Random.Range(0, zombies.Length)], spawnTransform[spawnerNumber].position, spawnTransform[spawnerNumber].rotation);
        ILivingEntity zombieLivingEntity = zombie.GetComponent<ILivingEntity>();
        zombie.GetComponent<Zombie>().target = GameObject.Find("Player");
        zombieLivingEntity.MaxHealth = maxHealth;
        zombieLivingEntity.CurrentHealth = maxHealth;
        zombieLivingEntity.MoveSpeed = moveSpeed;
    }
}
