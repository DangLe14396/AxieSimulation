using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float worldSpeed = 1;
    public HighLightMap highLightMap;
    public MainTileMap mainMap;

    public CharacterController currentActionCharacter,selectedCharacter;
    public List<List<CharacterController>> characters = new List<List<CharacterController>>();
    List<CharacterController> characterQueue = new List<CharacterController>();
    [SerializeField]
    private GameObject heroPrefab, monsterPrefab;
    public bool canPick = true, canMove = false,canAttack=false;
    public int currentTurn = -1;
    public int initRange = 3;
    public delegate void OnSpeedChanged(float newSpeed);
    public OnSpeedChanged onSpeedChanged;

    public int[] teamPowers=new int[2] { 0, 0 };
    public bool stressTest = false;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }
    IEnumerator Start()
    {

        characters.Add(new List<CharacterController>());
        characters.Add(new List<CharacterController>());

        if (stressTest)
        {
            //StartCoroutine(DoStartStressTest());
        }
        else
        {
            if (initRange > 0)
                InitWorld();
            yield return new WaitForSecondsRealtime(0.5f);
            GameUI.Instance.powerBar.UpdateProgress(teamPowers[0], teamPowers[1]);

            StartGame();
        }

        HighLightMap.Instance.mainMap = mainMap.mainMap;
    }
    private float currentFps=60;
    const string display = "{0} FPS";
    [SerializeField]
    private TMPro.TextMeshPro fpsText,rangeText;


    Vector3Int genCenter = Vector3Int.zero;
    Vector3Int genStartPos = Vector3Int.zero;
    int genRange = 1;
    int type = 1;
    float timer = 2;
    private void Update()
    {
        if (stressTest)
        {
            // measure average frames per second
            currentFps = 1f / Time.smoothDeltaTime;
            fpsText.text = (int)currentFps + " FPS";

           
            if (currentFps >= 30)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    Create(type % 2);
                    type++;
                    rangeText.text = "Radius:" + genRange;
                    genRange += 2;
                    timer = 1;

                }
            }
            else
            {
                rangeText.text = "RADIUS:" + (genRange - 2);

            }
        }
        
    }
    void Create(int type)
    {
        genStartPos = new Vector3Int(genCenter.x + HexGridHandler.directions[4].x * genRange, genCenter.y + HexGridHandler.directions[4].y * genRange, genCenter.z + HexGridHandler.directions[4].z * genRange);
        for (int i = 0; i < 6; i++)
        {
            for (int r = 0; r < genRange; r++)
            {
                CharacterController cc = GetCharacter((CharacterType)type);
                cc.Init(HexGridHandler.CubeToOddr(genStartPos));
                genStartPos = HexGridHandler.GetNeighbor(genStartPos, i, HexGridHandler.directions);
            }
        }
    }
  
    CharacterController GetCharacter(CharacterType type)
    {
        GameObject o = HighLightSpawner.Instance.Get((int)type+1);
        return o.GetComponent<CharacterController>();
    }
    void InitWorld()
    {
        Vector3Int center = Vector3Int.zero;
        Vector3Int startPos = Vector3Int.zero;

        int maxRange = initRange*2+1;
        CharacterController cc = GetCharacter(CharacterType.Monster);
        cc.Init(HexGridHandler.CubeToOddr(startPos));
        
        for (int r = 1; r <= initRange; r++)
        {
            startPos = new Vector3Int(center.x + HexGridHandler.directions[4].x * r, center.y + HexGridHandler.directions[4].y * r, center.z + HexGridHandler.directions[4].z * r);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    cc = GetCharacter(CharacterType.Monster);
                    cc.Init(HexGridHandler.CubeToOddr(startPos));
                    startPos = HexGridHandler.GetNeighbor(startPos, i, HexGridHandler.directions);
                }
            }
        }
        for (int r = initRange+2; r <= maxRange; r++)
        {
            startPos = new Vector3Int(center.x + HexGridHandler.directions[4].x * r, center.y + HexGridHandler.directions[4].y * r, center.z + HexGridHandler.directions[4].z * r);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    cc = GetCharacter(CharacterType.Hero);
                    cc.Init(HexGridHandler.CubeToOddr(startPos));
                    startPos = HexGridHandler.GetNeighbor(startPos, i, HexGridHandler.directions);
                }
            }
        }
    }

    public void StartGame()
    {
        currentTurn++;
        GameUI.Instance.roundSign.Show(currentTurn+1);

        GameUI.Instance.SetRound(currentTurn+1);
        characterQueue.Clear();
        for(int i=0;i< characters[(int)CharacterType.Hero].Count; i++)
        {
            if (characters[(int)CharacterType.Hero][i].isDead)
            {
                continue;
            }
            characterQueue.Add(characters[(int)CharacterType.Hero][i]);
        }
        Invoke(nameof(SetAction),1.5f);
    }
   public void Register(CharacterController ctr)
    {
        characters[(int)ctr.characterType].Add(ctr);
        teamPowers[(int)ctr.characterType] += (ctr.attackRate+1) * ctr.stat.health;
        MiniMap.Instance.Register(ctr);
    }
    public void ShowCharacterStat(CharacterController heroController)
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.OnDeselect();
        }
        selectedCharacter = heroController;
        GameUI.Instance.healthBar.SetUp(selectedCharacter);
    }
    public void UnSelected()
    {
        selectedCharacter = null;
        GameUI.Instance.healthBar.Hide();

    }
    
    public void SetAction()
    {
        if (characterQueue.Count > 0)
        {
            HeroController hero = (HeroController)characterQueue[0];
            characterQueue.RemoveAt(0);

            SetSelectedHero(hero);
        }
        else
        {
            StartGame();
        }

    }
    public void SetSelectedHero(CharacterController heroController)
    {
        currentActionCharacter = heroController;
        List<CharacterController> nearByEnemies = FindNearByEnemy(currentActionCharacter.GetCurrentCell());
        
        CharacterController target = FindClosestEnemy(currentActionCharacter.GetCurrentCell());
        if (target != null)
        {
            Vector3Int targetPoint = HexGridHandler.OddrToCube(target.GetCurrentCell());
            Vector3Int heroPoint = HexGridHandler.OddrToCube(currentActionCharacter.GetCurrentCell());
            Vector3Int result = targetPoint;
            int min = int.MaxValue;
            for (int i = 0; i < HexGridHandler.sideDirections.Length; i++)
            {
                Vector3Int targetNeighbor = HexGridHandler.GetNeighbor(targetPoint, i, HexGridHandler.sideDirections);
                if (!IsOccupied(HexGridHandler.CubeToOddr(targetNeighbor)))
                {
                    int distance = HexGridHandler.GetDistance(heroPoint, targetNeighbor);
                    if (distance < min)
                    {
                        min = distance;
                        result = targetNeighbor;
                    }
                }
                else
                {
                    if (targetNeighbor == heroPoint)
                    {
                        result = heroPoint;
                        break;
                    }
                }
            }

            if (result != targetPoint)
            {
                CameraShake.Instance.FocusOnTarget(currentActionCharacter.transform);

                currentActionCharacter.PrepareCombat(target);
                target.PrepareCombat(currentActionCharacter);

                if (result != heroPoint)
                {
                    MoveToPoint(HexGridHandler.CubeToOddr(result), isArrived =>
                    {
                        Attack(target);
                    });
                }
                else
                {
                    Attack(target);
                }
            }
            else
            {
                SetAction();
            }
               
        }
        
    }
    Coroutine c;
    public void Attack(CharacterController target)
    {
        Debug.Log("ATTACK ENEMY: " + target.gameObject.name);
        if (c != null)
        {
            StopCoroutine(c);
        }
        c =StartCoroutine(DoAttack(currentActionCharacter,target));


    }
    bool isFinished = false;

    IEnumerator DoAttack(CharacterController me,CharacterController target)
    {
        isFinished = false;
        float timer = 0.4f;
        while (timer > 0)
        {
            timer -= Time.deltaTime*worldSpeed;
            yield return null;
        }
        bool isTargetHurt = false;
        me.Attack(target,res =>
        {
            isTargetHurt = res;
            isFinished = true;
        });
        while (!isFinished)
        {
            yield return null;
        }
        if (!isTargetHurt)
        {
            timer = 2.5f;
            while (timer > 0)
            {
                timer -= Time.deltaTime * worldSpeed;
                yield return null;
            }
            isTargetHurt = false;
        }
        isFinished = false;
        target.Attack(me,res=> 
        {
            isFinished = true;
        });
        while (!isFinished)
        {
            yield return null;
        }


        if (target.currentHP <= 0)
        {
            target.Dead();
        }
        if (me.currentHP <= 0)
        {
            me.Dead();
        }

        me.isTurnEnded = true;
        timer = 1f;
        while (timer > 0)
        {
            timer -= Time.deltaTime * worldSpeed;
            yield return null;
        }


        if (me.isDead || target.isDead)
        {
            if (!CheckWin())
            {
                SetAction();
            }
            else
            {
                for(int i = 0; i < characters[GameController.Instance.victoryTeam].Count; i++)
                {
                    if (!characters[GameController.Instance.victoryTeam][i].isDead)
                    {
                        characters[GameController.Instance.victoryTeam][i].PlayVictoryPose();
                    }
                }
                GameController.Instance.levelManager.selectedLevelMap.Finish();
            }
        }
        else
        {
            SetAction();
        }

       
        c = null;
    }
    bool CheckWin()
    {
        if (teamPowers[(int)CharacterType.Monster] == 0)
        {
            GameController.Instance.victoryTeam = 0;
            return true;
        }
        else if (teamPowers[(int)CharacterType.Hero] == 0)
        {
            GameController.Instance.victoryTeam = 1;
            return true;

        }
        return false;
    }
  
    public void OnAttack()
    {
        canAttack = true;
        highLightMap.Hide();

        int range = currentActionCharacter.stat.damageRange;
        Vector3Int heroPos = mainMap.WorldToCell(currentActionCharacter.GetPos());
        heroPos = HexGridHandler.OddrToCube(heroPos);
        Vector3Int startPos = heroPos;
        highLightMap.SetTile(mainMap.CellToWorld(HexGridHandler.CubeToOddr(startPos)),Color.red);
        for (int r = 1; r <= range; r++)
        {
            startPos = new Vector3Int(heroPos.x + HexGridHandler.directions[4].x * r, heroPos.y + HexGridHandler.directions[4].y * r, heroPos.z + HexGridHandler.directions[4].z * r);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    CharacterType type;
                    bool isOccupied = IsOccupied(HexGridHandler.CubeToOddr(startPos),out type);
                    highLightMap.SetTile(mainMap.CellToWorld(HexGridHandler.CubeToOddr(startPos)),isOccupied?(type==currentActionCharacter.characterType?Color.white:Color.magenta):Color.red, r / 15f);

                    startPos = HexGridHandler.GetNeighbor(startPos, i, HexGridHandler.directions);
                }
            }
        }

    }
    public void OnMove()
    {
        highLightMap.Hide();
        canMove = true;
        int range = currentActionCharacter.stat.moveRange;
        Vector3Int heroPos = mainMap.WorldToCell(currentActionCharacter.GetPos());
        heroPos = HexGridHandler.OddrToCube(heroPos);
        Vector3Int startPos = heroPos;
        highLightMap.SetTile(mainMap.CellToWorld(HexGridHandler.CubeToOddr(startPos)), Color.white);
        for (int r = 1; r <= range; r++)
        {
            startPos = new Vector3Int(heroPos.x + HexGridHandler.directions[4].x * r, heroPos.y + HexGridHandler.directions[4].y * r, heroPos.z + HexGridHandler.directions[4].z * r);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    CharacterType type;
                    bool isOccupied = IsOccupied(HexGridHandler.CubeToOddr(startPos),out type);
                    highLightMap.SetTile(mainMap.CellToWorld(HexGridHandler.CubeToOddr(startPos)), isOccupied?Color.red:Color.white, r / 15f);

                    startPos = HexGridHandler.GetNeighbor(startPos, i, HexGridHandler.directions);
                }
            }
        }
    }
   
   
    public bool IsDestinationValid(Vector3Int from,Vector3Int to,float range)
    {
        return HexGridHandler.GetDistance(from, to)<=range;
    }

    public void OnCellSelected(Vector3Int destinationCell)
    {
      
    }
    public bool MoveToPoint(Vector3Int destinationCell,System.Action<bool> onArrived)
    {
        Vector3Int heroPos = mainMap.WorldToCell(currentActionCharacter.GetPos());
        heroPos = HexGridHandler.OddrToCube(heroPos);

        currentActionCharacter.MoveToPoint(FindWorldPath(heroPos,
            HexGridHandler.OddrToCube(destinationCell)), onArrived);



        return true;
       

    }

    public bool IsOccupied(Vector3Int cell, out CharacterType type)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            for (int c = 0; c < characters[i].Count; c++)
            {
                if (!characters[i][c].isDead && characters[i][c].GetCurrentCell() == cell)
                {
                    type = characters[i][c].characterType;
                    return true;
                }
            }
        }
        type = CharacterType.Hero;

        return false;
    }
    public bool IsOccupied(Vector3Int cell)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            for (int c = 0; c < characters[i].Count; c++)
            {
                if (!characters[i][c].isDead &&characters[i][c].GetCurrentCell() == cell)
                {
                    return true;
                }
            }
        }
        return false;
    }


    Vector3Int[] FindPath(Vector3Int a, Vector3Int b)
    {
        int distance = HexGridHandler.GetDistance(a, b);
        Vector3Int []results = new Vector3Int[distance];
        for (int i = 0; i <= distance; i++) {
            results[i] = (HexGridHandler.CubeRound(HexGridHandler.CubeLerp(a, b, 1f / distance * i)));
        }
        return results;
    }
    Vector3[] FindWorldPath(Vector3Int a, Vector3Int b)
    {
        int distance = HexGridHandler.GetDistance(a, b);
        Vector3[] results = new Vector3[distance+1];
        for (int i = 0; i <= distance; i++)
        {
            results[i] = mainMap.CellToWorld(HexGridHandler.CubeToOddr(HexGridHandler.CubeRound(HexGridHandler.CubeLerp(a, b, 1f / distance * i))));
        }
        return results;
    }

    List<CharacterController> FindNearByEnemy(Vector3Int from)
    {
        List<CharacterController> results = new List<CharacterController>();
        List<CharacterController> list = characters[(int)CharacterType.Monster];

        for(int i = 0; i < list.Count; i++)
        {
            if (list[i].isDead) continue;
            Vector3Int to = list[i].GetCurrentCell();
            if (HexGridHandler.GetDistance(HexGridHandler.OddrToCube(from), HexGridHandler.OddrToCube(to))==1)
            {
                results.Add(list[i]);
            }
        }
        return results;
    }
    CharacterController FindClosestEnemy(Vector3Int from)
    {
        List<CharacterController> list = characters[(int)CharacterType.Monster];

        CharacterController closestEnemy = null;
        from = HexGridHandler.OddrToCube(from);
        int min = int.MaxValue;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].isDead) continue;
            int distance = HexGridHandler.GetDistance(from, HexGridHandler.OddrToCube(list[i].GetCurrentCell()));
            if (distance < min)
            {
                min = distance;
                closestEnemy = list[i];
            }
        }
        return closestEnemy;
    }

    void OnArrived()
    {
        Debug.Log("CHARACTER ARRIVED");
        currentActionCharacter.didMove = true;
        canMove = false;
       

        ActionPanel.Instance.SetUp(false, true);
    }

    public void OnCharacterDamage(CharacterController character,int damage)
    {
        teamPowers[(int)character.characterType] = (teamPowers[(int)character.characterType] -(character.attackRate + 1) * (character.currentHP+damage)) + (character.attackRate + 1) * character.currentHP;
        GameUI.Instance.powerBar.UpdateProgress(teamPowers[0], teamPowers[1]);
    }

    public void SetWorldSpeed(float speed)
    {
        worldSpeed = speed;
        onSpeedChanged?.Invoke(worldSpeed);

    }
}


