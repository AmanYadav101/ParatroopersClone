using UnityEngine;
using System.Collections;

public class EnemyClimber : MonoBehaviour
{
    public float climbSpeed = 2f;
    private bool isClimbing = false;
    private Vector3 targetPosition;
    private Transform turret;
    private bool isInFormation = false;
    private int positionInFormation = -1;

    private const float ENEMY_HEIGHT = 1f; 
    private const float ENEMY_WIDTH = 0.4f; 

    private void Start()
    {
        turret = EnemyTracker.Instance.turret;
    }

    public void StartClimbing(int position)
    {
        positionInFormation = position;
        isClimbing = true;

        if (position < 3) 
        {
            StartCoroutine(MoveToFormationPosition());
        }
        else 
        {
            StartCoroutine(ClimbOverFormation());
        }
    }

    private IEnumerator MoveToFormationPosition()
    {
        bool isLeftSide = transform.position.x < turret.position.x;
        float xOffset = isLeftSide ? -ENEMY_WIDTH - 0.6f : ENEMY_WIDTH + 0.6f;

        Vector3 basePosition = turret.position + new Vector3(xOffset, -1, 0);

        switch (positionInFormation)
        {
            case 0: 
                targetPosition = basePosition + new Vector3(0, ENEMY_HEIGHT / 1.5f, 0);
                break;

            case 1: 
                targetPosition = basePosition;
                break;
            case 2: 
                targetPosition = basePosition + new Vector3(
                    isLeftSide ? -ENEMY_WIDTH : ENEMY_WIDTH, 
                    0, 
                    0
                ); break;
        }

        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                climbSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPosition; 
        isInFormation = true;
    }

  
    
    private IEnumerator ClimbOverFormation()
    {
        while (!AreOthersInFormation())
        {
            yield return new WaitForSeconds(0.2f);
        }

        Vector3 startPos = transform.position;
        Vector3 peakPoint = turret.position + new Vector3(
            transform.position.x < turret.position.x ? -ENEMY_WIDTH : ENEMY_WIDTH, 
            ENEMY_HEIGHT,  
            0
        );
        Vector3 endPoint = turret.position;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * climbSpeed;
        
            Vector3 currentPos = Vector3.Lerp(startPos, endPoint, progress);
            float heightOffset = Mathf.Sin(progress * Mathf.PI) * ENEMY_HEIGHT;
        
            transform.position = currentPos;
            yield return null;
        }

        if (turret != null)
        {
            Destroy(turret.gameObject);
        }
    }
    private bool AreOthersInFormation()
    {
        EnemyClimber[] allEnemies = FindObjectsOfType<EnemyClimber>();
        int readyCount = 0;

        foreach (EnemyClimber enemy in allEnemies)
        {
            if (enemy.isInFormation)
            {
                readyCount++;
            }
        }

        return readyCount >= 3;
    }
}