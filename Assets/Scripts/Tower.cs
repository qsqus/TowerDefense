using System.Collections;
using TMPro;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Tower stats")]
    [SerializeField] private float range = 10f;
    [SerializeField] protected float damage = 5f;
    [SerializeField] protected float fireRate = 1f;    // 1 projectile gets fired every 1/fireRate seconds
    [SerializeField] protected float projectileSpeed = 30f;
    [SerializeField] protected float rotationSpeed = 7f;
    [SerializeField] protected int price = 100;
    [SerializeField] protected float resellPriceMultiplier = 0.8f;

    [Header("Upgrading")]
    [SerializeField] private float upgradeTime = 40f;
    [SerializeField] private float upgradeTimeIncrese = 10f;
    [SerializeField] private int maxLevel = 5;
    [SerializeField] private float levelUpVolume = 0.3f;

    [Header("Upgraded stats")]
    [SerializeField] private float rangeUpgradeMultiplier = 1f;
    [SerializeField] private float damageUpgradeMultiplier = 1f;
    [SerializeField] private float fireRateUpgradeMultiplier = 1f;
    [SerializeField] private int resellPriceIncrease = 20;

    [Header("Tags")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private EnemyType[] targetedEnemyTypes = new EnemyType[] {EnemyType.Ground, EnemyType.Air};

    [Header("References")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] protected TMP_Text levelDisplay;
    [SerializeField] protected Transform rotationPoint;
    [SerializeField] protected Transform xAxisRotationPoint;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected GameObject projectileImpactEffet;
    [SerializeField] protected GameObject shootEffect;
    [SerializeField] protected GameObject buildEffect;
    

    [SerializeField] protected float shootEffectScale = 1f;

    protected Transform target;
    protected Transform targetBody;
    protected float fireCountdown = 0f;

    private bool isUpgrading = false;
    private float upgradeProgress = 0f;
    private int currentLevel = 1;
    private int resellPrice;
    private bool isDestroyed = false;
    private bool isSelected = false;
    
    private PlayerBuild playerBuild;

    private void Awake()
    {
        playerBuild = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerBuild>();

    }

    void Start()
    {
        // Calls Update Target every 0.2 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.2f);

        CalculateResellPrice();
        Debug.Log($"Resell price: {resellPrice}");

        progressBar.SetMaxValue(upgradeTime);
        progressBar.SetValue(upgradeProgress);


        ShowBuildEffect();

    }

    // Updates/sets nearest enemy in range as target every 0.2 seconds
    private void UpdateTarget()
    {
        if (target != null && target.GetComponent<Enemy>().IsDead)
        {
            target = null;
        }

        // Does not change target unless has to
        if (target != null && range >= Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)))
        {
            return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript.IsDead)
            {
                continue;
            }

            bool isCorrectType = false;
            for (int i = 0; i < targetedEnemyTypes.Length; i++)
            {
                if (targetedEnemyTypes[i] == enemyScript.GetEnemyType())
                {
                    isCorrectType = true;
                    break;
                }
            }

            if(!isCorrectType)
            {
                continue;
            }

            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(enemy.transform.position.x, enemy.transform.position.z)) - enemyScript.GetEnemyRadius();

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetBody = target.GetComponent<Enemy>().GetEnemyBody();
        }
        else
        {
            target = null;
        }
    }

    // Draws a gizmo sphere visual
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // Returns renderers
    public Renderer[] GetRenderers()
    {
        return renderers;
    }

    void Update()
    {
        // If no target do nothing
        if (target == null)
        {
            return;
        }

        SetRotation();

        HandleShooting();

    }
    // Handles shooting
    protected virtual void HandleShooting()
    {
        // Prevents shooting before tower is looking in enemy direction
        if (Mathf.Abs((Quaternion.LookRotation(targetBody.position - firePoint.position).eulerAngles - xAxisRotationPoint.transform.eulerAngles).x) > 2)
        {
            fireCountdown -= Time.deltaTime;
            return;
        }

        if (fireCountdown <= 0f)
        {
            Shoot();
            // if fire rate is 2 then that means we want to fire 2 bullets each second that means it should countdown from 0.5
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void CalculateResellPrice()
    {
        int temp = (int)(price * resellPriceMultiplier);
        resellPrice = temp - temp % LevelManager.instance.GetCoinWorth();
    }

    public int GetPrice()
    {
        return price;
    }

    public void StartUpgrading()
    {
        if(currentLevel < maxLevel)
        {
            isUpgrading = true;
            playerBuild.ToggleBuildingAnimation(isUpgrading);
            StartCoroutine(Upgrade(0.2f));
        }

    }

    public void StopUpgrading()
    {
        isUpgrading = false;
        playerBuild.ToggleBuildingAnimation(isUpgrading);
    }

    private IEnumerator Upgrade(float callFrequency)
    {
        while (isUpgrading)
        {

            if (!PauseMenu.IsPaused)
            {
                AddUpgradeProgress(callFrequency);
            }

            yield return new WaitForSeconds(callFrequency);
        }
    }

    // Adds progress to upgrade
    public void AddUpgradeProgress(float amount)
    {
        if(isDestroyed || gameObject == null || Object.ReferenceEquals(gameObject, null))
        {
            return;
        }

        upgradeProgress += amount;
        progressBar.SetValue(upgradeProgress);

        Debug.Log($"Tower is upgrading: {upgradeProgress}");
        if (upgradeProgress >= upgradeTime)
        {
            Debug.Log("Tower upgraded");

            LevelUp();

            if (currentLevel >= maxLevel)
            {
                isUpgrading = false;
            }
        }
    }

    // Levels up the tower
    private void LevelUp()
    {
        if(currentLevel >= maxLevel)
        {
            return;
        }

        SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.towerUpgrade, transform, levelUpVolume);

        upgradeTime += upgradeTimeIncrese;
        upgradeProgress = 0f;
        currentLevel += 1;

        levelDisplay.text = $"Lvl {currentLevel}";

        resellPrice += resellPriceIncrease;

        progressBar.SetMaxValue(upgradeTime);
        progressBar.SetValue(upgradeProgress);

        switch (currentLevel)
        {
            case 2:
                Debug.Log("Level 2 reached");
                damage *= damageUpgradeMultiplier;

                break;
            case 3:
                Debug.Log("Level 3 reached");
                range *= rangeUpgradeMultiplier;

                break;
            case 4:
                Debug.Log("Level 4 reached");
                damage *= damageUpgradeMultiplier;
                fireRate *= fireRateUpgradeMultiplier;

                break;
            case 5:
                Debug.Log("Level 5 reached");
                damage *= damageUpgradeMultiplier;
                range *= rangeUpgradeMultiplier;

                ToggleProgressBar(false);

                isUpgrading = false;
                playerBuild.ToggleBuildingAnimation(isUpgrading);

                break;

        }

        if (isSelected)
        {
            TowerRangeDisplayManager.instance.ShowTowerRange(transform.position, range);
        }

    }
    
    public void ShowBuildEffect()
    {
        GameObject buildEffectGO = Instantiate(buildEffect, transform.position, transform.rotation);

        Destroy(buildEffectGO, 2f);
    }

    public int GetResellPrice()
    {
        return resellPrice;
    }

    public float GetTowerRange()
    {
        return range;
    }

    public void ToggleProgressBar(bool isActive)
    {
        isActive = (currentLevel < maxLevel) ? isActive : false;

        progressBar.gameObject.SetActive(isActive);
    }

    public void DestroyTower()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
    
    protected void InstantiateShootEffect(Transform targetTransform)
    {
        GameObject shootEffectSO = Instantiate(shootEffect, targetTransform.position, targetTransform.rotation);
        
        if (shootEffectScale != 1f)
        {
            shootEffectSO.transform.localScale *= shootEffectScale;
        }

        Destroy(shootEffectSO, 2f);
    }

    // Checks if firePoint is facing the target
    protected bool IsFacingTarget()
    {
        // Calculate the direction vector from this GameObject to the target
        Vector3 directionToTarget = targetBody.position - firePoint.position;

        // Project direction onto the plane ignoring the y axis to only consider the x and z axes
        Vector3 directionToTargetXZ = new Vector3(directionToTarget.x, 0, directionToTarget.z).normalized;

        // Calculate the forward direction of this GameObject, ignoring the y axis
        Vector3 forwardXZ = new Vector3(firePoint.forward.x, 0, firePoint.forward.z).normalized;

        // Calculate the angle between the forward vector and the direction vector
        float angle = Vector3.Angle(forwardXZ, directionToTargetXZ);

        return angle <= 6f;
    }

    public EnemyType[] GetTargetedEnemyTypes()
    {
        return targetedEnemyTypes;
    }

    public void ToggleIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }

    protected abstract void SetRotation();

    protected abstract void Shoot();

}