class DamageCalculator
{
    static int[] damages = { 4, 5, 3 };
    public static int Calculate(int attackerRate,int targetRate)
    {
        return damages[(3+attackerRate-targetRate)%3];
    }
}


public class HexGridHandler
{
    static Vector3Int temp = Vector3Int.zero;
    public static Vector3Int CubeToOddr(Vector3Int cube)
    {
        int col = cube.x + (cube.z - (cube.z & 1)) / 2;
        int row = cube.z;
        temp.Set(col, row,0);
        return temp;
    }

    public static Vector3Int OddrToCube(Vector3Int hex)
    {
        int x = hex.x - (hex.y - (hex.y & 1)) / 2;
        int z = hex.y;
        int y = -x - z;
        temp.Set(x, y, z);
        return temp;
    }
    static int Abs(int value)
    {
        return value < 0 ? -value : value;
    }
    public static int GetDistance(Vector3Int a, Vector3Int b)
    {
        return (Abs(a.x - b.x) + Abs(a.y - b.y) + Abs(a.z - b.z)) / 2;
    }
    public static Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1,-1,0),
        new Vector3Int(1,0,-1),
        new Vector3Int(0,1,-1),
        new Vector3Int(-1,1,0),
        new Vector3Int(-1,0,1),
        new Vector3Int(0,-1,1),
    };
    public static Vector3Int[] sideDirections = new Vector3Int[]
   {
        new Vector3Int(1,-1,0),
        new Vector3Int(-1,1,0)
   };

    public static Vector3Int GetNeighbor(Vector3Int center, int dir, Vector3Int[] directions)
    {
        temp.Set(center.x + directions[dir].x, center.y + directions[dir].y, center.z + directions[dir].z);
        return temp;
    }
    public static float Lerp(int a, int b, float t)
    {
        return a + (b - a) * t;

    }
    public static Vector3Int CubeRound(Vector3 cube)
    {
        int rx = Mathf.RoundToInt(cube.x);
        int ry = Mathf.RoundToInt(cube.y);
        int rz = Mathf.RoundToInt(cube.z);

        float x_diff = Mathf.Abs(rx - cube.x);
        float y_diff = Mathf.Abs(ry - cube.y);
        float z_diff = Mathf.Abs(rz - cube.z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        temp.Set(rx, ry, rz);
        return temp;
    }

    public static Vector3 CubeLerp(Vector3Int a, Vector3Int b, float t)
    {
        return new Vector3(Lerp(a.x, b.x, t),
                    Lerp(a.y, b.y, t),
                    Lerp(a.z, b.z, t));
    }
}