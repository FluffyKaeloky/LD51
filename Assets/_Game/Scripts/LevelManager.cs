using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } = null;

    public PlayerInput PlayerInput { get => playerInput; }
    [SerializeField]
    private PlayerInput playerInput = null;


    private void Awake()
    {
        Instance = this;
    }

    public void Win()
    {
        print("gg you win");
    }

    public void Loose()
    {
        print("Oh no you loos");
    }
}
