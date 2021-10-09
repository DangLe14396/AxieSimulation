using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class CharacterController : MonoBehaviour
{
    public CharacterType characterType;
    protected Transform _transform;
    public HeroStat stat;
    public bool isDead = false;
    public bool isSelected = false;
    public bool isTurnEnded = false,didMove=false,didAttack=false;
    public int attackRate = 0;
    [SerializeField]
    private GameObject outlineObj;
    MeshRenderer mr;
    [SerializeField]
    private Material outline,normal;

    Vector3Int currentCell;
    Vector3 defaultScale;
    [SerializeField]
    protected SkeletonAnimation anim;
    [SerializeField]
    private CharacterHealthBar healthBar;
    public MiniMap_Mark miniMapMarker;
    [SerializeField]
    private ParticleSystem slashPS, firePS, bloodPS;




    public int currentHP;

    private  string[] attackAnims = new string[]
    {
        "back-gore","back-slam","horn-gore","horn-slash","mouth-bite","tail-multi-slap","tail-roll","tail-smash"
    };
    private  string[] hurtAnims = new string[]
    {
        "hit-by-normal-attack","hit-by-horn-attack"
    };
    private const string counterAnim = "last-stand/defense/evade";
    private const string idleAnim = "action/idle";

    [SerializeField]
    private AudioClip []attackSFXs,hurtSFXs,deadSFXs;
    [SerializeField]
    private AudioClip selectSFX,deselectSFX,criticalSFX,counterSFX;

    public SkeletonDataAsset GetAsset()
    {
        return anim.SkeletonDataAsset;
    }


    public delegate void OnTakenDamage(int damage);
    public OnTakenDamage onTakenDamage;
    // Start is called before the first frame update
    void Awake()
    {
        _transform = transform;
        defaultScale = _transform.localScale;
        defaultScale.x = defaultScale.x < 0 ? -defaultScale.x : defaultScale.x;
        mr = anim.GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        anim.OnMeshAndMaterialsUpdated += Anim_OnMeshAndMaterialsUpdated;
        anim.AnimationState.Event += AnimationState_Event;
    }

  

    bool isVisible = true;
    public void OnVisible()
    {
        if (!healthBar.gameObject.activeSelf && !isDead)
        {
            healthBar.gameObject.SetActive(true);
            isVisible = true;
        }
    }
    public void OnInvisible()
    {
        if (healthBar.gameObject.activeSelf)
        {
            healthBar.gameObject.SetActive(false);
            isVisible = false;

        }
    }
    public void Init()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.onSpeedChanged -= OnSpeedChanged;
        GameManager.Instance.onSpeedChanged += OnSpeedChanged;
        isDead = false;
        isVisible = true;
        _transform.localScale = defaultScale;
        Vector3 facing = _transform.localScale;
        facing.x = facing.x < 0 ? -1 : 1;
        healthBar.transform.localScale = facing;

        attackRate = Random.Range(0, 3);
        currentHP = stat.health;
        healthBar.OnUpdate(currentHP, stat.health);
        GameManager.Instance.Register(this);
        SetCurrentCell(GameManager.Instance.mainMap.WorldToCell(_transform.localPosition));
        anim.AnimationState.SetAnimation(0, idleAnim, true);
    }
  
    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.onSpeedChanged -= OnSpeedChanged;
    }
    void OnSpeedChanged(float newSpeed)
    {
        //anim.AnimationState.GetCurrent(0).TimeScale= newSpeed;
        anim.timeScale= newSpeed;
    }
    private void AnimationState_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name.Equals("hit"))
        {
            OnHitTarget();
        }
    }
    public void Init(Vector3Int cell)
    {
        transform.localPosition=GameManager.Instance.mainMap.CellToWorld(cell);
        SetCurrentCell(cell);
        gameObject.SetActive(true);
        Init();
    }
    public void PlayVictoryPose()
    {
        anim.AnimationState.SetAnimation(0, "action/victory-pose-back-flip", false);
        anim.AnimationState.AddAnimation(0, idleAnim, true,0);
    }
    public void PrepareCombat(CharacterController other)
    {
        Vector3 scale = defaultScale;
        scale.x = other.GetPos().x > _transform.localPosition.x ? defaultScale.x : -defaultScale.x;
        _transform.localScale = scale;


        Vector3 facing = _transform.localScale;
        facing.x = facing.x < 0 ? -1 : 1;
        healthBar.transform.localScale = facing;
    }
    CharacterController target;
    System.Action<bool> onFinishedAttacking,onHurt;
    public void Attack(CharacterController other, System.Action<bool> onFinishedAttacking)
    {
        this.onFinishedAttacking = onFinishedAttacking;
        target = other;
        Vector3 scale = defaultScale;
        scale.x = other.GetPos().x > _transform.localPosition.x ? defaultScale.x : -defaultScale.x;
        _transform.localScale = scale;
        Vector3 facing = _transform.localScale;
        facing.x = facing.x < 0 ? -1 : 1;
        healthBar.transform.localScale = facing;
        string a = "attack/melee/" + attackAnims[Random.Range(0, attackAnims.Length)];
        anim.AnimationState.SetAnimation(0,a, false);
        anim.AnimationState.AddAnimation(0, idleAnim, true,0);


    }
    void OnHitTarget()
    {
       
        Debug.Log("ON HIT TARGET "+ gameObject.name);
        int myDamage = DamageCalculator.Calculate(attackRate, target.attackRate);
        bool isHit=target.Hurt(myDamage,onHurt=> 
        {
            onFinishedAttacking?.Invoke(onHurt);
            onFinishedAttacking = null;
        }
        );

        if (isHit)
        {
            slashPS.Play();
            SoundManager.Instance.PlaySFX(attackSFXs[Random.Range(0, attackSFXs.Length)], 1);
        }
        
    }
    public bool Hurt(int takenDamage,System.Action<bool> onHurt)
    {
        float counterChance = Random.Range(0f, 1f);
        if (counterChance > 0.9f)
        {
            anim.AnimationState.SetAnimation(0, counterAnim, false);
            anim.AnimationState.AddAnimation(0, idleAnim, true, 0);

            onHurt?.Invoke(false);
            onHurt= null;
            SoundManager.Instance.PlaySFX(counterSFX, 1);
            EffectManager.Instance.Get(2).Active(_transform.localPosition + new Vector3(0 + Random.Range(-0.03f, 0.03f), 0.3f + Random.Range(0f, 0.04f)), "MISSED" );

            return false;
        }
        else
        {
            bloodPS.Play();
            bool isCritical = Random.Range(0f, 1f) > 0.9f;
            EffectManager.Instance.Get(1).Active(_transform.localPosition + new Vector3(0, 0.35f));
            SoundManager.Instance.PlaySFX(hurtSFXs[Random.Range(0, hurtSFXs.Length)], 1);
            if (isCritical)
            {
                SoundManager.Instance.PlaySFX(criticalSFX, 1);
            }
            this.onHurt += onHurt;
            takenDamage *= isCritical ? 2 : 1;
            int dmg = currentHP > takenDamage ? takenDamage : currentHP;
            currentHP -= takenDamage;
            currentHP = Mathf.Max(currentHP, 0);

            GameManager.Instance.OnCharacterDamage(this, dmg);
            EffectManager.Instance.Get(2).Active(_transform.localPosition + new Vector3(0 + Random.Range(-0.03f, 0.03f), 0.3f + Random.Range(0f, 0.04f)), takenDamage, isCritical);



            onTakenDamage?.Invoke(takenDamage);
            healthBar.OnUpdate(currentHP, stat.health);
            string a = "defense/" + hurtAnims[Random.Range(0, hurtAnims.Length)];
            anim.AnimationState.SetAnimation(0, a, false);
            anim.AnimationState.AddAnimation(0, idleAnim, true, 0);
            StartCoroutine(DoHurt(a));
            return true;
        }
    }
    IEnumerator DoHurt(string a)
    {
        float time = anim.skeleton.Data.FindAnimation(a).Duration;
        while (time > 0)
        {
            time -= Time.deltaTime * GameManager.Instance.worldSpeed;
            yield return null;
        }
        onHurt?.Invoke(true);
        onHurt = null;
    }
   
    public void Dead()
    {
        if (isDead) return;
        isDead = true;
        EffectManager.Instance.Get(0).Active(_transform.localPosition);
        SoundManager.Instance.PlaySFX(deadSFXs[Random.Range(0, deadSFXs.Length)], 1);
        miniMapMarker.Hide();
        gameObject.SetActive(false);
    }
    public Vector3Int GetCurrentCell()
    {
        return currentCell;
    }
    public void SetCurrentCell(Vector3Int cell)
    {
        currentCell = cell;
    }
    public Vector3 GetPos()
    {
        return _transform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (path != null && path.Length > 0)
            {
                _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, path[pathIndex], stat.speed * Time.deltaTime * GameManager.Instance.worldSpeed);
                if (Vector3.Distance(_transform.localPosition, path[pathIndex]) < 0.02f)
                {
                    miniMapMarker.UpdatePosition();
                    pathIndex++;
                    if (pathIndex >= path.Length)
                    {
                        anim.AnimationState.SetAnimation(0, idleAnim, true);

                        _transform.localPosition = path[path.Length-1];
                        arriveDestinationCallback?.Invoke(true);
                        isMoving = false;
                        path = null;
                        SetCurrentCell(GameManager.Instance.mainMap.WorldToCell(_transform.localPosition));
                    }
                }
            }
           
        }

    }
    private void Anim_OnMeshAndMaterialsUpdated(SkeletonRenderer skeletonRenderer)
    {
        LateUpdate();
    }
    Color c = Color.white;
    private void LateUpdate()
    {
        if (isVisible)
        {
            float dist = Vector2.Distance(_transform.localPosition, CameraShake.Instance.GetTransfrom().localPosition);
            c.a =1-Mathf.Clamp((Mathf.Max(dist,2)-2)/2f,0,1);
            if (dist >= 2) {
                mr.material.SetColor("_Color", c);
                healthBar.OnUpdateColor(c);
            }
        }
    }
    Vector3 moveDestination;
    bool isMoving = false;
    System.Action<bool> arriveDestinationCallback;
    public void MoveToPoint(Vector3 pos,System.Action<bool> callback)
    {
        isMoving = true;
        moveDestination = pos;
        Vector3 scale=defaultScale;
        scale.x = _transform.localPosition.x <= pos.x ? defaultScale.x : -defaultScale.x;
        _transform.localScale = scale;
        healthBar.transform.localScale = scale;
        this.arriveDestinationCallback = callback;
    }
    Vector3[] path;
    int pathIndex = 0;
    public void MoveToPoint(Vector3[] path, System.Action<bool> callback)
    {
        isMoving = true;
        Vector3 scale = defaultScale;
        scale.x = _transform.localPosition.x < path[path.Length-1].x ? defaultScale.x : -defaultScale.x;
        _transform.localScale = scale;
        Vector3 facing = _transform.localScale;
        facing.x = facing.x < 0 ? -1 : 1;
        healthBar.transform.localScale = facing;
        pathIndex = 0;
        this.path = path;
        this.arriveDestinationCallback = callback;
        anim.AnimationState.SetAnimation(0, "action/move-forward", true);
    }
    public virtual void OnSelected()
    {
        if (!ScrollBack.Instance.active) return;

        if (isSelected)
        {
            OnDeselect();
        }
        else
        {
            SoundManager.Instance.PlaySFX(selectSFX, 1);
            isSelected = true;
            outlineObj.SetActive(true);
            anim.transform.localScale = Vector3.one * 1.1f;
            CameraShake.Instance.ForceFocusOnTarget(_transform);
            GameManager.Instance.ShowCharacterStat(this);

        }
    }
    public virtual void OnDeselect()
    {
        if (isSelected)
        {
            SoundManager.Instance.PlaySFX(deselectSFX, 1);
            isSelected = false; 
            anim.transform.localScale = Vector3.one;
            //mr.material.SetFloat("_OutlineWidth", 0);
            //mr.material = normal;
            outlineObj.SetActive(false);

            GameManager.Instance.UnSelected();
            CameraShake.Instance.ClearForceFocus();

        }

    }
    bool isMouseDown = false;
    private void OnMouseDown()
    {
        isMouseDown = true;
    }
    private void OnMouseUp()
    {
        if (isMouseDown)
        {
            OnSelected();
        }
        isMouseDown = false;

    }
}

public enum CharacterType
{
    Hero,Monster
}