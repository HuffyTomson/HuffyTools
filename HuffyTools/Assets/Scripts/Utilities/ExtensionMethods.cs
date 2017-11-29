using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


// delegate and callbacks //
public delegate int ActionInt();
public delegate int ActionInt<T1>(T1 t1);
public delegate int ActionInt<T1, T2>(T1 t1, T2 t2);
public delegate int ActionInt<T1, T2, T3>(T1 t1, T2 t2, T3 t3);

public static class ExtensionMethods
{
    // float //
    public static float Remap(this float _f, float _fromMin, float _fromMax, float _toMin, float _toMax)
    {
        return (((_toMax - _toMin) * (_f - _fromMin)) / (_fromMax - _fromMin)) + _toMin;
    }

    public static float RemapSimple(this float _f, float _fromMin, float _fromMax)
    {
        return (_f - _fromMin) / (_fromMax - _fromMin);
    }

    public static bool About(this float _f, float _value)
    {
        if (_f + 0.001f > _value && _f - 0.001f < _value)
            return true;
        else
            return false;
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

    // Vector3 // 
    public static Vector3 SetX(this Vector3 _vec, float _value)
    {
        return new Vector3(_value, _vec.y, _vec.z);
    }
    public static Vector3 SetY(this Vector3 _vec, float _value)
    {
        return new Vector3(_vec.x, _value, _vec.z);
    }
    public static Vector3 SetZ(this Vector3 _vec, float _value)
    {
        return new Vector3(_vec.x, _vec.y, _value);
    }

    // vector 2 // 
    public static bool About(this Vector2 _vec, Vector2 _value)
    {
        float mag = _vec.magnitude;
        float mag2 = _value.magnitude;

        if (mag.About(mag2))
            return true;
        else
            return false;
    }

    // color //
    public static Color HSBLerp(this Color _from, Color _to, float _f)
    {
        return (HSBColor.Lerp(HSBColor.FromColor(_from), HSBColor.FromColor(_to), _f)).ToColor();
    }
    public static Color SetR(this Color _color, float _value)
    {
        return new Color(_color.r, _color.g, _color.b, _color.a);
    }
    public static Color SetG(this Color _color, float _value)
    {
        return new Color(_color.r, _value, _color.b, _color.a);
    }
    public static Color SetB(this Color _color, float _value)
    {
        return new Color(_color.r, _color.g, _value, _color.a);
    }
    public static Color SetA(this Color _color, float _value)
    {
        return new Color(_color.r, _color.g, _color.b, _value);
    }
	
    // animation //
    public static IEnumerator Play(this Animation animation, string clipName, bool useTimeScale, System.Action onComplete)
    {
        //We Don't want to use timeScale, so we have to animate by frame..
        if (!useTimeScale)
        {
            AnimationState _currState = animation[clipName];
            bool isPlaying = true;
            float _startTime = 0F;
            float _progressTime = 0F;
            float _timeAtLastFrame = 0F;
            float _timeAtCurrentFrame = 0F;
            float deltaTime = 0F;
            
            animation.Play(clipName);

            _timeAtLastFrame = Time.realtimeSinceStartup;
            while (isPlaying)
            {
                _timeAtCurrentFrame = Time.realtimeSinceStartup;
                deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                _timeAtLastFrame = _timeAtCurrentFrame;

                _progressTime += deltaTime;
                _currState.normalizedTime = _progressTime / _currState.length;
                animation.Sample();

                //Debug.Log(_progressTime);

                if (_progressTime >= _currState.length)
                {
                    //Debug.Log(&quot;Bam! Done animating&quot;);
                    if (_currState.wrapMode != WrapMode.Loop)
                    {
                        //Debug.Log(&quot;Animation is not a loop anim, kill it.&quot;);
                        //_currState.enabled = false;
                        isPlaying = false;
                    }
                    else
                    {
                        //Debug.Log(&quot;Loop anim, continue.&quot;);
                        _progressTime = 0.0f;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
            yield return null;
            if (onComplete != null)
            {
                onComplete();
            }
        }
        else
        {
            animation.Play(clipName);
        }
    }
}
