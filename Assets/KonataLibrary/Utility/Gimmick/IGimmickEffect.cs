using System;

public interface IGimmickEffect<in TGimmick, TEnum> 
    where TGimmick : IGimmick<TEnum>
    where TEnum : Enum
{
    public void OnInvocating(TGimmick gimmick);
    public void Discard();
}