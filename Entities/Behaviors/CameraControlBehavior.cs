using System;
using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Libraries;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Behaviors;

public class CameraControlBehavior : Camera2D, IDebuggable<Node>, IMovableCamera
{
    private ILogger _logger;
    [Export] public bool IsDebugging { get; set; }

    [Export] public float DefaultZoomLevel { get; set; } = 1.0f;
    [Export] public Vector2 DefaultPan { get; set; } = new Vector2(0, 0);
    [Export] public float MinZoom { get; set; } = 0.5f;
    [Export] public float MaxZoom { get; set; } = 2f;

    [Export] public float ZoomFactor { get; set; } = 0.1f;
    [Export] public float ZoomDuration { get; set; } = 0.2f;

    [Export] public float ZoomLevel { get; set; } = 1.0f;

    [Export] public Vector2 MaxPanRight { get; set; } = new Vector2(100, 0);
    [Export] public Vector2 MaxPanLeft { get; set; } = new Vector2(-100, 0);
    
    [Export] public Vector2 MaxPanUp { get; set; } = new Vector2(0, -100);
    
    [Export] public Vector2 MaxPanDown { get; set; } = new Vector2(0, 100);

    private Tween TweenUtil { get; set; }

    private Rect2 ScreenSize { get; set; } = new Rect2(0, 0, 0, 0);

    public override void _Ready()
    {
        this._logger = new GDLogger(level: LogLevelOutput.Debug);
        this.ScreenSize = GetViewportRect();
        this.TweenUtil = GetNode<Tween>("Tween");
    }

    public void SetZoom(float amount)
    {
        _logger.Debug("Setting zoom to: " + amount);
        const string zoomProperty = "zoom";
        ZoomLevel = Mathf.Clamp(amount, MinZoom, MaxZoom);
        var endZoomLevel = new Vector2(ZoomLevel, ZoomLevel);

        TweenUtil.InterpolateProperty(
            this,
            zoomProperty,
            Zoom,
            endZoomLevel,
            ZoomDuration,
            Godot.Tween.TransitionType.Sine,
            Godot.Tween.EaseType.Out
        );
        TweenUtil.Start();
    }

    public void SetPan(Vector2 newOffset)
    {
        _logger.Debug("Setting pan offset to: " + newOffset);
        const string offsetProperty = "offset";
        //Offset = Mathf.Clamp(offset, -MaxPan, MaxPan);
        var endZoomLevel = new Vector2(ZoomLevel, ZoomLevel);

        TweenUtil.InterpolateProperty(
            this,
            offsetProperty,
            Offset,
            newOffset,
            ZoomDuration,
            Godot.Tween.TransitionType.Sine,
            Godot.Tween.EaseType.Out
        );
        TweenUtil.Start();
    }

    public void ResetCamera()
    {
        _logger.Debug("Camera should be reset");
        SetZoom(DefaultZoomLevel);
        SetPan(DefaultPan);
    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        //inputEvent.GetActionStrength(InputAction.CameraUp) > 0 ? SetZoom(ZoomLevel+ZoomFactor) : 0;
        var upStrength = inputEvent.GetActionStrength(InputAction.CameraUp);
        var downStrength = inputEvent.GetActionStrength(InputAction.CameraDown);
        var leftStrength = inputEvent.GetActionStrength(InputAction.CameraLeft);
        var rightStrength = inputEvent.GetActionStrength(InputAction.CameraRight);
        
        
        
        _logger.Debug("downStrength = " + downStrength + "|\r\n| upStrength=" + upStrength +"leftStrength=" + leftStrength + "|\r\n| rightStrength=" + rightStrength);
        if(leftStrength> 0.2f)
        {
            var newOffset = MaxPanLeft * leftStrength;
            SetPan(newOffset);
        }
        if(rightStrength> 0.2f)
        {
            var newOffset = MaxPanRight * rightStrength;
            SetPan(newOffset);
        }
        if (upStrength > 0.2f)
        {
            //ZoomLevel = MaxZoom * upStrength;
            //SetZoom(ZoomLevel);
            var newOffset = MaxPanUp * upStrength;
            SetPan(newOffset);
        }

        if (downStrength > 0.2f)
        {
            //ZoomLevel = MaxZoom * -downStrength;
            //SetZoom(ZoomLevel);
            var newOffset = MaxPanDown * downStrength;
            SetPan(newOffset);
        }
        
        //if (downStrength - upStrength == 0f && leftStrength-rightStrength==0f)
        if (inputEvent.IsActionPressed(InputAction.CameraReset))
        {
            ResetCamera();
        }
    }
}