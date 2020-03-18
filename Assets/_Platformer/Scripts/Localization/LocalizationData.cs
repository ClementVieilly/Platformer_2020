///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 12/03/2020 12:15
///-----------------------------------------------------------------


namespace Com.IsartDigital.Platformer.Localization {
    [System.Serializable]
	public class LocalizationData {

        public Localizationitem[] items; 
	}

    [System.Serializable]
    public class Localizationitem
    {
        public string key; 
        public string value; 
    }
}