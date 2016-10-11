using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.utils
{
    public enum InterpolationType
    {
        Linear,
        EaseOut,
        EaseIn,
        Exponential,
        Smoothstep,
        Smootherstep
    }

    public class Interpolations
    {
        
        public static float EaseIn(float value)
        {
            return Mathf.Cos(value * Mathf.PI * 0.5f);
        }

        public static float Easeout(float value)
        {
            return Mathf.Sin(value * Mathf.PI * 0.5f);
        }

        public static float Exponential(float value, float power = 2f)
        {
            return Mathf.Pow(value, power);
        }

        public static float Smoothstep(float value)
        {
            return Mathf.Pow(value, 2f) * (3f - (2f * value)); ;
        }

        public static float Smootherstep(float value)
        {
            return Mathf.Pow(value, 3f) * (value * (6f * value - 15f) + 10f);
        }

        public static float Interpolation(float value, InterpolationType interpolation = InterpolationType.Linear)
        {
            switch(interpolation)
            {
                case InterpolationType.EaseIn:
                    return EaseIn(value);
                case InterpolationType.EaseOut:
                    return Easeout(value);
                case InterpolationType.Exponential:
                    return Exponential(value);
                case InterpolationType.Smoothstep:
                    return Smoothstep(value);
                case InterpolationType.Smootherstep:
                    return Smootherstep(value);
                case InterpolationType.Linear:
                default:
                    return value; 
            }
        }
    }
}
