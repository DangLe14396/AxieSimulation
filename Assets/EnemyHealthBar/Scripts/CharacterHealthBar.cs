using UnityEngine;
using TMPro;
public class CharacterHealthBar : MonoBehaviour {

    [SerializeField]
    private TextMeshPro hpText,levelText;
    [SerializeField]
    private Transform bar;
    int hp, maxHp;
   

    private void Start()
    {
        levelText.text = GetComponentInParent<CharacterController>().attackRate.ToString();

    }
    public void SetOrder()
    {

    }
    float newHp;
    bool isUpdated = false;
    public void OnUpdate(int hp,int maxHp)
    {
        isUpdated = true;
        this.hp = hp;

        this.maxHp = maxHp;
        
    }
    public void OnUpdateColor(Color c)
    {
        hpText.color = c;
        levelText.color = c;
    }
    int lastHP = -1;
    Vector3 pos=Vector3.one;
    private void Update()
    {
        if (isUpdated)
        {
            newHp = Mathf.MoveTowards(newHp, hp, 30 * Time.deltaTime);
            if (lastHP != (int)newHp)
            {
                hpText.text = ((int)(newHp)).ToString();
                lastHP = (int)newHp;
            }
            float value = hp * 1f / maxHp;
            //pos.x = -1.075f + 1.075f * value;
            pos.x = Mathf.Lerp(pos.x,value,0.1f);
            bar.localScale = pos;

            if ((int)newHp == hp && pos.x<value+0.02f)
            {
                pos.x = value;
                bar.localScale = pos;
                isUpdated = false;
            }
        }
    }
   

   

}