using System;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

class Program
{
    // static void Main(string[] args)
    // {
       
    //     //string[] largs =["ctn", "-name", "Test", "-headers" , "First Name,Last Name,Age"];
    //     //string[] largs =["tn","t", "Faizan Younas,20,Islamabad"];
    //     //string[] largs =["sn", "Test"];
    //     //CliNotes.ExecuteCommand(args);
    //     startit();
    // }
    static void Main(string[] args)
    {
        var CliNotes=new CliNote();
        CliNotes.ExecuteCommand(args);
    }
    static void startit() {
        var CliNotes=new CliNote();
        
        while(true) {
            Console.Write("Command: ");
            var args= Console.ReadLine();
            var argsArray=args.Split(" ");
            CliNotes.ExecuteCommand(argsArray);
        }
    }


}
