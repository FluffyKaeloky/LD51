using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Pawn))]
[RequireComponent(typeof(FartManager))]
public class PlayerInput : InputBase
{
    public int playerIndex = 0;

    public string horizontalInputName = "Horizontal";
    public string verticalInputName = "Vertical";
    public string holdItInputName = "HoldIt";
    public string interactInputName = "Interact";



    private Player player = null;
    private Pawn pawn = null;
    private FartManager fartManager;
    public InteractableZone currentInteractZone;

    private float horizontalInput, verticalInput;

    private void Start()
    {
        player = ReInput.players.GetPlayer(playerIndex);
        pawn = GetComponent<Pawn>();
        fartManager = GetComponent<FartManager>();
    }

    private void Update()
    {
        horizontalInput = player.GetAxis(horizontalInputName);
        verticalInput = player.GetAxis(verticalInputName);

        if (player.GetButtonDown(holdItInputName))
        {
            fartManager.RefrainFart();
        }

        if (player.GetButtonDown(interactInputName) && currentInteractZone != null)
        {
            currentInteractZone.Interact();
        }

    }

    private void FixedUpdate()
    {
        Vector2 input = new Vector2(horizontalInput, verticalInput).normalized;

        pawn.Move(input.x, input.y);
        pawn.LookTo(new Vector2(input.x, input.y));
    }
}
