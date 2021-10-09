using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameLoader : MonoBehaviour {
    [SerializeField]
    private TMPro.TextMeshProUGUI text;
    [SerializeField]
    private Image loadingBar;
    [SerializeField]
    private Transform loading;
    // Use this for initialization
    void Start()
    {
        //
        LoadScene();
        c= StartCoroutine(Interface());
        Application.backgroundLoadingPriority = ThreadPriority.High;
    }

        public void LoadScene()
    {
        StartCoroutine(Load());
    }
    IEnumerator Interface()
    {
        string loading =text.text;
        while (enabled)
        {
            text.text = loading;
            yield return new WaitForSeconds(0.15f);
            text.text = loading+".";
            yield return new WaitForSeconds(0.15f);
            text.text = loading + "..";
            yield return new WaitForSeconds(0.15f);
            text.text = loading + "...";
            yield return new WaitForSeconds(0.15f);
            text.text = loading + "..";
            yield return new WaitForSeconds(0.15f);
            text.text = loading + ".";
            yield return new WaitForSeconds(0.15f);
        }
    }
    Coroutine c;
    IEnumerator Load()
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 axis = new Vector3(0, 0, 1);
        AsyncOperation async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        async.allowSceneActivation = false;
        //float timer = 0;
        //loadingBar.fillAmount = 0;
        while (async.progress < 0.9f)
        {
            //loadingBar.fillAmount = async.progress;

            //timer += Time.deltaTime;
            loading.Rotate(axis, 2);
            yield return null;
        }

        //loadingBar.fillAmount = 1;
        float t = 0.3f;
        while (t > 0)
        {
            loading.Rotate(axis, 2);
            t -= Time.deltaTime;
            yield return null;
        }

        //int attemp = 0;
        //while (!MasterControl.HasInternet())
        //{
            //attemp++;

            //if (attemp >= 7)
            //{
            //    MessagePanel2.Instance.SetUp("Please turn on Wifi/Mobile Data to update the game");

            //    GameObject o = MessagePanel2.Instance.gameObject;
            //    while (o.activeSelf)
            //    {
            //        loading.Rotate(axis, 2);
            //        yield return null;
            //    }
            //}
            t = 1;
            while (t > 0)
            {
                loading.Rotate(axis, 2);
                t -= Time.deltaTime;
                yield return null;
            }
        //}


#if UNITY_EDITOR
        t = 1f;
        while (t > 0)
        {
            loading.Rotate(axis, 2);
            t -= Time.deltaTime;
            yield return null;
        }
#endif
        t = 0.5f;
        StopCoroutine(this.c);
        text.text = "Ready!!!";
        Image loadingImg = loading.GetComponent<Image>();
        Color c = loadingImg.color;
        c.a = 0;
        while (t > 0)
        {
            loadingImg.color = Color.Lerp(loadingImg.color, c, 0.2f);
            loading.Rotate(axis, 20);
            t -= Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1, 0.2f);
            yield return null;
        }

        async.allowSceneActivation = true;

    }
}
