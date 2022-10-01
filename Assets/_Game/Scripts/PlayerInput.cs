using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Pawn))]
public class PlayerInput : InputBase
{
    public int playerIndex = 0;

    public string horizontalInputName = "Horizontal";
    public string verticalInputName = "Vertical";

    private Player player = null;
    private Pawn pawn = null;

    private float horizontalInput, verticalInput;

    private void Start()
    {
        player = ReInput.players.GetPlayer(playerIndex);
        pawn = GetComponent<Pawn>();
    }

    private void Update()
    {
        horizontalInput = player.GetAxis(horizontalInputName);
        verticalInput = player.GetAxis(verticalInputName);
    }

    private void FixedUpdate()
    {
        pawn.Move(horizontalInput, verticalInput);
    }
}
