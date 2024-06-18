using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    protected static AudioManager _AM { get { return AudioManager.INSTANCE; } }

    #region Coroutine Helpers

    /// <summary>
    /// Executes the Action block as a Coroutine on the next frame.
    /// </summary>
    /// <param name="func">The Action block</param>
    protected void ExecuteNextFrame(Action func)
    {
        StartCoroutine(ExecuteAfterFramesCoroutine(1, func));
    }
    /// <summary>
    /// Executes the Action block as a Coroutine after X frames.
    /// </summary>
    /// <param name="func">The Action block</param>
    protected void ExecuteAfterFrames(int frames, Action func)
    {
        StartCoroutine(ExecuteAfterFramesCoroutine(frames, func));
    }
    private IEnumerator ExecuteAfterFramesCoroutine(int frames, Action func)
    {
        for (int f = 0; f < frames; f++)
            yield return new WaitForEndOfFrame();
        func();
    }

    /// <summary>
    /// Executes the Action block as a Coroutine after X seconds
    /// </summary>
    /// <param name="seconds">Seconds.</param>
    protected void ExecuteAfterSeconds(float seconds, Action func)
    {
        if (seconds <= 0f)
            func();
        else
            StartCoroutine(ExecuteAfterSecondsCoroutine(seconds, func));
    }
    private IEnumerator ExecuteAfterSecondsCoroutine(float seconds, Action func)
    {
        yield return new WaitForSeconds(seconds);
        func();
    }

    /// <summary>
    /// Executes the Action block as a Coroutine whern a condition is met
    /// </summary>
    /// <param name="condition">The Condition block</param>
    /// <param name="func">The Action block</param>
    protected void ExecuteWhenTrue(Func<bool> condition, Action func)
    {
        StartCoroutine(ExecuteWhenTrueCoroutine(condition, func));
    }
    private IEnumerator ExecuteWhenTrueCoroutine(Func<bool> condition, Action func)
    {
        while (condition() == false)
            yield return new WaitForEndOfFrame();
        func();
    }

    #endregion

    /// <summary>
    /// Maps a value from one range to another
    /// </summary>
    /// <returns>The mapped value</returns>
    /// <param name="value">The input Value.</param>
    /// <param name="inMin">Input min</param>
    /// <param name="inMax">Input max</param>
    /// <param name="outMin">Output min</param>
    /// <param name="outMax">Output max</param>
    /// <param name="clamp">Clamp output value to outMin..outMax</param>
    public float Map(float value, float inMin, float inMax, float outMin, float outMax, bool clamp = true)
    {
        float f = ((value - inMin) / (inMax - inMin));
        float d = (outMin <= outMax ? (outMax - outMin) : -(outMin - outMax));
        float v = (outMin + d * f);
        if (clamp) v = ClampSmart(v, outMin, outMax);
        return v;
    }
    /// <summary>
    /// Maps a value from 0f..1f to another range
    /// </summary>
    /// <returns>The mapped value</returns>
    /// <param name="value">The input Value.</param>
    /// <param name="outMin">Output min</param>
    /// <param name="outMax">Output max</param>
    /// <param name="clamp">Clamp output value to outMin..outMax</param>
    public float MapFrom01(float value, float outMin, float outMax, bool clamp = true)
    {
        return Map(value, 0f, 1f, outMin, outMax, clamp);
    }
    /// <summary>
    /// Maps a value from a range to 0f..1f
    /// </summary>
    /// <returns>The mapped value</returns>
    /// <param name="value">The input Value.</param>
    /// <param name="inMin">Input min</param>
    /// <param name="inMax">Input max</param>
    /// <param name="clamp">Clamp output value to 0f..1f</param>
    public float MapTo01(float value, float inMin, float inMax, bool clamp = true)
    {
        return Map(value, inMin, inMax, 0f, 1f, clamp);
    }
    /// <summary>
    /// Clamps a value, swapping min/max if necessary
    /// </summary>
    /// <returns>The smart.</returns>
    /// <param name="value">The value to clamp</param>
    /// <param name="min">Min output value</param>
    /// <param name="max">Max output value</param>
    public float ClampSmart(float value, float min, float max)
    {
        if (min < max)
            return Mathf.Clamp(value, min, max);
        if (max < min)
            return Mathf.Clamp(value, max, min);
        return value;
    }
}

public class Singleton<T> : GameBehaviour where T : GameBehaviour
{
    public bool dontDestroy;
    private static T instance_;
    public static T INSTANCE
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = FindObjectOfType<T>();
                if (instance_ == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    singleton.AddComponent<T>();
                }
            }
            return instance_;
        }
    }
    protected virtual void Awake()
    {
        if (instance_ == null)
        {
            instance_ = this as T;
            if (dontDestroy) DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    //static T _instance;
    //public static T Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //            Debug.LogError("GameBehaviour<" + typeof(T).ToString() + "> not instantiated!\nNeed to call Instantiate() from " + typeof(T).ToString() + "Awake().");
    //        return _instance;
    //    }
    //}

    //// Instantiate singleton
    //protected bool Instantiate()
    //{
    //    if (_instance != null)
    //    {
    //        Debug.LogWarning("Instance of GameBehaviour<" + typeof(T).ToString() + "> already exists! Destroying myself.\nIf you see this when a scene is LOADED from another one, ignore it.");
    //        DestroyImmediate(gameObject);
    //        return false;
    //    }
    //    _instance = this as T;
    //    return true;
    //}
}
