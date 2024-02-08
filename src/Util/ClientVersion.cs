namespace sodoff.Util;
public class ClientVersion {
    public static bool IsOldSoD(string apiKey) {
        return (
            apiKey == "a1a06a0a-7c6e-4e9b-b0f7-22034d799013" ||
            apiKey == "a1a13a0a-7c6e-4e9b-b0f7-22034d799013" ||
            apiKey == "a2a09a0a-7c6e-4e9b-b0f7-22034d799013" ||
            apiKey == "a3a12a0a-7c6e-4e9b-b0f7-22034d799013"
        );
    }
    public static bool Use2013SoDTutorial(string apiKey) {
        return (
            apiKey == "a1a06a0a-7c6e-4e9b-b0f7-22034d799013" ||
            apiKey == "a1a13a0a-7c6e-4e9b-b0f7-22034d799013"
        );
    }
    public static bool Use2016SoDTutorial(string apiKey) {
        return (
            apiKey == "a2a09a0a-7c6e-4e9b-b0f7-22034d799013"
        );
    }
    public static bool Use2019SoDTutorial(string apiKey) {
        return (
            apiKey == "a3a12a0a-7c6e-4e9b-b0f7-22034d799013"
        );
    }
    public static bool Use2021SoDTutorial(string apiKey) {
        return !IsOldSoD(apiKey);
    }

    public static bool IsMaM(string apiKey) {
        return apiKey == "e20150cc-ff70-435c-90fd-341dc9161cc3";
    }
    
    public static bool IsWoJS(string apiKey) {
        return apiKey == "1552008f-4a95-46f5-80e2-58574da65875";
    }
    
    public static bool IsMB(string apiKey) {
        return apiKey == "6738196d-2a2c-4ef8-9b6e-1252c6ec7325";
    }
}
