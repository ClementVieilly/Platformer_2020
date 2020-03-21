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

		[Header("Birds")]
		//[SerializeField] private string _birds = null;

		[Header("Props")]
		[SerializeField] private string _props_Flaque_Toxique = null;
		[SerializeField] private string _props_DestructiblePlatform_Tuyaux_Verre = null;

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
		//public string Birds { get => _birds; }
		public string Props_Flaque_Toxique { get => _props_Flaque_Toxique; }
		public string Props_DestructiblePlatform_Tuyaux_Verre { get => _props_DestructiblePlatform_Tuyaux_Verre; }
		public string Collectible_Life { get => _collectible_Life; }
		public string Collectible_Score { get => _collectible_Score; }
	}
}