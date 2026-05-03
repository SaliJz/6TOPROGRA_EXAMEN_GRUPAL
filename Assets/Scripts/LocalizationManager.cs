
public enum Language 
{ 
    Spanish, 
    English 
}

public static class LocalizationManager
{
    public static Language CurrentLanguage { get; private set; } = Language.Spanish;

    public static void SetLanguage(Language lang) => CurrentLanguage = lang;

    public static string GetText(string es, string en) =>
        CurrentLanguage == Language.Spanish ? es : en;

    public static string GetText(LocalizedString str) => GetText(str.es, str.en);
}

[System.Serializable]
public struct LocalizedString
{
    public string es;
    public string en;
}