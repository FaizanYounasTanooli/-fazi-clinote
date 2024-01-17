
using System.Text;

public class TableNote : Note {
    public List<string> Headers {get; set;}
    public List<List<string>> Rows {get; set;}
    public TableNote()
    {

    }
    public TableNote(string Name, List<string> Headers)
    {
        this.Name=Name;
        this.Headers=Headers;
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
        if (Rows==null)
        {
            Rows=new List<List<string>>();
        }
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
    public void AddRows(List<List<string>> RowsList) {
        if (Rows==null)
        {
            Rows=new List<List<string>>();
        }
        Rows.AddRange(RowsList);
        Console.WriteLine("Rows Added Successfully");

    }
    public override void Show() {
        Console.WriteLine("Note: "+Name);
        Console.WriteLine(TableGenerator.GenerateTable(this.Headers,this.Rows));
    }
    public override void Show(string searchString) {
        List<List<string>> matchingRows = new List<List<string>>();
        foreach (var row in Rows)
        {
            foreach (var cell in row)
            {
                if (cell.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                {
                    matchingRows.Add(row);
                    break; // Break the inner loop, as we've found a match in this row.
                }
            }
        }
        if (matchingRows.Count==1)
        {
            
        }
        Console.WriteLine(TableGenerator.GenerateTable(this.Headers,matchingRows));
    }
    
}