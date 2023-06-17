using UnityEngine;

public static class Helpers
{
    public static int LayerToInt(this LayerMask mask)
    {
        return Mathf.RoundToInt(Mathf.Log(mask.value, 2));
    }
    
}
