
public class Lesson4 : Music {
    private static readonly string[] Lesson4Notes = {
        "G2", "A2", "B2", "C3", "D3", "E3",
        "F#3", "G3", "F#3", "E3", "D3", "C3",
        "B2", "A2", "G2"
    };

    public override string[] GetNotes() {
        return Lesson4Notes;
    }
}