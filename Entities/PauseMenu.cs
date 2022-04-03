using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;

public class PauseMenu : Control
{
    [Export]
    private string InventoryDisplayPath { get; set; } = "./InventoryPanel/InventoryDisplay";

    [Export]
    private string MissionManagerDisplayPath { get; set; } = "./MissionPanel/MissionDisplay";

    [Export]
    private string TitleDisplayPath { get; set; } = "./TitlePanel/Title";

    public Label InventoryDisplay { get; set; }
    public Label MissionDisplay { get; set; }
    public Label TitleDisplay { get; set; }

    public bool IsHidden = false;

    [Export]
    private float PauseToggleCooldownWaitTime = 1.0f;

    private float AccumulatorPauseToggleCooldown = 0.0f;
    private bool CanTogglePause = true;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TitleDisplay = GetNode<Label>(TitleDisplayPath);
        InventoryDisplay = GetNode<Label>(InventoryDisplayPath);
        MissionDisplay = GetNode<Label>(MissionManagerDisplayPath);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed(InputAction.Pause))
        {
            if (CanTogglePause)
            {
                GD.Print("Can toggle pause yet, time left ");
                CanTogglePause = false;
                TogglePauseMenu();
                AccumulatorPauseToggleCooldown = PauseToggleCooldownWaitTime;
            }
            else
            {
                GD.Print("Cannot toggle pause yet, time left ", AccumulatorPauseToggleCooldown);
            }
        }
        if (AccumulatorPauseToggleCooldown > 0)
        {
            AccumulatorPauseToggleCooldown -= delta;
        }
        else
        {
            CanTogglePause = true;
        }
    }

    private void TogglePauseMenu()
    {
        GD.Print("Toggling Pause Menu");
        this.TogglePause();
        if (this.IsPaused())
        {
            GD.Print("Should be showing Pause Menu");
            Show();
        }
        else
        {
            GD.Print("Should be hiding Pause Menu");
            Hide();
        }
    }

    public void RefreshUI(Player player)
    {
        InventoryDisplay.Text = player.Inventory.Display();
        MissionDisplay.Text = player.MissionManager.Display();
    }
}