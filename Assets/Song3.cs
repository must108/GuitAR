// Twinkle Twinkle Little Star - musescore

public class Song3 : Music {
    private static readonly string[] Song3Notes = {
        "G3", "G3", "D4", "D4", "E4", "E4",
        "D4", "C4", "C4", "B3", "B3", "A3",
        "A3", "G3", "D4", "D4", "C4", "C4",
        "B3", "B3", "A3", "D4", "D4", "C4",
        "C4", "B3", "B3", "A3", "G3", "G3",
        "D4", "D4", "E4", "E4", "D4", "C4",
        "C4", "B3", "B3", "A3", "A3", "G3"
    };

    public override string[] GetNotes() {
        return Song3Notes;
    }
}