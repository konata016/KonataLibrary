using System;
using System.Collections.Generic;
using UnityEngine;

public class GimmickController<TGimmick, TEnum>
    where TGimmick : IGimmick<TEnum>
    where TEnum : Enum
{
    private readonly Dictionary<TEnum, IGimmickEffect<TGimmick, TEnum>> gimmickEffectMap;

    private Action onDiscarding;

    public GimmickController()
    {
        gimmickEffectMap = new Dictionary<TEnum, IGimmickEffect<TGimmick, TEnum>>();
        onDiscarding = null;
    }

    
    /// <summary>
    /// ギミックの効果処理の追加
    /// </summary>
    public void AddGimmickEffect(
        TEnum gimmickType,
        IGimmickEffect<TGimmick, TEnum> gimmickEffect)
    {
        gimmickEffectMap.Add(gimmickType, gimmickEffect);
        onDiscarding += gimmickEffect.Discard;
    }

    /// <summary>
    /// ギミック起動時処理
    /// </summary>
    public void OnEnterGimmick(TGimmick gimmick, Transform character)
    {
        if (!gimmickEffectMap.ContainsKey(gimmick.GimmickType))
        {
            return;
        }

        gimmick.OnGimmickEnter(character);
        gimmickEffectMap[gimmick.GimmickType].OnInvocating(gimmick);
    }

    /// <summary>
    /// 破棄時処理
    /// </summary>
    public void Discard()
    {
        onDiscarding?.Invoke();
    }
}