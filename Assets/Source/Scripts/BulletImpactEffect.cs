using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletImpactEffect : EffectAbstract
{
    [SerializeField]
    private ParticleSystem ps;
    [SerializeField]
    private ParticleSystem[] otherPS;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void Active(Vector3 pos, Color color)
    {
        throw new System.NotImplementedException();
    }
    public override void Active(Transform parent)
    {
        transform.parent = parent;
        gameObject.SetActive(true);
        ps.Play();
        //StartCoroutine(DoWait(parent.gameObject));
    }
    float time = 0;
    public override void Active(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        ps.Play();
        time = Time.time;
        if (audioSource != null && SettingPanel.Instance.isSFXEnabled)
        {
            audioSource.Play();
        }

    }

    public override void Active(Vector3 pos, Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsUsing()
    {
        return  (Time.time-time)<0.5f;
    }
    public override void Active(SpriteRenderer sr)
    {
        gameObject.SetActive(true);
        ParticleSystem.ShapeModule shape = ps.shape;
        shape.spriteRenderer = sr;
        for (int i = 0; i < otherPS.Length; i++)
        {
            ParticleSystem.ShapeModule s = otherPS[i].shape;
            s.spriteRenderer = sr;
        }
        ps.Play();
    }
}
