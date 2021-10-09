using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinText, plusText;
    [SerializeField]
    private Transform coinTransform;
    public bool isPersistant = true;
    [SerializeField]
    private bool isGem = false;
    // Start is called before the first frame update
    void Start()
    {
        CoinManager.onUpdate += UpdateCoin;
        CoinManager.onAdded += OnAdded;
        UpdateCoin(CoinManager.Instance.GetCoin().ToString(),CoinManager.Instance.GetGem().ToString());
    }
    public void UpdateCoin(string amount,string amountGem)
    {
        coinText.text = isGem?amountGem:amount;
        CoinEffect();

    }
    public void OpenShop()
    {
        SoundManager.Instance.PlayClickSFX();
    }
    public void CoinEffect()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DoEffect());
        }
    }
    IEnumerator DoEffect()
    {
        float angle = Mathf.PI/4;
        Vector3 one = Vector3.one;
        while (angle < 3*Mathf.PI/4)
        {
            angle += Mathf.PI / 10;
            coinTransform.localScale = one*(0.34f+Mathf.Sin(angle));
            yield return null;
        }
        coinTransform.localScale = Vector3.one;
        angle = Mathf.PI;
        yield return null;
    }
    public void Show()
    {
        if (isPersistant) return;
        Active();
    }
    public void Hide()
    {
        if (isPersistant) return;
        Deactive();
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    public void OnAdded(int amount)
    {
        if (!gameObject.activeSelf) return;
        ShowPlusText(amount);
    }
    private void OnEnable()
    {
        plusText.text = "";
    }
    public void ShowPlusText(int amount)
    {
        if (!gameObject.activeInHierarchy) return;
        plusText.text = (amount > 0 ? "+" : "") + amount + "<sprite=0>";
        StopAllCoroutines();
        StartCoroutine(DoShowCoinPS());
    }
    Color c;
    IEnumerator DoShowCoinPS()
    {
        c = plusText.color;
        c.a = 0;
        plusText.color = c;
        Transform t = plusText.transform;
        t.localScale = Vector3.one;
        Vector3 scale = t.localScale;
        t.localScale = new Vector3(0, 0, 1);
        while (c.a < 0.98f)
        {
            c.a = Mathf.Lerp(c.a, 1, 0.25f);
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);
            plusText.color = c;
            yield return null;
        }
        float a = 0;
        float s = t.localScale.x;
        while (a < Mathf.PI*3)
        {
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);
            scale.x = scale.y = s + Mathf.Sin(a) / 10f;
            a += Mathf.PI / 45;
            yield return null;
        }
        scale.x = scale.y = 1.5f;
        while (c.a > 0.02f)
        {
            c.a = Mathf.Lerp(c.a, 0, 0.2f);
            plusText.color = c;
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);

            yield return null;
        }
        c.a = 0;
        plusText.color = c;
    }

}
