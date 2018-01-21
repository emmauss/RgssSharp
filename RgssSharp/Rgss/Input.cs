using Microsoft.Xna.Framework.Input;
using System.Linq;
using IronRuby.Runtime;
using Xna = Microsoft.Xna.Framework;

namespace RgssSharp.Rgss
{
	public static class Input
	{
		public const int NUMBER_PLAYERS = 0;

		private static KeyboardState[] _kbdStates;
		private static GamePadState[] _gpStates;


		internal static void Initialize()
		{
			_kbdStates = new KeyboardState[NUMBER_PLAYERS];
			_gpStates = new GamePadState[NUMBER_PLAYERS];
		}

		public static void Update()
		{
			for (var i = 0; i < NUMBER_PLAYERS; i++)
			{
				_kbdStates[i] = Keyboard.GetState((Xna.PlayerIndex) i);
				_gpStates[i] = GamePad.GetState((Xna.PlayerIndex) i);
			}
		}

		[RubyMethod("press?")]
		public static bool IsPressed(Keys key, int playerIndex)
		{
			return _kbdStates[playerIndex].GetPressedKeys().Contains(key);
		}

		public static GamePadState GetGamePadState(int playerIndex = 0)
		{
			return GamePad.GetState((Xna.PlayerIndex) playerIndex);
		}

		public static KeyboardState GetKeyboardState(int playerIndex = 0)
		{
			return Keyboard.GetState((Xna.PlayerIndex) playerIndex);
		}

		public static void SetVibration(float leftMotor, float rightMotor, int playerIndex = 0)
		{
			GamePad.SetVibration((Xna.PlayerIndex) playerIndex, leftMotor, rightMotor);
		}

		public static void StopVibration(int playerIndex = 0)
		{
			GamePad.SetVibration((Xna.PlayerIndex) playerIndex, 0.0f, 0.0f);
		}
	}
}
