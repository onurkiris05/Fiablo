using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class Helpers
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();

    public static WaitForSeconds BetterWaitForSeconds(float seconds)
    {
        // Check if WaitForSeconds instance for the specified seconds exists in the dictionary
        if (!WaitDictionary.TryGetValue(seconds, out var wait))
        {
            wait = new WaitForSeconds(seconds);
            WaitDictionary.Add(seconds, wait);
        }

        return wait;
    }

    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }

    public static void SetChildrenActiveState(this Transform t, bool state)
    {
        foreach (Transform child in t) child.gameObject.SetActive(state);
    }

    public static int LayerToInt(this LayerMask mask)
    {
        return Mathf.RoundToInt(Mathf.Log(mask.value, 2));
    }

    public static int Sqr(this int value)
    {
        return value * value;
    }

    public static float Sqr(this float value)
    {
        return value * value;
    }

    public static JToken ToToken(this Vector3 vector)
    {
        JObject state = new JObject();
        IDictionary<string, JToken> stateDict = state;
        stateDict["x"] = vector.x;
        stateDict["y"] = vector.y;
        stateDict["z"] = vector.z;
        return state;
    }

    public static Vector3 ToVector3(this JToken state)
    {
        Vector3 vector = new Vector3();
        if (state is JObject jObject)
        {
            IDictionary<string, JToken> stateDict = jObject;

            if (stateDict.TryGetValue("x", out JToken x))
            {
                vector.x = x.ToObject<float>();
            }

            if (stateDict.TryGetValue("y", out JToken y))
            {
                vector.y = y.ToObject<float>();
            }

            if (stateDict.TryGetValue("z", out JToken z))
            {
                vector.z = z.ToObject<float>();
            }
        }

        return vector;
    }
}