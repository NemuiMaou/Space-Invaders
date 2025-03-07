using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;

    public delegate void EnemyDied(int points);
    public static event EnemyDied OnEnemyDeath;
    private int score;
    public enum EnemyType
    {
        SmallType,
        MediumType,
        LargeType,
        MysteryShip
    }
    private void Start()
    {
        Scoring(); 
    }

    private void Scoring()
    {
        switch (enemyType)
        {
            case EnemyType.SmallType:
                score = 10;
                break;
            case EnemyType.MediumType:
                score = 20;
                break;
            case EnemyType.LargeType:
                score = 30;
                break;
            case EnemyType.MysteryShip:
                score = 50;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); 
            Debug.Log($"Enemy killed was worth {score} points");
            OnEnemyDeath?.Invoke(score);
            gameObject.SetActive(false); 
        }
    }
}