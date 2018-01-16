using UnityEngine;

namespace GlobalDefines
{
	public static class FilePath
	{
        public static readonly string ObjectBundlePath = "/object.assetbundle";
        public static readonly string LevelBundlePath = "/level.assetbundle";
        public static readonly string ConfigBundlePath = "/config.assetbundle";
	}

    public static class DefineString
    {
        public static readonly string FirstLevel = "Level11";
    }

    public static class DefineNumber
    {
        public static readonly int DefaultHP = 3;
        public static readonly float DefaultMoveSpeed = 3.0f;
        public static readonly float DefaultJumpSpeed = 10.0f;
        public static readonly int DefaultBulletNumber = 3;
        public static readonly float FallFactor = 1.0f;
        public static readonly float LowJumpFactor = 1.0f;
        public static readonly float BulletSpeed = 9.0f;
        public static readonly float BulletDuration = 8.0f;
        public static readonly float FireCooldown = 0.15f;
        public static readonly float DieDuration = 0.8f;
        public static readonly float InvincibleDuration = 1.0f;
        public static readonly float HitBlinkDuration = 0.1f;
        public static readonly float CloseTurnDistance = 0.1f;
        public static readonly float MonsterMoveTriggerDistanceX = 12.0f;
        public static readonly float MonsterMoveTriggerDistanceY = 6.0f;
        public static readonly float MonsterFireTriggerDistanceX = 6.0f;
        public static readonly float MonsterFireTriggerDistanceY = 3.0f;
        public static readonly float MaxFallSpeed = -20.0f;
        public static readonly float PlayerMinY = -8.0f;
        public static readonly float CameraOffsetX = 10.5f;
        public static readonly float CameraOffsetY = 5.4f;
    }

    public static class PrefsKey
    {
        public static readonly string PlayerMaxHP = "PlayerMaxHP";
        public static readonly string PlayerMoveSpeed = "PlayerMoveSpeed";
        public static readonly string PlayerJumpSpeed = "PlayerJumpSpeed";
        public static readonly string PlayerEnableDoubleJump = "PlayerEnableDoubleJump";
        public static readonly string PlayerBulletNumber = "PlayerBulletNumber";

        public static readonly string SavePoint = "SavePoint";
        public static readonly string LevelMap = "LevelMap";

        public static readonly string LevelDoubleJump = "LevelDoubleJump";
        public static readonly string LevelExtraBullet = "LevelExtraBullet";
        public static readonly string LevelBinaryDoor = "LevelBinaryDoor";
    }

    public static class PlayerStatus
    {
        public static readonly int InBonfire = 0x00000001;
    }

    public enum MoveDirection 
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public enum TipsType
    {
        Move = 1,
        Jump = 2,
        Shoot = 3
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
