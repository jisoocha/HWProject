using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

	public static float GetNorm2(Vector2 vector)
    {
        return Mathf.Sqrt (Mathf.Pow (vector.x, 2) + Mathf.Pow (vector.y, 2));
    }


}
