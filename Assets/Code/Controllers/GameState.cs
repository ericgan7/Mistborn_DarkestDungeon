using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState _instance;

    public static GameState Instance { get { return _instance; } }
    // State Variables to keep track of
    public GameController gc;
    public InputController ic;
    public UIController uic;
    //data loader

    public Team ally;
    public Team enemy;
    public Inventory inventory;
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        inventory = new Inventory();
    }
}
