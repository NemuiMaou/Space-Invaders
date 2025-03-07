using System.Collections;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Enemy[] prefabs = new Enemy[5];

    private Vector3 direction = Vector3.right;
    private Vector3 iPosition;

    [Header("Grid")]
    public int rows = 5;
    public int columns = 11;

    private float moveSpeed = 1.0f;
    private float minMoveSpeed = 0.1f;
    private float currentMoveSpeed;
    private int invadersLeft;
    private int totalInvaders;

    [Header("Scripts")] 
    public GameManager gamemanager;
    private void Awake()
    {
        iPosition = transform.position;
        CreateInvaderGrid();
    }

    private void Start()
    {
        currentMoveSpeed = moveSpeed;
        StartCoroutine(MoveInvaders());
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += EnemyDeath;
    }
    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= EnemyDeath;
    }

    private void CreateInvaderGrid()
    {
        totalInvaders = rows * columns; 

        float width = 2f * (columns - 1);
        float height = 2f * (rows - 1);
        Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);

        for (int i = 0; i < rows; i++)
        {
            Vector3 rowPosition = new Vector3(centerOffset.x, (.75f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++)
            {
                Enemy enemy = Instantiate(prefabs[i % prefabs.Length], transform);
                Vector3 position = rowPosition;
                position.x += 1.5f * j;
                enemy.transform.position = transform.position + position;
            }
        }

        invadersLeft = totalInvaders;
    }

    private IEnumerator MoveInvaders()
    {
        while (invadersLeft > 0)
        {
            if (IsBorder())
            {
                FrontStep();
            }
            
            transform.position += direction * currentMoveSpeed;
            
            currentMoveSpeed = Mathf.Max(minMoveSpeed, moveSpeed - 0.015f * ((totalInvaders - invadersLeft) * 2));
            yield return new WaitForSeconds(currentMoveSpeed);
        }
    }
    public void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = iPosition;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        CreateInvaderGrid();
        currentMoveSpeed = moveSpeed;
    }

    private void FrontStep()
    {
        direction = new Vector3(-direction.x, 0f, 0f);
        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }

    private bool IsBorder()
    {
        bool leftmostOutOfBounds = false;
        bool rightmostOutOfBounds = false;

        foreach (Transform enemy in transform)
        {
            if (enemy.gameObject.activeSelf)
            {
                if (enemy.position.x <= -8f)
                    leftmostOutOfBounds = true;
                if (enemy.position.x >= 8f)
                    rightmostOutOfBounds = true;

                if (leftmostOutOfBounds && rightmostOutOfBounds)
                    return true;
            }
        }

        return leftmostOutOfBounds || rightmostOutOfBounds;
    }

    private void EnemyDeath(int points)
    {
        invadersLeft--;
        gamemanager.ScoreIncrease(points);
        moveSpeed = Mathf.Min(5.0f, moveSpeed + 0.02f);
        
        if (invadersLeft <= 0)
        {
            ResetInvaders();
        }
    }
}