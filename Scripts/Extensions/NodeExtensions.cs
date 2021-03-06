using System;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using TinyIoC;

namespace ThemedHorrorJam5.Scripts.Extensions
{
    public static class NodeExtensions
    {
        public static Entities.Global GetGlobal(this Node node)
        {
            var temp = node.GetNode("/root/Global");
            return (Entities.Global)temp;
        }

        public static TinyIoCContainer GetContainer(this Node node) => node.GetGlobal().Container;
        

        public static void DrawCircleArc(this Node2D node , Vector2 center, float radius, float angleFrom, float angleTo, Color color)
        {
            int nbPoints = 32;
            var pointsArc = new Vector2[nbPoints + 1];

            for (int i = 0; i <= nbPoints; i++)
            {
                float anglePoint = Mathf.Deg2Rad(angleFrom + (i * (angleTo - angleFrom) / nbPoints) - 90f);
                pointsArc[i] = center + (new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius);
            }

            for (int i = 0; i < nbPoints - 1; i++)
            {
                node.DrawLine(pointsArc[i], pointsArc[i + 1], color);
            }
        }

        public static bool TryConnectSignal(this Node node, string signal, Godot.Object target, string methodName)
        {
            Error result;
            try
            {
                result = node.Connect(signal, target, methodName);
                if (result != Error.Ok)
                {
                    var message =
                    $@"-------------------------------------
                    ConnectBodyEntered with args failed with {result.ToString()}
                    TryConnectSignal args
                    node:{node.Name ?? "null"}
                    signal:{signal ?? "null"}
                    target:{target.ToString() ?? "null"}
                    methodName :{methodName ?? "null"}
                    -------------------------------------";

                    GD.PrintErr(MethodBase.GetCurrentMethod()?.Name, new ApplicationException(result.ToString()), message);
                    GD.PrintStack();
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr(ex);
                GD.Print($@"TryConnectSignal args
                    node:{node?.Name ?? "null"}
                    signal:{signal ?? "null"}
                    target:{target.ToString() ?? "null"}
                    methodName :{methodName ?? "null"}");
                throw;
            }
            return result == Error.Ok;
        }

        public static bool TryDisconnectSignal(this Node node, string signal, Godot.Object target, string methodName)
        {
            try
            {
                if (node.HasSignal(signal))
                {
                    node.Disconnect(signal, target, methodName);
                    return true;
                }
                
                GD.Print($@"TryDisconnectSignal failed args
                node:{node.Name ?? "null"}
                signal:{signal ?? "null"}
                target:{target.ToString() ?? "null"}
                methodName :{methodName ?? "null"}");
                return false;
                
            }
            catch (Exception ex)
            {
                GD.Print(ex);
                GD.Print($@"TryDisconnectSignal args
                    node:{node?.Name ?? "null"}
                    signal:{signal ?? "null"}
                    target:{target.ToString() ?? "null"}
                    methodName :{methodName ?? "null"}");
                return false;
            }
        }

        public static bool IsPlayer(this Node node) => node.Name.ToLower().Contains("player");

        public static void Pause(this Node node)
        {
            node.GetTree().Paused = true;
        }

        public static bool IsPaused(this Node node) => node.GetTree().Paused;

        public static void Unpause(this Node node)
        {
            node.GetTree().Paused = false;
        }

        public static void TogglePause(this Node node)
        {
            node.GetTree().Paused = (!node.GetTree().Paused);
        }

        public static async Task WaitForSeconds(this Node node, float seconds)
        {
            try
            {
                await node.ToSignal(node.GetTree().CreateTimer(seconds), "timeout");
            }
            catch (Exception ex)
            {
                GD.PrintErr("WaitForSeconds threw", ex);
                throw;
            }
        }

        public static string DisplayPositionData(this Node2D node, string title) =>
        @$"
        |-----------------------------------------------------------
        | {title} Global Position: {node.GlobalPosition.ToString()}
        | {title} Local Position: {node.Position.ToString()}
        |-----------------------------------------------------------";
        
    }
}