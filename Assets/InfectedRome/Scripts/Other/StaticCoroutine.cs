using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCoroutine : Singleton<StaticCoroutine> {
	private Dictionary<string, Coroutine> runningCoroutines = new Dictionary<string, Coroutine>();





    public static Coroutine StartCoroutine(IEnumerator routine) {
        return Instance.StartCoroutineInternal(routine);
    }

    public static Coroutine StartCoroutine(string routineName, IEnumerator routine) {
        return Instance.StartCoroutineInternal(routineName, routine);
    }

    public static void StopCoroutine(Coroutine routine) {
        Instance.StopCoroutineInternal(routine);
    }

    public static void StopCoroutine(string routineName) {
        Instance.StopCoroutineInternal(routineName);
    }

    public static void StopAllCoroutines() {
        Instance.StopAllCoroutinesInternal();
    }



    private Coroutine StartCoroutineInternal(IEnumerator routine) {
        return base.StartCoroutine(routine);
    }

    private Coroutine StartCoroutineInternal(string routineName, IEnumerator routine) {
        StopCoroutineInternal(routineName);
        Coroutine coroutine = base.StartCoroutine(routine);
        runningCoroutines[routineName] = coroutine;
        return coroutine;
    }

    private void StopCoroutineInternal(Coroutine routine) {
        base.StopCoroutine(routine);
    }

    private void StopCoroutineInternal(string routineName) {
        if (runningCoroutines.TryGetValue(routineName, out Coroutine coroutine)) {
            base.StopCoroutine(coroutine);
            runningCoroutines.Remove(routineName);
        }
    }

    private void StopAllCoroutinesInternal() {
        base.StopAllCoroutines();
        runningCoroutines.Clear();
    }
}
