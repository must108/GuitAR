public class Lesson3 : Music {
    private static readonly string[] Lesson3Notes = {
        "C3", "D3", "E3", "F3", "G3",
        "A3", "B3", "C4", "B3", "A3",
        "G3", "F3", "E3", "D3", "C3"
    };

    public override string[] GetNotes() {
        return Lesson3Notes;
    }
}