using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rewired;
using Rewired.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BSCore
{
	public class BSCoreInput
	{
		private static readonly Dictionary<Option, DataStoreFloat> _axisSensitivities = new Dictionary<Option, DataStoreFloat>();

		private static readonly Dictionary<Option, DataStoreBool> _axisInversions = new Dictionary<Option, DataStoreBool>();

		public static bool Enabled = true;

		private static BSCoreInput _instance;

		private int _playerId;

		private Player _player;

		public static bool UsingJoystick { get; private set; }

		public static Player Player
		{
			get
			{
				if (_instance == null)
				{
					_instance = new BSCoreInput();
				}
				return _instance._player;
			}
		}

		public static void RegisterAxisSensitivity(Option option, DataStoreFloat dataStore)
		{
			_axisSensitivities.Add(option, dataStore);
		}

		public static void RegisterAxisInversion(Option option, DataStoreBool dataStore)
		{
			_axisInversions.Add(option, dataStore);
		}

		public static float GetAxis(Option option)
		{
			float num = Player.GetAxis((int)option);
			if (_axisSensitivities.ContainsKey(option))
			{
				num *= Mathf.Lerp(0.5f, 1.5f, _axisSensitivities[option].Value);
			}
			if (_axisInversions.ContainsKey(option))
			{
				num *= (_axisInversions[option].Value ? (-1f) : 1f);
			}
			return num;
		}

		public static float GetAxis(int action)
		{
			return Player.GetAxis(action);
		}

		public static bool GetButtonDown(Option option, bool notOverUI = true)
		{
			if (!notOverUI || !EventSystem.current.IsPointerOverGameObject())
			{
				return Player.GetButtonDown((int)option);
			}
			return false;
		}

		public static bool GetButtonDown(int action)
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				return Player.GetButtonDown(action);
			}
			return false;
		}

		public static bool GetButton(Option option)
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				return Player.GetButton((int)option);
			}
			return false;
		}

		public static bool GetButtonUp(Option option)
		{
			return Player.GetButtonUp((int)option);
		}

		public static void ValidateMappings()
		{
			PlayerSaveData saveData = Player.GetSaveData(false);
			List<string> fields = GetFields(typeof(Option));
			List<string> list = new List<string>();
			foreach (ControllerMapSaveData allControllerMapSaveDatum in saveData.AllControllerMapSaveData)
			{
				if (list.Count >= fields.Count)
				{
					break;
				}
				foreach (string item in fields)
				{
					if (allControllerMapSaveDatum.map.ContainsAction(item))
					{
						list.Add(item);
					}
				}
			}
			if (list.Count < fields.Count)
			{
				Debug.LogError(string.Format("{0} options are not mapped in saved data. Resetting the mappings.", fields.Count - list.Count));
				ResetToDefault();
			}
		}

		private static List<string> GetFields(Type type)
		{
			return (from f in type.GetFields(BindingFlags.Static | BindingFlags.Public)
				where f.FieldType == typeof(int)
				select f.Name).ToList();
		}

		public static void ResetToDefault()
		{
			UserDataStore_PlayerPrefs userDataStore_PlayerPrefs = UnityEngine.Object.FindObjectOfType<UserDataStore_PlayerPrefs>();
			_instance._player.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
			_instance._player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
			_instance._player.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
			userDataStore_PlayerPrefs.Save();
		}

		public BSCoreInput()
		{
			_player = ReInput.players.GetPlayer(_playerId);
			Controller lastActiveController = _player.controllers.GetLastActiveController();
			if (lastActiveController != null)
			{
				UsingJoystick = lastActiveController.type == ControllerType.Joystick;
			}
			_player.controllers.AddLastActiveControllerChangedDelegate(OnActiveControllerChanged);
		}

		private void OnActiveControllerChanged(Player player, Controller controller)
		{
			UsingJoystick = controller.type == ControllerType.Joystick;
		}
	}
}
