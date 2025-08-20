using System.Collections;
using UnityEngine;

public class BGMUtility : MonoBehaviour {

	// 볼륨 업
    public static IEnumerator CVolumeUp(AudioSource bgmSource, float volumeChangeSpeed = 2.0f) {
        float volume = PlayerPrefs.GetFloat("Option_BGMVolume");
        float differenceVolume = volume;

        bgmSource.volume = 0;

        float deltaVolume = 0;
        while (deltaVolume < volume) {
            deltaVolume += differenceVolume * volumeChangeSpeed * Time.unscaledDeltaTime;
            bgmSource.volume = deltaVolume;

            yield return null;
        }
        
        bgmSource.volume = volume;
    }

    // 볼륨 다운
    public static IEnumerator CVolumeDown(AudioSource bgmSource, float volumeChangeSpeed = 2.0f) {
        float volume = PlayerPrefs.GetFloat("Option_BGMVolume");
        float differenceVolume = volume;

        bgmSource.volume = volume;

        float deltaVolume = volume;
        while (deltaVolume > 0) {
            deltaVolume -= differenceVolume * volumeChangeSpeed * Time.unscaledDeltaTime;
            bgmSource.volume = deltaVolume;

            yield return null;
        }

        bgmSource.volume = 0;
    }

}
