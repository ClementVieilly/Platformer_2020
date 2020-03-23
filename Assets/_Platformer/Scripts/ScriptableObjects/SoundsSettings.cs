///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 11/02/2020 17:16
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer
{

	[CreateAssetMenu(
		menuName = "Platformer/Sound"
	)]

	public class SoundsSettings : ScriptableObject
	{

		[Header("Characters SD")]
		[SerializeField] private string _character_Spawn = null;
		[SerializeField] private string _character_Idle = null;
		[SerializeField] private string _character_IdleLong = null;
		[SerializeField] private string _character_Run = null;
		[SerializeField] private string _character_Jump = null;
		[SerializeField] private string _character_Planer = null;
		[SerializeField] private string _character_Fall = null;
		[SerializeField] private string _character_WallCatch = null;
		[SerializeField] private string _character_Death = null;

		[Header("Musics")]
		[SerializeField] private string _music_Menu = null;
		[SerializeField] private string _music_Level_1 = null;
		[SerializeField] private string _music_Level_2 = null;

		[Header("Ambiances")]
		[SerializeField] private string _ambiance_Level_1 = null;
		[SerializeField] private string _ambiance_Level_2 = null;

		[Header("UI")]
		[SerializeField] private string _ui_Button = null;
		[SerializeField] private string _ui_Start = null;
		[SerializeField] private string _ui_Pause = null;

		[Header("Collectibles")]
		[SerializeField] private string _collectible_Life = null;
		[SerializeField] private string _collectible_Score = null;
		[SerializeField] private string _collectible_BigScore_Lvl1 = null;
		[SerializeField] private string _collectible_BigScore_Lvl2 = null;

		[Header("Birds")]
		[SerializeField] private string _birds_dove = null;
		[SerializeField] private string _birds_owl = null;

		[Header("Platforms")]
		[SerializeField] private string _env_DestructiblePlatform_Glass = null ;
		[SerializeField] private string _env_DestructiblePlatform_Wood = null;
		[SerializeField] private string _env_Trigger_MobilePlatform= null;
		[SerializeField] private string _env_Trigger_TimedDoor = null;
		[SerializeField] private string _env_Time_DoorClosed = null;
		[SerializeField] private string _env_Time_DoorClosing = null;
		[SerializeField] private string _env_Time_DoorOpen = null;
		[SerializeField] private string _env_Mobile_Platform = null;

		[Header("Checkpoints")]
		[SerializeField] private string _checkpoint_Open = null;
		[SerializeField] private string _checkpoint_Closed = null;

		[Header("Props")]
		[SerializeField] private string _env_Spider_01_Idle = null;
		[SerializeField] private string _env_Spider_02_Idle = null;
		[SerializeField] private string _env_Cooking_Pot = null;
		[SerializeField] private string _env_electric_sparks = null;
		[SerializeField] private string _env_electric_arc = null;


		public string Character_Spawn { get => _character_Spawn; }
		public string Character_Idle { get => _character_Idle; }
		public string Character_IdleLong { get => _character_IdleLong; }
		public string Character_Run { get => _character_Run; }
		public string Character_Jump { get => _character_Jump; }
		public string Character_Planer { get => _character_Planer; }
		public string Character_Fall { get => _character_Fall; }
		public string Character_WallCatch { get => _character_WallCatch; }
		public string Character_Death { get => _character_Death; }
		public string Music_Menu { get => _music_Menu; }
		public string Music_Level_1 { get => _music_Level_1; }
		public string Music_Level_2 { get => _music_Level_2; }
		public string Ambiance_Level_1 { get => _ambiance_Level_1; }
		public string Ambiance_Level_2 { get => _ambiance_Level_2; }
		public string Ui_Button { get => _ui_Button; }
		public string Ui_Start { get => _ui_Start; }
		public string Ui_Pause { get => _ui_Pause; }
		public string Env_DestructiblePlatform_Glass { get => _env_DestructiblePlatform_Glass; }
		public string Env_DestructiblePlatform_Wood { get => _env_DestructiblePlatform_Wood; }
		public string Env_Cooking_Pot { get => _env_Cooking_Pot; }
		public string Env_Spider_01_Idle { get => _env_Spider_01_Idle; }
		public string Env_Spider_02_Idle { get => _env_Spider_02_Idle; }
		public string Collectible_Life { get => _collectible_Life; }
		public string Collectible_Score { get => _collectible_Score; }
		public string Birds_dove { get => _birds_dove; }
		public string Birds_owl { get => _birds_owl; }
		public string Env_Trigger_MobilePlatform { get => _env_Trigger_MobilePlatform; }
		public string Env_Trigger_TimedDoor { get => _env_Trigger_TimedDoor; }
		public string Env_Time_DoorClosed { get => _env_Time_DoorClosed; }
		public string Env_Time_DoorClosing { get => _env_Time_DoorClosing; }
		public string Env_Time_DoorOpen { get => _env_Time_DoorOpen; }
		public string Env_Mobile_Platform { get => _env_Mobile_Platform; }
		public string Checkpoint_Open { get => _checkpoint_Open; }
		public string Checkpoint_Closed { get => _checkpoint_Closed; }
		public string Collectible_BigScore_Lvl1 { get => _collectible_BigScore_Lvl1; }
		public string Collectible_BigScore_Lvl2 { get => _collectible_BigScore_Lvl2; }
		public string Env_electric_sparks { get => _env_electric_sparks; }
		public string Env_electric_arc { get => _env_electric_arc; }
	}
}