
using System.Text;

public class TableNote : Note {
    public List<string> Headers {get; set;}
    public List<List<string>> Rows {get; set;}
    public TableNote()
    {

    }
    public TableNote(string Name, string Headers,string Rows="")
    {
        this.Name=Name;
        this.Headers=Headers.Split(",").Select(item=>item.Trim()).ToList();
        if (Rows=="") {
            this.Rows= new List<List<string>>();
        }
        else {
            this.Rows= Rows.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None).Select(row=> row.Split(",").Select(item=>item.Trim()).ToList()).ToList();
        }
    }
    public void AddRow(string Row) {
        var RowList=Row.Split(",").Select(item=>item.Trim()).ToList();
        if (RowList.Count==Headers.Count)
        {
            Rows.Add(RowList);
            Console.WriteLine("Row Added Successfully");
        }
        else {
            Console.WriteLine("Column Count mismatch");
        }

    }
    public override void Show() {
        Console.WriteLine(TableGenerator.GenerateTable(this.Headers,this.Rows));
    }
    
}