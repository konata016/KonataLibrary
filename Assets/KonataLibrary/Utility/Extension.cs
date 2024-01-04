using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public static class JsonUtilityExtension
{
    [Serializable]
    private struct JsonData<T>
    {
        [SerializeField]
        private T[] arr;
        public T[] Arr => arr;

        public JsonData(IEnumerable<T> data)
        {
            arr = data.ToArray();
        }
    }

    public static T[] ImportArr<T>(string dataPath, bool isStreamReader = false)
    {
        var asset = GetData(dataPath, isStreamReader);

        if (String.IsNullOrEmpty(asset))
        {
            Debug.LogError($"ファイルが存在しません！{dataPath}");
            return null;
        }

        var data = JsonUtility.FromJson<JsonData<T>>(asset);
        return data.Arr;
    }

    public static T Import<T>(string dataPath, bool isStreamReader = false)
    {
        var asset = GetData(dataPath, isStreamReader);

        if (String.IsNullOrEmpty(asset))
        {
            Debug.LogError($"ファイルが存在しません！{dataPath}");
            return default;
        }

        return JsonUtility.FromJson<T>(asset);
    }

    public static void ExportArr<T>(IEnumerable<T> data, string dataPath, bool prettyPrint = false)
    {
        var json = JsonUtility.ToJson(new JsonData<T>(data), prettyPrint);
        var writer = new StreamWriter(dataPath, false);
        Debug.Log(json);
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    public static void Export<T>(T data, string dataPath, bool prettyPrint = false)
    {
        var json = JsonUtility.ToJson(data, prettyPrint);
        var writer = new StreamWriter(dataPath, false);
        writer.Write(json);
        Debug.Log(json);
        writer.Flush();
        writer.Close();
    }

    private static string GetData(string dataPath, bool isStreamReader = false)
    {
        if (isStreamReader)
        {
            return new StreamReader(dataPath).ReadToEnd();
        }

        var path = ImportSaveLocationPath(dataPath);
        return $"{Resources.Load<TextAsset>(path)}";
    }

    private static string ImportSaveLocationPath(string dataPath)
    {
        const string Key = "Resources/";
        const string Extension = ".json";

        var adjustedPath = dataPath.Substring(dataPath.IndexOf(Key) + Key.Length);
        return adjustedPath.Remove(adjustedPath.IndexOf(Extension));
    }
}

public static class TransformExtension
{
    public static void AccessAllChildComponent<T>(
        this Transform transform,
        Action<T> onAccessedComponent)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var component = child.GetComponent<T>();
            onAccessedComponent?.Invoke(component);
        }
    }
}

public static class ButtonExtension
{
    /// <summary>
    /// ボタン押下時処理
    /// (Action登録時にRemoveAllListenersが呼ばれた後に登録される)
    /// </summary>
    public static void OnClickButton(this Button button, Action onClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}

public static class ToggleExtension
{
    /// <summary>
    /// チェック変更時処理
    /// (Action登録時にRemoveAllListenersが呼ばれた後に登録される)
    /// </summary>
    public static void OnValueChanged(
        this Toggle toggle,
        Action<bool> onValueChanged)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((isOn) => onValueChanged?.Invoke(isOn));
    }

    /// <summary>
    /// チェック時処理
    /// (Action登録時にRemoveAllListenersが呼ばれた後に登録される)
    /// </summary>
    public static void OnSelected(
        this Toggle toggle,
        Action onSelected)
    {
        toggle.OnValueChanged((isOn) =>
        {
            if (isOn)
            {
                onSelected?.Invoke();
            }
        });
    }
}

public static class InputFieldExtension
{
    /// <summary>
    /// 編集終了時処理
    /// (Action登録時にRemoveAllListenersが呼ばれた後に登録される)
    /// </summary>
    public static void OnEndEdit(this InputField inputField, Action<string> onEndEdit)
    {
        inputField.onEndEdit.RemoveAllListeners();
        inputField.onEndEdit.AddListener(call => onEndEdit?.Invoke(call));
    }
    
    /// <summary>
    /// 値変更時処理
    /// (Action登録時にRemoveAllListenersが呼ばれた後に登録される)
    /// </summary>
    public static void OnValueChanged(this InputField inputField, Action<string> onValueChanged)
    {
        inputField.onValueChanged.RemoveAllListeners();
        inputField.onValueChanged.AddListener(call => onValueChanged?.Invoke(call));
    }
    
    /// <summary>
    /// 編集終了時処理
    /// (Action登録時にRemoveAllListenersが呼ばれた後に登録される)
    /// </summary>
    public static void OnEndEdit(this TMP_InputField inputField, Action<string> onEndEdit)
    {
        inputField.onEndEdit.RemoveAllListeners();
        inputField.onEndEdit.AddListener(call => onEndEdit?.Invoke(call));
    }
}

public static class LineRendererExtension
{
    /// <summary>
    /// ライン内の頂点位置の配列を取得する
    /// </summary>
    public static Vector3[] GetPositionArr(this LineRenderer lineRenderer)
    {
        var positionArr = new Vector3[lineRenderer.positionCount];
        for (var i = 0; i < positionArr.Length; i++)
        {
            positionArr[i] = lineRenderer.GetPosition(i);
        }

        return positionArr;
    }
}

