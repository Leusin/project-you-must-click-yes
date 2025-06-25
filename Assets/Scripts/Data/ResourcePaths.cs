namespace ProjectYouMustClickYes
{
    public static class ResourcePaths
    {
        public const string MainAudioMixer = "MasterMixer";
        public const string MainVolume = "CustomCRTVolumeProfile";
        public const string BgmGroup = "BGM";

        public static string GetDialogue(Language lang)
        {
            return $"Data/dialogues_{lang.ToString()}";
        }
    }
}