using Palmmedia.ReportGenerator.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("Upgraded stats")]
    [SerializeField] private float rangeIncrease = 1f;
    [SerializeField] private float damageIncrease = 1f;
    [SerializeField] private float fireRateIncrease = 1f;
    [SerializeField] private int resellPriceIncrease = 20;

    [Header("Tags")]
    [SerializeField] private string enemyTag = "Enemy";

    [Header("References")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] protected Transform rotationPoint;
    [SerializeField] protected Transform xAxisRotationPoint;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected GameObject projectileImpactEffet;

    protected Transform target;
    protected float fireCountdown = 0f;

    private bool isUpgrading = false;
    private float upgradeProgress = 0f;
    private int currentLevel = 1;
    private int resellPrice;


    void Start()
    {
        // Calls Update Target every 0.2 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.2f);

        CalculateResellPrice();
        Debug.Log($"Resell price: {resellPrice}");

        progressBar.SetMaxValue(upgradeTime);
        progressBar.SetValue(upgradeProgress);

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
            if (enemy.GetComponent<Enemy>().IsDead)
            {
                continue;
            }

            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(enemy.transform.position.x, enemy.transform.position.z));

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
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
        if (Mathf.Abs((Quaternion.LookRotation(target.position - firePoint.position).eulerAngles - xAxisRotationPoint.transform.eulerAngles).x) > 2)
        {
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
            StartCoroutine(Upgrade(0.2f));
        }

    }

    public void StopUpgrading()
    {
        isUpgrading = false;
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
        if(gameObject == null || Object.ReferenceEquals(gameObject, null))
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

        upgradeTime += upgradeTimeIncrese;
        upgradeProgress = 0f;
        currentLevel += 1;

        resellPrice += resellPriceIncrease;

        progressBar.SetMaxValue(upgradeTime);
        progressBar.SetValue(upgradeProgress);

        switch (currentLevel)
        {
            case 2:
                Debug.Log("Level 2 reached");
                damage *= damageIncrease;

                break;
            case 3:
                Debug.Log("Level 3 reached");
                range *= rangeIncrease;

                TowerRangeDisplayManager.instance.HideTowerRange();
                TowerRangeDisplayManager.instance.ShowTowerRange(transform.position, range);

                break;
            case 4:
                Debug.Log("Level 4 reached");
                damage *= damageIncrease;

                break;
            case 5:
                Debug.Log("Level 5 reached");
                damage *= damageIncrease;

                break;

        }

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
        progressBar.gameObject.SetActive(isActive);
    }

    protected abstract void SetRotation();

    protected abstract void Shoot();

}
