using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float health = 100f;
    [SerializeField]
    private int damage = 1;

    private Transform target;
    private int pathElementIdx = 0;

    private void Start()
    {
        GetTarget();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // Destroys enemy when there is no pathElements left
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);

        // Chooses next target when arrived at current target
        if (Vector3.Distance(transform.position, target.position) <= 0.5f)
        {
            pathElementIdx += 1;
            GetTarget();
        }
    }

    private void GetTarget()
    {
        target = EnemyPath.GetEnemyPathElement(pathElementIdx);
    }

}
