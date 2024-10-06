
public class Lesson1 : Music {
    private static readonly string[] Lesson1Notes = {
        "E4", "F4", "F#4", "G4", "G#4",
        "B3", "C4", "C#4", "D4", "D#4",
        "G3", "G#3", "A3"
    };

    public override string[] GetNotes() {
        return Lesson1Notes;
    }
};