public static class FloatExtensions
{
    public static string ToFloatString(this float num)
    {
        if (num >= 1000000000000000000000f)
        {
            var value = num / 1000000000000000000000D;
            return $"{value:0.##}ac";
        }
        
        // 10^18
        if (num >= 1000000000000000000f)
        {
            var value = num / 1000000000000000000D;
            return $"{value:0.##}ab";
        }
        
        // 10^15
        if(num >= 1000000000000000f)
        {
            var value = num / 1000000000000000D;
            return $"{value:0.##}aa";
        }

        // 10^12
        if (num >= 1000000000000f)
        {
            var value = num / 1000000000000D;
            return $"{value:0.##}T";
        }

        // 10^9
        if (num >= 1000000000f)
        {
            var value = num / 1000000000D;
            return $"{value:0.##}B";
        }
        
        // 10^6
        if (num >= 1000000f)
        {
            var value = num / 1000000D;
            return $"{value:0.##}M";
        }

        // 10^3
        if (num >= 1000f)
        {
            var value = num / 1000D;
            return $"{value:0.##}K";
        }

        return $"{num}";
    }
}

public static class LongExtensions
{
    public static string ToUnitString(this long num)
    {
        // memo:
        //     1 ac（10^21）はlongでは実現できない
        //     acの実装を行う場合はdecimal型を使用することが考えられる
        
        // 10^18
        if (num >= 1000000000000000000)
        {
            var value = num / 1000000000000000000D;
            return $"{value:0.##}ab";
        }
        
        // 10^15
        if(num >= 1000000000000000)
        {
            var value = num / 1000000000000000D;
            return $"{value:0.##}aa";
        }

        // 10^12
        if (num >= 1000000000000)
        {
            var value = num / 1000000000000D;
            return $"{value:0.##}T";
        }

        // 10^9
        if (num >= 1000000000)
        {
            var value = num / 1000000000D;
            return $"{value:0.##}B";
        }
        
        // 10^6
        if (num >= 1000000)
        {
            var value = num / 1000000D;
            return $"{value:0.##}M";
        }

        // 10^3
        if (num >= 1000)
        {
            var value = num / 1000D;
            return $"{value:0.##}K";
        }

        return $"{num}";
    }
}

public static class ColorExtension
{
    public static Color ConvertColorCodeToColor(string colorCode)
    {
        if (ColorUtility.TryParseHtmlString(colorCode, out var c))
        {
            return c;
        }

        Debug.Log("カラーコードからColorの変換に失敗しました");
        return Color.white;
    }
    
    public static Color ToHDRColor(Color ldrColor, float intensity)
    {
        var factor = Mathf.Pow(2, intensity);
        return new Color(
            ldrColor.r * factor,
            ldrColor.g * factor,
            ldrColor.b * factor,
            ldrColor.a
        );
    }
}

public static class ImageExtension
{
    public static void SetAlpha(this Image image, float alpha)
    {
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }
}

public static class TweenExtension
{
    public static IObservable<Tween> OnCompleteAsObservable(this Tween tweener)
    {
        return Observable.Create<Tween>(o =>
        {
            tweener.OnComplete(() =>
            {
                o.OnNext(tweener);
                o.OnCompleted();
            });
            return Disposable.Create(() =>
            {
                tweener.Kill();
            });
        });
    }
}

public static class SliderExtension
{
    public static void Setup(this Slider slider, float value, Range<float> range)
    {
        slider.minValue = range.Min;
        slider.maxValue = range.Max;
        slider.value =  Mathf.Clamp(value, range.Min, range.Max);
    }
}

public static class LinqExtension
{
    public static T FirstOrDefault<T>(this T[] source, Func<T, bool> predicate)
    {
        if (source == null)
        {
            return default(T);
        }
        
        for (var i = 0; i < source.Length; i++)
        {
            var data = source[i];
            var isMatch = predicate(data);
            if (isMatch)
            {
                return data;
            }
        }
        
        return default(T);
    }
}

public static class StringExtension
{
    /// <summary>
    /// {$}を置換する
    /// </summary>
    public static string ReplacePlaceholder(this string input, string replacement)
    {
        return ReplacePlaceholder(input, "{$}", replacement);
    }
    
    /// <summary>
    /// 特定の文字列を置換する
    /// </summary>
    /// <param name="placeholder">置換対象</param>
    /// <param name="replacement">置換後の文字列</param>
    public static string ReplacePlaceholder(this string input, string placeholder, string replacement)
    {
        var pattern = Regex.Escape(placeholder);
        var result = Regex.Replace(input, pattern, replacement);
        return result;
    }
}

/// <summary>
/// テーブルの管理クラス
/// </summary>
[Serializable]
public class TableBase<TKey, TValue, Type> where Type : KeyAndValue<TKey, TValue>
{
    [SerializeField] private List<Type> list;
    private Dictionary<TKey, TValue> table;

    public Dictionary<TKey, TValue> GetTable()
    {
        if (table == null)
        {
            table = ConvertListToDictionary(list);
        }
        return table;
    }

    /// <summary>
    /// Editor Only
    /// </summary>
    public List<Type> GetList()
    {
        return list;
    }

    private static Dictionary<TKey, TValue> ConvertListToDictionary(List<Type> list)
    {
        Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
        foreach (KeyAndValue<TKey, TValue> pair in list)
        {
            dic.Add(pair.Key, pair.Value);
        }
        return dic;
    }
}

/// <summary>
/// シリアル化できる、KeyValuePair
/// </summary>
[Serializable]
public class KeyAndValue<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public KeyAndValue(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
    public KeyAndValue(KeyValuePair<TKey, TValue> pair)
    {
        Key = pair.Key;
        Value = pair.Value;
    }
}