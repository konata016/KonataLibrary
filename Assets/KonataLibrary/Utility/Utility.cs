using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

public class Utility
{
    public enum Mode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    public static void SetBlendMode(Material material, Mode blendMode)
    {
        material.SetFloat("_Mode", (float)blendMode);
        material.SetOverrideTag("RenderType", $"{blendMode}");

        switch (blendMode)
        {
            case Mode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                break;
            case Mode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                break;
            case Mode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                break;
            case Mode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                break;
        }
        
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
    }

    /// <summary>
    /// [,] 区切りの文字列を int の配列にする
    /// </summary>
    public static int[] ConvertStringToIntArray(string str)
    {
        var arr = str.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var iArr = new int[arr.Length];
        for (var i = 0; i < iArr.Length; i++)
        {
            var id = 0;
            if (int.TryParse(arr[i], out int ii))
            {
                id = ii;
            }
            else
            {
                Debug.LogError($"想定外の文字列：{arr[i]}");
            }

            iArr[i] = id;
        }

        return iArr;
    }

    /// <summary>
    /// 円状に並べる設定
    /// </summary>
    public static void SetupCirclePosition(
        Action<Vector3, int> onSettingPosition,
        int count,
        float radius,
        float angle)
    {
        for (int i = 0; i < count; i++)
        {
            var r = (angle / count) * i;
            r *= Mathf.Deg2Rad;

            var x = radius * Mathf.Cos(r);
            var y = 0;
            var z = radius * Mathf.Sin(r);

            onSettingPosition?.Invoke(new Vector3(x, y, z), i);
        }
    }
    
    /// <summary>
    /// 確率抽選に当選したか
    /// </summary>
    public static bool IsWinningProbabilityDrawing(float probability)
    {
        var ranNum = Random.value * 100f;
        
        if(probability >= 100f)
        {
            return true;
        }

        if (probability <= 0f)
        {
            return false;
        }
        
        return ranNum < probability;
    }
    
    /// <summary>
    /// 文字列をenumに変換する
    /// </summary>
    public static T TryConversionStringToEnum<T>(string text) where T : struct, Enum
    {
        if (!Enum.TryParse(text, out T word) || !Enum.IsDefined(typeof(T), word))
        {
            Debug.LogError($"想定外の文字列：{text}");
            return default;
        }

        return word;
    }

    /// <summary>
    /// ピラミッド状に並べる座標を取得する
    /// </summary>
    /// <param name="range">範囲</param>
    public static Vector3[] GetPyramidPositionArr(Vector2Int range)
    {
        var posList = new List<Vector3>();
        var index = 0;
        var cacheCount = 0;
        var countX = 0;
        var countZ = 0;

        for (var y = 0; y < Mathf.Max(range.x, range.y); y++)
        {
            for (var x = 0; x < range.x - countX; x++)
            {
                for (var z = 0; z < range.y - countZ; z++)
                {
                    var posX = range.x == 1 ? 0 : -((range.x - (y + 1)) / 2f) + x;
                    var posZ = range.y == 1 ? 0 : -((range.y - (y + 1)) / 2f) + z;
                    posList.Add(new Vector3(posX, y, posZ));
                    index++;

                    countZ = range.y == 1 ? 0 : y;
                }

                countX = range.x == 1 ? 0 : y;
            }

            if (cacheCount == index) break;
            cacheCount = index;
        }

        return posList.ToArray();
    }
    
    public static string GetTimeText(float time, string timeTextZero = "00:00:00")
    {
        if (time <= 0)
        {
            return timeTextZero;
        }

        var timeSpan = TimeSpan.FromSeconds(time);
        var text = timeSpan.TotalHours <= 24
            ? $"{timeSpan:hh\\:mm\\:ss}"
            : $"{timeSpan:dd\\dhh\\hmm\\m}";

        return text;
    }
}
