﻿using Godot;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class NodeExtensions
    {
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
                    ConnectBodyEntered with args failed with 
                        {result}
                    TryConnectSignal args
                    node:{node?.Name ?? "null"}
                    signal:{signal ?? "null"}
                    target:{target.ToString() ?? "null"}
                    methodName :{methodName ?? "null"}
                    -------------------------------------";

                    GD.PrintErr(MethodBase.GetCurrentMethod().Name, new ApplicationException(result.ToString()), message);
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
                else
                {
                    GD.Print($@"TryDisconnectSignal failed args
                    node:{node?.Name ?? "null"}
                    signal:{signal ?? "null"}
                    target:{target.ToString() ?? "null"}
                    methodName :{methodName ?? "null"}");
                    return false;
                }
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

        public static bool IsPlayer(this Node node)
        {
            var result = StringExtensions.Equals(node.Name, "player");
            return result;
        }

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

        public static T GetNode<T>(this Node node, string path) where T : Node
        {
            return (T)node.GetNode(path);
        }

        public static async Task WaitForSeconds(this Node node, float seconds)
        {
            try
            {
                await node.ToSignal(node.GetTree().CreateTimer(seconds), "timeout");
            }
            catch (System.Exception ex)
            {
                GD.PrintErr("WaitForSeconds threw", ex);
                throw;
            }
        }
    }
}