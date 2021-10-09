using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hpText, nameText,levelText;
    [SerializeField]
    private Image avatarImg,hpBar;
    CharacterController currentCharacter;
    [SerializeField]
    Animator anim;
    [SerializeField]
    private Spine.Unity.SkeletonGraphic avatar;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetUp(CharacterController character)
    {
        ScrollBack.Instance.active = false;
        if (currentCharacter != null)
        {
            currentCharacter.onTakenDamage -= OnUpdate;
        }

        currentCharacter = character;

        currentCharacter.onTakenDamage += OnUpdate;

        avatar.skeletonDataAsset = character.GetAsset();
        avatar.startingLoop = true;
        avatar.startingAnimation = "action/idle";
        avatar.Initialize(true);

        nameText.text = character.stat.nickname;
        levelText.text = character.attackRate.ToString();
        SetHP(character.currentHP, character.stat.health);
        gameObject.SetActive(true);
    }
    void OnUpdate(int takenDamage)
    {
        SetHP(currentCharacter.currentHP, currentCharacter.stat.health);
    }
    public void Hide()
    {
        anim.SetTrigger("Close");
    }
    public void Deactive()
    {
        currentCharacter.OnDeselect();
        currentCharacter.onTakenDamage -= OnUpdate;
        gameObject.SetActive(false);
        ScrollBack.Instance.active = true;

    }
    public void SetHP(int hp,int maxHp)
    {
        hpText.text = hp + "/" + maxHp;
        hpBar.fillAmount = hp*1f / maxHp;
    }
}
