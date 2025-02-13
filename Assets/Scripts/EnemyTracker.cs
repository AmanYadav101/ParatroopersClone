using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public static EnemyTracker Instance; // Singleton instance for easy access

    public Transform turret; // Reference to the turret

    private List<GameObject> _leftEnemies = new List<GameObject>(); // Enemies on the left side
    private List<GameObject> _rightEnemies = new List<GameObject>(); // Enemies on the right side

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add an enemy to the tracker
    public void AddEnemy(GameObject enemy)
    {
        if (IsEnemyOnLeft(enemy))
        {
            _leftEnemies.Add(enemy);
        }
        else
        {
            _rightEnemies.Add(enemy);
        }
    }

    // Remove an enemy from the tracker
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

    // Check if an enemy is on the left side of the turret
    private bool IsEnemyOnLeft(GameObject enemy)
    {
        return enemy.transform.position.x < turret.position.x;
    }

    // Check if a side has reached the maximum number of enemies
    public bool CanDropEnemy(bool isLeftSide)
    {
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
}