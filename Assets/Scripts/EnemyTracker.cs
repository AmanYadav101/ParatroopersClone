using System.Collections.Generic;
using UnityEngine;


public class EnemyTracker : MonoBehaviour
{
    public static EnemyTracker Instance;
    public Transform turret;

    private List<GameObject> _leftEnemies = new List<GameObject>();
    private List<GameObject> _rightEnemies = new List<GameObject>();
    private bool isClimbingStarted = false; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        if (isClimbingStarted)
        {
            Destroy(enemy);
            return;
        }

        if (IsEnemyOnLeft(enemy))
        {
            _leftEnemies.Add(enemy);
            if (_leftEnemies.Count == 4)
            {
                isClimbingStarted = true; 
                StartClimbing(_leftEnemies);
            }
        }
        else if(IsEnemyOnRightt(enemy))
        {
            _rightEnemies.Add(enemy);
            if (_rightEnemies.Count == 4)
            {
                isClimbingStarted = true; 
                StartClimbing(_rightEnemies);
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (_leftEnemies.Contains(enemy))
        {
            _leftEnemies.Remove(enemy);
        }
        else if (_rightEnemies.Contains(enemy))
        {
            _rightEnemies.Remove(enemy);
        }
    }

    private bool IsEnemyOnLeft(GameObject enemy)
    {
        return enemy.transform.position.x < turret.position.x-0.6f;
    }

    private bool IsEnemyOnRightt(GameObject enemy)
    {
        return enemy.transform.position.x > turret.position.x + 0.6f;
    }

    public bool CanDropEnemy(bool isLeftSide)
    {
        if (isClimbingStarted) return false;

        if (_leftEnemies.Count == 4 || _rightEnemies.Count == 4) return false;
        if (isLeftSide)
        {
            return _leftEnemies.Count < 4;
        }
        else
        {
            return _rightEnemies.Count < 4;
        }
    }

    private void StartClimbing(List<GameObject> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyClimber climbing = enemies[i].GetComponent<EnemyClimber>();
            if (climbing != null)
            {
                climbing.StartClimbing(i);
            }
        }
    }
}