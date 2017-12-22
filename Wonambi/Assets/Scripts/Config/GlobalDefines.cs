using UnityEngine;

namespace GlobalDefines
{
	public static class FilePath
	{
        public static readonly string TileBundlePath = "/tiles.assetbundle";
        public static readonly string ObjectBundlePath = "/object.assetbundle";
        public static readonly string LevelBundlePath = "/level.assetbundle";
        public static readonly string ConfigBundlePath = "/config.assetbundle";

        public static readonly string ColorToPrefabFile = "ColorToPrefab";
	}

    public static class DefineNumber
    {
        public static readonly int DefaultHP = 3;
        public static readonly float DefaultMoveSpeed = 3.0f;
        public static readonly float DefaultJumpSpeed = 10.0f;
        public static readonly float FallFactor = 1.0f;
        public static readonly float LowJumpFactor = 1.0f;
        public static readonly float BulletSpeed = 10.0f;
        public static readonly float BulletDuration = 1.0f;
        public static readonly float FireCooldown = 0.3f;
        public static readonly float DieDuration = 0.8f;
        public static readonly float InvincibleDuration = 1.0f;
        public static readonly float HitBlinkDuration = 0.05f;
        public static readonly float CloseTurnDistance = 0.1f;
        public static readonly float MonsterMoveTriggerDistance = 20.0f;
        public static readonly float MonsterFireTriggerDistance = 8.0f;
        public static readonly float MaxFallSpeed = -20.0f;
        public static readonly float PlayerMinY = -10.0f;
    }

    public static class PrefsKey
    {
        public static readonly string PlayerMaxHP = "PlayerMaxHP";
        public static readonly string PlayerMoveSpeed = "PlayerMoveSpeed";
        public static readonly string PlayerJumpSpeed = "PlayerJumpSpeed";
        public static readonly string PlayerEnableDoubleJump = "PlayerEnableDoubleJump";
        public static readonly string PlayerSavePoint = "PlayerSavePoint";
        public static readonly string PlayerLevelMap = "PlayerLevelMap";
    }

    public enum MoveDirection 
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public static class GlobalFunc
    {
        public static Vector3 StringToVector3(string s)
        {
            if(s.StartsWith("(") && s.EndsWith(")")) {
                s = s.Substring(1, s.Length - 2);
            }
            string[] sArray = s.Split(',');

            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }
    }
}
