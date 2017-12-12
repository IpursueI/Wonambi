﻿using UnityEngine;

namespace GlobalDefines
{
	public class FilePath
	{
		public static readonly string MonsterData = "Config/MonsterData";
	}

    public class DefineNumber
    {
        public static readonly int DefaultHP = 3;
        public static readonly float DefaultMoveSpeed = 3.0f;
        public static readonly float DefaultJumpSpeed = 10.0f;

        public static readonly float FallFactor = 0.5f;
        public static readonly float LowJumpFactor = 1.0f;
    }

    public class PrefsKey
    {
        public static readonly string PlayerMaxHP = "PlayerMaxHP";
        public static readonly string PlayerMoveSpeed = "PlayerMoveSpeed";
        public static readonly string PlayerJumpSpeed = "PlayerJumpSpeed";
        public static readonly string PlayerEnableDoubleJump = "PlayerEnableDoubleJump";
    }
}
