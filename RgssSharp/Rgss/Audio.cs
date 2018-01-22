using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;

namespace RgssSharp.Rgss
{
	[RubyModule("Audio", DefineIn = typeof(RubyModule))]
	public static class Audio
	{
		public const int MAX_SOUNDS = 16;

		private static Dictionary<int, RgssSound> _sounds;

		internal class RgssSound : IDisposable, IEquatable<RgssSound>
		{
			private bool _isDisposed;

			public SoundEffectInstance EffectInstance { get; set; }

			public SoundEffect Effect { get; set; }

			public string Filename { get; }

			public int Volume {  get; }

			public int Pitch { get; }

			public RgssSound(string filename, int volume, int pitch, bool loop)
			{
				Filename = filename;
				Volume = volume;
				Pitch = pitch;
				LoadSound(loop);
			}

			private void LoadSound(bool loop)
			{
				using (var stream = File.OpenRead(Filename))
				{
					Effect = SoundEffect.FromStream(stream);
					EffectInstance = Effect.CreateInstance();
					EffectInstance.Volume = Volume / 100.0f;
					EffectInstance.Pitch = ((Pitch - 100) * 2) / 100.0f;
					EffectInstance.IsLooped = loop;
				}
			}

			public bool IsSame(string filename, int volume, int pitch)
			{
				return !_isDisposed && filename == Filename && volume == Volume && pitch == Pitch;
			}

			public void Dispose()
			{
				if (_isDisposed)
					return;
				EffectInstance.Dispose();
				Effect.Dispose();
				_isDisposed = true;
			}

			public bool IsDisposed()
			{
				return _isDisposed;
			}

			public bool Equals(RgssSound other)
			{
				if (ReferenceEquals(null, other)) 
					return false;
				if (ReferenceEquals(this, other)) 
					return true;
				return string.Equals(Filename, other.Filename) && Volume == other.Volume && Pitch == other.Pitch;
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) 
					return false;
				if (ReferenceEquals(this, obj)) 
					return true;
				return obj.GetType() == GetType() && Equals((RgssSound) obj);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					var hashCode = (Filename != null ? Filename.GetHashCode() : 0);
					hashCode = (hashCode * 397) ^ Volume;
					hashCode = (hashCode * 397) ^ Pitch;
					return hashCode;
				}
			}
		}



		internal static void Initialize()
		{
			// Initialize with the keys so we don't have to check if it the key exists each time a sound is played
			_sounds = new Dictionary<int, RgssSound>(MAX_SOUNDS);
		 
			for (var i = 0; i < MAX_SOUNDS; i++)
				_sounds[i] = null;
		}

		internal static void Play(int index, string filename, int volume, int pitch, bool loop)
		{
			if (_sounds[index] != null)
			{

				if (!_sounds[index].IsSame(filename, volume, pitch))
					_sounds[index].Dispose();
				else
				{
					if (_sounds[index].EffectInstance.State != SoundState.Playing)
						_sounds[index].EffectInstance.Play();
					return;
				}
			}
			var sound = new RgssSound(filename, volume, pitch, loop);
			sound.EffectInstance.Play();
			_sounds[index] = sound;
		}

		internal static void Stop(int index)
		{
			if (_sounds[index] != null && !_sounds[index].IsDisposed())
				_sounds[index].EffectInstance.Stop();
		}

		internal static void Fade(int index, int time)
		{

		}

		[RubyMethod("bgm_play")]
		public static void BgmPlay(string filename, int volume, int pitch)
		{
			Play(0, filename, volume, pitch, true);
		}

		[RubyMethod("bgs_play")]
		public static void BgsPlay(string filename, int volume, int pitch)
		{
			Play(1, filename, volume, pitch, true);
		}

		[RubyMethod("me_play")]
		public static void MePlay(string filename, int volume, int pitch)
		{
			Play(2, filename, volume, pitch, false);
		}

		[RubyMethod("se_play")]
		public static void SePlay(string filename, int volume, int pitch)
		{
			Play(3, filename, volume, pitch, false);
		}

		[RubyMethod("bgm_stop")]
		public static void BgmStop()
		{
			Stop(0);
		}

		[RubyMethod("bgs_stop")]
		public static void BgsStop()
		{
			Stop(1);
		}

		[RubyMethod("me_stop")]
		public static void MeStop()
		{
			Stop(2);
		}

		[RubyMethod("se_stop")]
		public static void SeStop()
		{
			Stop(3);
		}

		[RubyMethod("bgm_fade")]
		public static void BgmFade(int time)
		{
			Fade(0, time);
		}

		[RubyMethod("bgs_fade")]
		public static void BgsFade(int time)
		{
			Fade(1, time);
		}

		[RubyMethod("me_fade")]
		public static void MeFade(int time)
		{
			Fade(2, time);
		}

		[RubyMethod("se_fade")]
		public static void SeFade(int time)
		{
			Fade(3, time);
		}
	}
}
