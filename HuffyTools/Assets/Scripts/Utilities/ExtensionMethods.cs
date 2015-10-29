using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods
{
    // float //
    public static float FromTo(this float _f, float _fromMin, float _fromMax, float _toMin, float _toMax)
    {
        float returnFloat = (((_toMax - _toMin) * (_f - _fromMin)) / (_fromMax - _fromMin)) + _toMin;
        return returnFloat;
    }
	
	// <T>list //
    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
	
    // UI //
    public static void SetTogglesInteractable(this List<Toggle> _toggles, bool _value)
    {
        foreach (Toggle t in _toggles)
        {
            t.interactable = _value;
        }
    }
    public static void SetTogglesOn(this List<Toggle> _toggles, bool _value)
    {
        foreach (Toggle t in _toggles)
        {
            t.isOn = _value;
        }
    }
}
