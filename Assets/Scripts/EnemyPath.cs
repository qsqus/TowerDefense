using System.Xml;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    private static Transform[] enemyPathElements;

    private void Awake()
    {
        enemyPathElements = new Transform[transform.childCount];
        
        for (int i = 0; i < enemyPathElements.Length; i++)
        {
            enemyPathElements[i] = transform.GetChild(i);
        }
    }

    static public Transform GetEnemyPathElement(int idx)
    {
        if(idx < enemyPathElements.Length)
        {
            return enemyPathElements[idx];
        }

        return null;
    }
}
