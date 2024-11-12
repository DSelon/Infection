using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSystem : MonoBehaviour {

    public MonsterSpawner monsterSpawner;
    public GameObject waveText;
	
    private int wave;
    public int Wave {

        get {
            return wave;
        }

        set {
            wave = value;
        }

    }



    private void Start() {
        StartCoroutine(CStart());

        wave = 1;
    }

    private IEnumerator CStart() {
        yield return new WaitForSeconds(3);

        StartCoroutine(CRunWave(wave));
    }



    private IEnumerator CRunWave(int waveNumber) {
        StartCoroutine(CPrintWaveText());

        int spawnerCount = monsterSpawner.spawnTransform.Length;
        switch (waveNumber) {
            case 1:
                StartCoroutine(CSpawnMonster(0, 0, 0));

                yield return new WaitForSeconds(1);
                break;
            case 2:
                StartCoroutine(CSpawnMonster(0, 0, 0));

                yield return new WaitForSeconds(1);
                break;
            case 3:
                StartCoroutine(CSpawnMonster(0, 0, 0));

                yield return new WaitForSeconds(1);
                break;
            case 4:
                StartCoroutine(CSpawnMonster(0, 0, 0));

                yield return new WaitForSeconds(1);
                break;
            case 5:
                StartCoroutine(CSpawnMonster(0, 0, 0));

                for (int i = 0; i < 30; i++) {
                    StartCoroutine(CSpawnMonster(0, Random.Range(0, spawnerCount), 0));
                }

                StartCoroutine(CSpawnMonster(0, Random.Range(0, spawnerCount), 3));

                yield return new WaitForSeconds(10);
                break;
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(CCheck());

        yield return null;
    }

    private IEnumerator CCheck() {
        while (true) {
            if (monsterSpawner.SpawnedMonsters.Count == 0) {
                wave++;
                StartCoroutine(CRunWave(wave));

                break;
            }

            yield return null;
        }
    }

    private IEnumerator CPrintWaveText() {
        waveText.SetActive(true);
        waveText.GetComponent<Text>().text = "Wave " + wave;
        yield return new WaitForSeconds(3);
        waveText.SetActive(false);
    }

    private IEnumerator CSpawnMonster(float time, int spawnerNumber, int monsterNumber) {
        yield return new WaitForSeconds(time);
        monsterSpawner.SpawnMonster(spawnerNumber, monsterNumber);
    }

}
