// Minuet in G - we have the sheet music!

public class Song2 : Music {
    private static readonly string[] Song2Notes = {
        "D4", "G3", "A3", "B3", "C4",
        "D4", "G3", "G3", "E4", "C4",
        "D4", "E4", "F#4", "G4", "G3",
        "G3", "C4", "D4", "C4", "B3",
        "A3", "B3", "C4", "B3", "A3",
        "G3", "F#3", "G3", "A3", "B3",
        "G3", "B3", "A3", "D4", "G3",
        "A3", "B3", "C4", "D4", "G3",
        "G3", "E4", "C4", "D4", "E4",
        "F#4", "G4", "G3", "G3", "C4",
        "D4", "C4", "B3", "A3", "B3",
        "C4", "B3", "A3", "G3", "A3",
        "B3", "A3", "G3", "F#3", "G3"   
    };

    public override string[] GetNotes() {
        return Song2Notes;
    }
}