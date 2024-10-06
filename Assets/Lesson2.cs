public class Lesson2 : Music { 
    private static readonly string[] Lesson2Notes = {
        "D3", "D#3", "E3", "F3", "F#3",
        "A2", "A#2", "B2", "C3", "C#3",
        "E2", "F2", "F#2", "G2", "G#2"
    };

    public override string[] GetNotes() {
        return Lesson2Notes;
    }
}