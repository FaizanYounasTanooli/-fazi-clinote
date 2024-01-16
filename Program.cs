using System;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

class Program
{
    static void Main(string[] args)
    {
        string json = """[{"Name":"John","Age":30,"City":"New York"},{"Name":"Alice","Age":25,"City":"Los Angeles"},{"Name":"Bob","Age":35,"City":"Chicago"}]""";
        var CliNotes=new CliNote();
        CliNotes.LoadNotes();
        //string[] largs =["ctn", "-name", "Test", "-headers" , "First Name,Last Name,Age"];
        //string[] largs =["tn","t", "Faizan Younas,20,Islamabad"];
        //string[] largs =["sn", "Test"];
        CliNotes.ExecuteCommand(args);
        //CliNotes.ExecuteCommand(args);
    }
    
}
