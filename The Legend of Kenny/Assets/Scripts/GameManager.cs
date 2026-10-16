using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    
    [SerializeField] private GameObject _gate;
    
    [Header("Mice")]
    [SerializeField] private GameObject _mousePrefab;
    [SerializeField] private Transform _mouseParent;
    private GameObject _mouse;
    private int _mouseCount;
    private bool _spawnMouse;
    private Vector2 _mousePosition;
    public int MouseDeath;
    
    [Header("UI")]
    [SerializeField] private TMP_Text _pauseUI;
    [SerializeField] private TMP_Text _quitUI;
    [SerializeField] private TMP_Text _timeDisplay;
    [SerializeField] private Image _gameOverKen;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartText;
    private Mousekeeping.GameState _state;
    public Mousekeeping.GameState State
    {
        get => _state;

        set
        {
            _state = value;
            _pauseUI.enabled = State == Mousekeeping.GameState.Pause;
            _quitUI.enabled = State == Mousekeeping.GameState.Pause;
            _gameOverKen.enabled = State == Mousekeeping.GameState.GameOver;
            _gameOverText.enabled = State == Mousekeeping.GameState.GameOver;
            _restartText.enabled = State == Mousekeeping.GameState.GameOver;
        }
    }

    private float _seconds = 80;

    public float Seconds
    {
        get => _seconds;
        set
        {
            _seconds = value;
            _timeDisplay.text = String.Format("{0:00}", Seconds);
        }
    }
    
    [Header("Pickup")]
    [SerializeField] private GameObject _pickUpPrefab;
    [SerializeField] private Transform _pickUpParent;
    private GameObject _pickUp;
    [SerializeField] private Vector2 _pickUpSpawnBounds;
    [SerializeField, Range(0.0000001f, 0.01f)]
    private float _pickUpSpawnProbability = 0.0001f;
    private Vector2 _prefabSize;
    private KenBehavior _ken;
    [SerializeField, Range(1.0f, 10.0f)] private float _powerUpDuration = 5.0f;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("New instance initialized...");
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Duplicate instance found and deleted...");
        }
    }

    void Start()
    {
        State = Mousekeeping.GameState.Play;
        _pickUpSpawnBounds = new Vector2(Random.Range(-9f, 9f), Random.Range(-4f, 4f));
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape))
        {
            State = State == Mousekeeping.GameState.Play ?
                Mousekeeping.GameState.Pause :
                Mousekeeping.GameState.Play;
        }
        if (State == Mousekeeping.GameState.Play)
        {
            if (Seconds > 0)
            {
                Seconds -= Time.deltaTime;
            }
            else
            {
                State = Mousekeeping.GameState.GameOver;
            }
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                if (!_spawnMouse)
                {
                    StartCoroutine(SpawnRoutine());
                }
            }
            if (MouseDeath >= 11)
            {
                Destroy(_gate);
            }
        }
        if (State == Mousekeeping.GameState.Pause)
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                Application.Quit();
            }
        }
        if (State == Mousekeeping.GameState.GameOver)
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                Application.Quit();
            }
        }
        
        if (!_pickUp && Random.value < _pickUpSpawnProbability)
        {
            Vector2 position;
            do
            {
                position = new Vector2(
                    Random.Range(-_pickUpSpawnBounds.x, _pickUpSpawnBounds.x),
                    Random.Range(-_pickUpSpawnBounds.y, _pickUpSpawnBounds.y));
            } while (
                Physics2D.OverlapBox(position, _prefabSize, 0.0f) &&
                Physics2D.OverlapBox(position, _prefabSize, 45.0f));
                
            _pickUp = Instantiate(_pickUpPrefab, position, Quaternion.identity, _pickUpParent);
        }
    }
    
    IEnumerator SpawnRoutine()
    {
        _mousePosition = new Vector2(Random.Range(-9f, 9f), Random.Range(-4f, 4f));
        _spawnMouse = true;
        if (_mouseCount <= 10)
        {
            _mouse = Instantiate(_mousePrefab, _mousePosition, Quaternion.identity, _mouseParent);
            _mouseCount++;
        }
        yield return new WaitForSeconds(2);
        _spawnMouse = false;
    }
    
    public IEnumerator PowerUp()
    {
        if (_ken.CanShoot)
        {
            yield break;
        }

        _ken.CanShoot = true;
        yield return new WaitForSeconds(_powerUpDuration);
        _ken.CanShoot = false;
    }

    public void PowerStart()
    {
        StartCoroutine(PowerUp());
    }
}