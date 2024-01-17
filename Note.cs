public abstract class Note {
    public string Name {get; set;}
    public Note() {
    }
    public Note(string Name) {
        this.Name=Name;
    }

    public abstract void Show();
    public abstract void Show(string filter);
}