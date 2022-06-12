using Godot;

namespace ThemedHorrorJam5.Scripts.SteeringAIFramework
{
    public static class GSAI
    {
        public static class Behaviors
        {
            private const string Root = "res://addons/com.gdquest.godot-steering-ai-framework/Behaviors/";

            public static GDScript GSAIArrive()
            {
                return (GDScript)GD.Load($"{Root}GSAIArrive.gd");
            }
            public static GDScript GSAIAvoidCollisions()
            {
                return (GDScript)GD.Load($"{Root}GSAIAvoidCollisions.gd");
            }
            public static GDScript GSAIBlend()
            {
                return (GDScript)GD.Load($"{Root}GSAIBlend.gd");
            }
            public static GDScript GSAICohesion()
            {
                return (GDScript)GD.Load($"{Root}GSAICohesion.gd");
            }
            public static GDScript GSAIEvade()
            {
                return (GDScript)GD.Load($"{Root}GSAIEvade.gd");
            }
            public static GDScript GSAIFace()
            {
                return (GDScript)GD.Load($"{Root}GSAIFace.gd");
            }
            public static GDScript GSAIFlee()
            {
                return (GDScript)GD.Load($"{Root}GSAIFlee.gd");
            }
            public static GDScript GSAIFollowPath()
            {
                return (GDScript)GD.Load($"{Root}GSAIFollowPath.gd");
            }
            public static GDScript GSAILookWhereYouGo()
            {
                return (GDScript)GD.Load($"{Root}GSAILookWhereYouGo.gd");
            }
            public static GDScript GSAIMatchOrientation()
            {
                return (GDScript)GD.Load($"{Root}GSAIMatchOrientation.gd");
            }
            public static GDScript GSAIPriority()
            {
                return (GDScript)GD.Load($"{Root}GSAIPriority.gd");
            }
            public static GDScript GSAIPursue()
            {
                return (GDScript)GD.Load($"{Root}GSAIPursue.gd");
            }
            public static GDScript GSAISeek()
            {
                return (GDScript)GD.Load($"{Root}GSAISeek.gd");
            }
            public static GDScript GSAISeparation()
            {
                return (GDScript)GD.Load($"{Root}GSAISeparation.gd");
            }
        }

        public static class Agents
        {
            private const string Root = "res://addons/com.gdquest.godot-steering-ai-framework/Agents/";

            public static GDScript GSAIKinematicBody2DAgent()
            {
                return (GDScript)GD.Load($"{Root}GSAIKinematicBody2DAgent.gd");
            }
            public static GDScript GSAIKinematicBody3DAgent()
            {
                return (GDScript)GD.Load($"{Root}GSAIKinematicBody3DAgent.gd");
            }
            public static GDScript GSAIRigidBody2DAgent()
            {
                return (GDScript)GD.Load($"{Root}GSAIRigidBody2DAgent.gd");
            }
            public static GDScript GSAIRigidBody3DAgent()
            {
                return (GDScript)GD.Load($"{Root}GSAIRigidBody3DAgent.gd");
            }
            public static GDScript GSAISpecializedAgent()
            {
                return (GDScript)GD.Load($"{Root}GSAISpecializedAgent.gd");
            }
        }

        public static class Proximities
        {
            private const string Root = "res://addons/com.gdquest.godot-steering-ai-framework/Proximities/";

            public static GDScript GSAIInfiniteProximity()
            {
                return (GDScript)GD.Load($"{Root}GSAIInfiniteProximity.gd");
            }
            public static GDScript GSAIProximity()
            {
                return (GDScript)GD.Load($"{Root}GSAIProximity.gd");
            }
            public static GDScript GSAIRadiusProximity()
            {
                return (GDScript)GD.Load($"{Root}GSAIRadiusProximity.gd");
            }

        }

        private const string Root = "res://addons/com.gdquest.godot-steering-ai-framework/";

        public static GDScript GSAIAgentLocation()
        {
            return (GDScript)GD.Load($"{Root}GSAIAgentLocation.gd");
        }
        public static GDScript GSAIGroupBehavior()
        {
            return (GDScript)GD.Load($"{Root}GSAIGroupBehavior.gd");
        }
        public static GDScript GSAIPath()
        {
            return (GDScript)GD.Load($"{Root}GSAIPath.gd");
        }
        public static GDScript GSAISteeringAgent()
        {
            return (GDScript)GD.Load($"{Root}GSAISteeringAgent.gd");
        }
        public static GDScript GSAISteeringBehavior()
        {
            return (GDScript)GD.Load($"{Root}GSAISteeringBehavior.gd");
        }
        public static GDScript GSAITargetAcceleration()
        {
            return (GDScript)GD.Load($"{Root}GSAITargetAcceleration.gd");
        }
        public static GDScript GSAIUtils()
        {
            return (GDScript)GD.Load($"{Root}GSAIUtils.gd");
        }


        public static GDScript GSAIBlend()
        {
            var agent = (GDScript)GD.Load($"{Root}Behaviors/GSAIBlend.gd");
            return agent;
        }

        public static GDScript GSAIPriority()
        {
            var agent = (GDScript)GD.Load($"{Root}GSAISteeringAgent.gd");
            return agent;
        }
        public static GDScript GSAIPursue()
        {
            var agent = (GDScript)GD.Load($"{Root}GSAISteeringAgent.gd");
            return agent;
        }
        public static GDScript GSAIFlee()
        {
            var agent = (GDScript)GD.Load($"{Root}GSAISteeringAgent.gd");
            return agent;
        }
        public static GDScript GSAIAvoidCollisions()
        {
            var agent = (GDScript)GD.Load($"{Root}GSAISteeringAgent.gd");
            return agent;
        }
        public static GDScript GSAIFace()
        {
            var agent = (GDScript)GD.Load($"{Root}GSAISteeringAgent.gd");
            return agent;
        }
        public static GDScript GSAILookWhereYouGo()
        {
            var agent = (GDScript)GD.Load($"{Root}GSAISteeringAgent.gd");
            return agent;
        }
    }
}