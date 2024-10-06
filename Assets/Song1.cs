// Red River Valley - we have the sheet music!

public class Song1 : Music { 
    private static readonly string[] Song1Notes = {
        "D3", "G3", "B3", "B3", "A3", "G3", 
        "A3", "G3", "E3", "G3", "D3", "G3",
        "B3", "G3", "B3", "D4", "C4", "B3",
        "A3", "D4", "C4", "B3", "B3", "A3",
        "G3", "A3", "B3", "D4", "C4", "E3",
        "E3", "D3", "F#3", "G3", "A3", "B3",
        "A3", "G3" 
    };

    public override string[] GetNotes() {
        return Song1Notes;
    }
 }