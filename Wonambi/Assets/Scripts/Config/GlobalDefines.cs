using UnityEngine;

namespace GlobalDefines
{
	public class FilePath
	{
		public static readonly string MonsterData = "Config/MonsterData";
        public static readonly string TileBundlePath = "/tiles.assetbundle";
        public static readonly string ObjectBundlePath = "/object.assetbundle";
        public static readonly string LevelBundlePath = "/level.assetbundle";
	}

    public class DefineNumber
    {
        public static readonly int DefaultHP = 3;
        public static readonly float DefaultMoveSpeed = 3.0f;
        public static readonly float DefaultJumpSpeed = 10.0f;
        public static readonly float FallFactor = 1.0f;
        public static readonly float LowJumpFactor = 1.0f;
        public static readonly float BulletSpeed = 8.0f;
        public static readonly float BulletDuration = 1.0f;
        public static readonly float DieDuration = 0.8f;
        public static readonly float InvincibleDuration = 1.0f;
        public static readonly float HitBlinkDuration = 0.1f;
    }

    public class PrefsKey
    {
        public static readonly string PlayerMaxHP = "PlayerMaxHP";
        public static readonly string PlayerMoveSpeed = "PlayerMoveSpeed";
        public static readonly string PlayerJumpSpeed = "PlayerJumpSpeed";
        public static readonly string PlayerEnableDoubleJump = "PlayerEnableDoubleJump";
    }
}
