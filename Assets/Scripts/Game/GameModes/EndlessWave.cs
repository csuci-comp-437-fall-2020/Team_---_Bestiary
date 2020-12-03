using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EndlessWave : MonoBehaviour
{
     public static EndlessWave SharedInstance = null;
     public EnemySpawner flyerSpawnPrefab;
     public EnemySpawner walkerSpawnPrefab;
     public EnemySpawner crawlerSpawnPrefab;
     public float waveDuration = 20f;
     public float waveShrinker = 0.95f;
     public int waveSpawns = 3;
     public Canvas prefabCanvas;
     public GameObject gameOverPrefab;

     public float LeftBounds;
     public float RightBounds;
     public float UpperBounds;
     public float LowerBounds;

     private bool _gameOver;
     private Canvas _canvas;
     private GameObject _gameOverPanel;
     private Text _text;
     private Animator _textAnimator;
     private float _timer;
     
     private int _waveCounter = 0;
     
     private static readonly int TimeInterval = Animator.StringToHash("TimeInterval");
     private static readonly int Over = Animator.StringToHash("GameOver");


     void Awake()
    {
        if (SharedInstance != null && SharedInstance != this)
        {
            Destroy(SharedInstance);
        }
        SharedInstance = this;
    }

     private void Start()
     {
         _canvas = Instantiate(prefabCanvas);
         _text = _canvas.GetComponentInChildren<Text>();
         _gameOverPanel = Instantiate(gameOverPrefab, _canvas.transform);
         _gameOverPanel.SetActive(false);
         _textAnimator = _text.GetComponent<Animator>();
     }

     void Update()
     {
         if (!_gameOver)
         {
             _timer += Time.deltaTime;
             _text.text = $"{(int) _timer / 60}:{_timer % 60:00.000}";
             if (_timer >= waveDuration * _waveCounter)
             {
                 waveDuration *= waveShrinker;
                 _textAnimator.SetTrigger(TimeInterval);
                 Spawn();
                 _waveCounter++;
             }
         }
     }

    private void Spawn()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
        int enemiesToSpawn = _waveCounter * waveSpawns - enemyCount;

        if (enemiesToSpawn > 0)
        {
            enemiesToSpawn = Random.Range(1, enemiesToSpawn);
            
            float positionX = Random.Range(LeftBounds, RightBounds);
            float positionY = Random.Range(LowerBounds, UpperBounds);

            EnemySpawner spawner;

            switch (Random.Range(0, 100))
            {
                case int n when (n >= 0 && n < 40):
                    spawner = Instantiate(flyerSpawnPrefab, new Vector3(positionX, positionY),
                        Quaternion.identity);
                    spawner.spawnerLife = enemiesToSpawn;
                    Debug.Log("Spawned: " + enemiesToSpawn + " flying enemies");
                    break;
                case int n when (n >= 40 && n < 80):
                    spawner = Instantiate(walkerSpawnPrefab, new Vector3(positionX, positionY),
                        Quaternion.identity);
                    spawner.spawnerLife = enemiesToSpawn;
                    Debug.Log("Spawned: " + enemiesToSpawn + " walking enemies");
                    break;
                case int n when (n >= 80 && n < 100):
                    
                    spawner = Instantiate(crawlerSpawnPrefab, new Vector3(RightBounds, positionY),
                        Quaternion.identity);
                    spawner.spawnerLife = enemiesToSpawn / 2;
                    Debug.Log("Spawned: " + enemiesToSpawn + " crawling enemies");
                    break;
            }
        }
    }

    public void GameOver()
    {
        _gameOver = true;
        _gameOverPanel.SetActive(true);
        _textAnimator.SetBool(Over, true);
        ;
    }
}
