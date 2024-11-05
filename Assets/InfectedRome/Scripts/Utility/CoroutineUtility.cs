using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtility : Singleton<CoroutineUtility> {

    // Static 기능
	private Dictionary<string, Coroutine> runningCoroutines = new Dictionary<string, Coroutine>();



    public static Coroutine StartStaticCoroutine(IEnumerator routine) {
        return Instance.StartStaticCoroutineInternal(routine);
    }

    public static Coroutine StartStaticCoroutine(string routineName, IEnumerator routine) {
        return Instance.StartStaticCoroutineInternal(routineName, routine);
    }

    public static void StopStaticCoroutine(Coroutine routine) {
        Instance.StopStaticCoroutineInternal(routine);
    }

    public static void StopStaticCoroutine(string routineName) {
        Instance.StopStaticCoroutineInternal(routineName);
    }

    public static void StopAllStaticCoroutines() {
        Instance.StopAllStaticCoroutinesInternal();
    }



    private Coroutine StartStaticCoroutineInternal(IEnumerator routine) {
        return base.StartCoroutine(routine);
    }

    private Coroutine StartStaticCoroutineInternal(string routineName, IEnumerator routine) {
        StopStaticCoroutineInternal(routineName);
        Coroutine coroutine = base.StartCoroutine(routine);
        runningCoroutines[routineName] = coroutine;
        return coroutine;
    }

    private void StopStaticCoroutineInternal(Coroutine routine) {
        base.StopCoroutine(routine);
    }

    private void StopStaticCoroutineInternal(string routineName) {
        if (runningCoroutines.TryGetValue(routineName, out Coroutine coroutine)) {
            base.StopCoroutine(coroutine);
            runningCoroutines.Remove(routineName);
        }
    }

    private void StopAllStaticCoroutinesInternal() {
        base.StopAllCoroutines();
        runningCoroutines.Clear();
    }
    //





    // 람다식 기능
    public static void CallWaitForOneFrame(Action action) {
        StartStaticCoroutine(DoCallWaitForOneFrame(action));
    }
        
    public static void CallWaitForSeconds(float seconds, Action action) {
        StartStaticCoroutine(DoCallWaitForSeconds(seconds, action));
    }

    public static void CallWaitForSecondsRealtime(float seconds, Action action) {
        StartStaticCoroutine(DoCallWaitForSecondsRealtime(seconds, action));
    }

    
    
    private static IEnumerator DoCallWaitForOneFrame(Action action) {
        yield return 0;
        action();
    }

    private static IEnumerator DoCallWaitForSeconds(float seconds, Action action) {
        yield return new WaitForSeconds(seconds);
        action();
    }

    private static IEnumerator DoCallWaitForSecondsRealtime(float seconds, Action action) {
        yield return new WaitForSecondsRealtime(seconds);
        action();
    }
}
