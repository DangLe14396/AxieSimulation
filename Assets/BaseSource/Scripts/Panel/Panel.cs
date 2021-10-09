using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Panel : MonoBehaviour, IBack
{
    protected Animator ani;
    protected Transform _transform;
    protected bool overrideBack = false;
    public virtual void Show()
    {
        SoundManager.Instance.PlaySFX(SFX.popUpOpen, 0.5f);
        Active();
    }
    public virtual void Hide()
    {
        SoundManager.Instance.PlaySFX(SFX.popUpClose, 0.5f);
        BackButtonController.Instance.UnRegister(this);
        ani.SetTrigger("Close");
    }
    public virtual void Deactive()
    {
        gameObject.SetActive(false);
    }
    public virtual void Active()
    {
        BackButtonController.Instance.Register(this,overrideBack);
        gameObject.SetActive(true);
        overrideBack = false;
    }
    public void Init()
    {
        ani = GetComponent<Animator>();
        _transform = transform;
        PostInit();
    }
    public virtual void ShowAfterAd() { }
    public virtual void Close()
    {
        Hide();
    }
    public abstract void PostInit();

    public virtual void OnBack()
    {
        Close();
    }
}
