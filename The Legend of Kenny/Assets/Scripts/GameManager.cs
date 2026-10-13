using UnityEngine;
using System.Collections;
using TMPro;

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
    private Mousekeeping.GameState _state;
    public Mousekeeping.GameState State
    {
        get => _state;

        set
        {
            _state = value;
            _pauseUI.enabled = State == Mousekeeping.GameState.Pause;
            _quitUI.enabled = State == Mousekeeping.GameState.Pause;
        }
    }
    
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
            if (!_spawnMouse)
            {
                StartCoroutine(SpawnRoutine());
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
                Debug.Log("Quitting game...");
            }
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
}