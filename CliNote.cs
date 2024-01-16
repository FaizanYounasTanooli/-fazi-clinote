using System.ComponentModel.DataAnnotations;
using System.Text;
using System.IO;
using Newtonsoft.Json;
//using Newtonsoft.Json;

public class CliNote
{
    List<Note> Notes = new List<Note>();
    string FilePath = @"D:\Notes.txt";
    //List<TableNote> TableNotes = new List<TableNote>();
    public CliNote()
    {

        LoadNotes();
    }
    bool IsCmdOk(string cmd, string abriviation, string name)
    {
        return (cmd.ToLower() == abriviation.ToLower() || cmd.ToLower() == name.ToLower());
    }
    public void ExecuteCommand(string[] commnad)
    {
        if (commnad.Length > 0)
        {
            var cmd = commnad[0];
            var args = commnad.Skip(1).ToArray();
            if (IsCmdOk(cmd, "ctn", "CreateTableNote"))
            {
                CreateTable(args);
            }
            else if (IsCmdOk(cmd, "cnn", "CreateNormalNote"))
            {
                CreateNormal(args);
            }
            else if (IsCmdOk(cmd, "tn", "TakeNote"))
            {
                TakeNote(args);
            }
            else if (IsCmdOk(cmd, "sn", "ShowNote"))
            {
                ShowNote(args[0]);
            }
            else
            {
                Console.WriteLine("Command not recognized");
            }
        }
        else
        {
            Console.WriteLine("This is a CLI notes application built by Faizan Younas Tanooli");
        }
    }
    void ShowNote(string name)
    {
        var NoteToShow = Notes.Find(note => note.Name == name);
        if (NoteToShow != null)
        {
            NoteToShow.Show();
        }
        else
        {
            Console.WriteLine("Note not found with name:" + name);
        }
    }
    void TakeNote(string[] args) 
    {
        if (args.Length==3)
        {
            if (IsCmdOk(args[0],"-t","-Table"))
            {
                string Name=args[1];
                string Row=args[2];
                var CurrentNote = (TableNote)Notes.Find(note=>note.Name==Name);
                if (CurrentNote !=null)
                {
                    CurrentNote.AddRow(Row);
                    
                }
                else {
                    Console.WriteLine("Note Not found with name: "+Name);
                }
            }
            else {
                Console.WriteLine("Invalid Commnad");
            }
        }
        else {
            Console.WriteLine("Invalid number of arguments given");
        }
        SaveNotes();
    }
    void CreateNormal(string[] args)
    {
        if (args.Length > 0)
        {
            var cmd = args[0];
            var subArgs = args.Skip(1).ToArray();
            if (IsCmdOk(cmd, "t", "CreateTableNote"))
            {
                CreateTable(args);
            }
            
        }
        else
        {
            Console.WriteLine("This is a CLI notes application built by Faizan Younas Tanooli");
        }

    }
    static bool ValidateFlags(string[] expectedStrings, string[] actualArray)
    {
        if (expectedStrings.Length != actualArray.Length)
        {
            Console.WriteLine("Invalid number of aruguments given.");
            return false;
        }

        // Validation HashSet to track unique strings
        var validationHashSet = new HashSet<string>(expectedStrings);

        for (int i = 0; i < actualArray.Length; i++)
        {
            if (!validationHashSet.Contains(actualArray[i]))
            {
                Console.WriteLine($"Unexpected argument found at index {i}: {actualArray[i]}");
                return false;
            }
            else
            {
                validationHashSet.Remove(actualArray[i]);
            }
        }

        return validationHashSet.Count == 0;
    }
    public void SaveNotes()
    {
        var settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };
        var NotesJson = JsonConvert.SerializeObject(Notes, settings);
        File.WriteAllText(FilePath, NotesJson);
    }

    public void LoadNotes()
    {
        string NotesJson = File.ReadAllText(FilePath);
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        if (NotesJson != "")
        {
            var TempNotes = JsonConvert.DeserializeObject<List<Note>>(NotesJson,settings);
            if (TempNotes != null)
            {
                Notes = TempNotes;
            }
        }
    }

    void CreateTable(string[] args)
    {
        string[] Flags = args.Where(arg => arg.StartsWith("-")).ToArray();
        string[] FlagsArguments = args.Where(arg => !arg.StartsWith("-")).ToArray();
        if (Flags.Length != FlagsArguments.Length)
        {
            Console.WriteLine("Invalid number of arguments given");
        }
        if (ValidateFlags(["-name", "-headers"], Flags))
        {
            int index = 0;
            string NoteName = "";
            string Headers = "";
            foreach (var Flag in Flags)
            {
                if (Flag == "-name")
                {
                    NoteName = FlagsArguments[index];
                }
                else if (Flag == "-headers")
                {
                    Headers = FlagsArguments[index];
                }
                index++;
            }
            var TblNote = new TableNote(NoteName, Headers);
            Notes.Add(TblNote);
            SaveNotes();
        }
    }
}