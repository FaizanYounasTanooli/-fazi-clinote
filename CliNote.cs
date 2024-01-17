using System.ComponentModel.DataAnnotations;
using System.Text;
using System.IO;
using Newtonsoft.Json;
//using Newtonsoft.Json;

public class CliNote
{
    List<Note> Notes = new List<Note>();
    static string scope="global";
    static string FilePath =  @"Notes.txt";
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
        LoadNotes();
        if (commnad.Length > 0 || (commnad.Length == 1 && commnad[0] != ""))
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
                ShowNote(args);
            }
            else if (IsCmdOk(cmd, "h", "Help"))
            {
                ShowHelp();
            }
            else
            {
                if (commnad.Length <= 2)
                {
                    ShowNote(commnad);
                }
                else
                {
                    Console.WriteLine("Command not recognized");
                }
            }
        }
        else
        {
            ShowList();
            Console.WriteLine("Developed by Faizan Younas Tanooli");
        }
    }
    void ShowList()
    {
        if (Notes.Count > 0)
        {
            Console.WriteLine("Notes list:");
            Console.WriteLine(TableGenerator.GenerateTable(Notes.Select((note, index) => new { SrNo = index + 1, note.Name, Type = note.GetType() }).ToList()));
        }
        else
        {
            Console.WriteLine("No note added yet.");
            ShowHelp();
        }
    }
    void ShowHelp()
    {
        Console.WriteLine("CLI Notes Application Help");
        Console.WriteLine("==========================\n");

        Console.WriteLine("General Usage:");
        Console.WriteLine("  [command] [arguments]\n");

        Console.WriteLine("Commands:");

        // Create Table Note
        Console.WriteLine("CreateTableNote (ctn):");
        Console.WriteLine("  Creates a new table note with specified headers.");
        Console.WriteLine("  Usage: ctn -name [note name] -headers [header1,header2,...]\n");

        // Create Normal Note
        Console.WriteLine("CreateNormalNote (cnn):");
        Console.WriteLine("  Creates a new normal note.");
        Console.WriteLine("  Usage: cnn [note name]\n");

        // Take Note
        Console.WriteLine("TakeNote (tn):");
        Console.WriteLine("  Adds content to an existing table note.");
        Console.WriteLine("  Usage: tn -t [note name] -row [\"cell1,cell2,...\"]\n");

        // Show Note
        Console.WriteLine("ShowNote (sn):");
        Console.WriteLine("  Displays the content of a note.");
        Console.WriteLine("  Usage: sn [note name]\n");

        // Show List
        Console.WriteLine("ShowList:");
        Console.WriteLine("  Displays a list of all notes.");
        Console.WriteLine("  Usage: Execute command without arguments to see all notes.\n");

        // Help
        Console.WriteLine("Help (h):");
        Console.WriteLine("  Shows this help information.\n");
        Console.WriteLine("Developer: Faizan Younas Tanooli");
    }
    void ShowNote(string[] args)
    {
        string name="";
        string filter="";
        if (args.Length>=1)
        {
            name = args[0];
            
        }
        if (args.Length>=2)
        {
            filter=args[1];
        }
        var NoteToShow = GetNote(name);
        if (NoteToShow != null)
        {
            if (filter!="")
            {
                NoteToShow.Show(filter);
            }
            else {
                 NoteToShow.Show();
            }
        }
        else
        {
            Console.WriteLine("Note not found with name:" + name);
        }
        
    }
    Note? GetNote(string name)
    {
        return Notes.Find(note => note.Name.ToLower() == name.ToLower());
    }
    void TakeNote(string[] args)
    {
        if (args.Length >= 2)
        {
            if (IsCmdOk(args[0], "-t", "-Table"))
            {
                string Name = args[1];
                var CurrentNote = (TableNote?)GetNote(Name);
                if (CurrentNote != null)
                {
                    string? Row = null;
                    try
                    {

                        Row = args[2];
                    }
                    catch
                    {

                    }
                    if (Row != null)
                    {
                        CurrentNote.AddRow(Row);
                    }
                    else
                    {
                        List<List<string>> Rows = GetRows(CurrentNote);
                        CurrentNote.AddRows(Rows);
                    }

                }
                else
                {
                    Console.WriteLine("Note Not found with name: " + Name);
                }
            }
            else
            {
                Console.WriteLine("Invalid Commnad");
            }
        }
        else
        {
            Console.WriteLine("Invalid number of arguments given");
        }
        SaveNotes();
    }

    List<List<string>> GetRows(TableNote TblNote)
    {
        string AddMore = "";
        List<List<string>> RowsToAdd = new List<List<string>>();

        while (AddMore == "")
        {
            List<string> RowToAdd = new List<string>();
            foreach (string Column in TblNote.Headers)
            {
                Console.Write(Column + ": ");
                string Value = Console.ReadLine() ?? "N/A";
                RowToAdd.Add(Value);
            }
            RowsToAdd.Add(RowToAdd);
            Console.Write("Hit Enter to add more type n to exit");
            AddMore = Console.ReadLine() ?? "n";
        }
        return RowsToAdd;
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
    static bool ValidateFlags(string[] expectedStrings, List<string> actualArray)
    {
        var validationHashSet = new HashSet<string>(expectedStrings);

        for (int i = 0; i < actualArray.Count; i++)
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
        string? NotesJson = JsonConvert.SerializeObject(Notes, settings);
        if (!File.Exists(FilePath))
        {
            using (FileStream fs = File.Create(FilePath))
            {

            }
        }
        File.WriteAllText(FilePath, NotesJson);
    }

    public void LoadNotes()
    {
        if (File.Exists(FilePath))
        {
            string NotesJson = File.ReadAllText(FilePath);
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            if (NotesJson != "")
            {
                var TempNotes = JsonConvert.DeserializeObject<List<Note>>(NotesJson, settings);
                if (TempNotes != null)
                {
                    Notes = TempNotes;
                }
            }
        }
    }

    void CreateTable(string[] args)
    {
        var Flags = args.Where(arg => arg.StartsWith("-")).ToList();
        var FlagsArguments = args.Where(arg => !arg.StartsWith("-")).ToList();
        bool Result = true;
        
        if (Flags.Count == 1)
        {
            Result = ValidateFlags(new[] {"-headers"}, Flags);
        }
        if (!Result)
        {
            return;
        }
        string Headers = "";
        string NoteName = args[0];
        int HeadersIndex = Flags.IndexOf("-headers");
        if (HeadersIndex == -1)
        {
            List<string> HeadersList = GetHeaders();
            var TblNote = new TableNote(NoteName, HeadersList);
            Notes.Add(TblNote);
        }
        else
        {
            Headers = FlagsArguments[HeadersIndex];
            var TblNote = new TableNote(NoteName, Headers);
            Notes.Add(TblNote);
        }
        SaveNotes();
    }

    List<string> GetHeaders()
    {
        string? HeaderName = null;
        List<string> Headers = new List<string>();
        Console.WriteLine("Enter Column Names");
        while (HeaderName != "")
        {
            Console.Write("Name :");
            HeaderName = Console.ReadLine()?.Trim() ?? "";
            if (HeaderName != "")
            {
                Headers.Add(HeaderName);
            }
        }
        return Headers;
    }
}