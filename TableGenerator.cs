
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
public class TableGenerator
{
    enum ConsoleColor
    {
        Black = 0,
        Red,
        Green,
        Yellow,
        Blue,
        Magenta,
        Cyan,
        White,
    }
    enum ColorType
    {
        ForeGround = 30,
        BackGround = 40
    }

    static string Colored(string text, ConsoleColor color = ConsoleColor.White, ColorType colorType = ColorType.ForeGround)
    {
        int baseColor = (int)colorType;
        int currentColor = +(int)color;
        return $"[{baseColor + currentColor}m{text}[0m";
    }

    static string GenerateTable(string json)
    {
        var dataArray = JsonSerializer.Deserialize<JsonElement[]>(json);
        if (dataArray == null || dataArray.Length == 0)
            return "No data to display.";
        var headers = new List<string>();
        foreach (var property in dataArray[0].EnumerateObject())
        {
            headers.Add(property.Name);
        }
        int[] columnWidths = new int[headers.Count];
        for (int i = 0; i < headers.Count; i++)
        {
            columnWidths[i] = Math.Max(headers[i].Length, dataArray.Max(row => row.GetProperty(headers[i]).ToString().Length));
        }
        string horizontalLine = $"┌{string.Join("┬", columnWidths.Select(width => new string('─', width)))}┐";
        string endingLine = $"└{string.Join("┴", columnWidths.Select(width => new string('─', width)))}┘";
        string headerRow = $"│{string.Join("│", headers.Select((string header, int i) => Colored(header.PadRight(columnWidths[i]), ConsoleColor.Cyan)))}│";
        string separatorRow = $"├{string.Join("┼", columnWidths.Select(width => new string('─', width)))}┤";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(horizontalLine);
        sb.AppendLine(headerRow);
        sb.AppendLine(separatorRow);

        foreach (var row in dataArray)
        {
            StringBuilder dataRowBuilder = new StringBuilder("│");

            foreach (var header in headers)
            {
                string cellValue = Convert.ToString(row.GetProperty(header));
                dataRowBuilder.Append(cellValue.PadRight(columnWidths[headers.IndexOf(header)])).Append("│");
            }

            sb.AppendLine(dataRowBuilder.ToString());
        }

        sb.AppendLine(endingLine);
        return sb.ToString();
    }
    public static string GenerateTable<T>(IList<T> obj)
    {
        var headers = GetPropertyNames(obj[0]);
        int[] columnWidths = new int[headers.Count];
        for (int i = 0; i < headers.Count; i++)
        {
            columnWidths[i] = Math.Max(headers[i].Length, obj.Max(row => row.GetType().GetProperty(headers[i]).GetValue(row,null).ToString().Length));
        }
        string horizontalLine = $"┌{string.Join("┬", columnWidths.Select(width => new string('─', width)))}┐";
        string endingLine = $"└{string.Join("┴", columnWidths.Select(width => new string('─', width)))}┘";
        string headerRow = $"│{string.Join("│", headers.Select((string header, int i) => Colored(header.PadRight(columnWidths[i]), ConsoleColor.Cyan)))}│";
        string separatorRow = $"├{string.Join("┼", columnWidths.Select(width => new string('─', width)))}┤";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(horizontalLine);
        sb.AppendLine(headerRow);
        sb.AppendLine(separatorRow);

        foreach (var row in obj)
        {
            StringBuilder dataRowBuilder = new StringBuilder("│");

            foreach (var header in headers)
            {
                string cellValue = Convert.ToString(row.GetType().GetProperty(header).GetValue(row,null));
                dataRowBuilder.Append(cellValue.PadRight(columnWidths[headers.IndexOf(header)])).Append("│");
            }

            sb.AppendLine(dataRowBuilder.ToString());
        }

        sb.AppendLine(endingLine);
        return sb.ToString();
    }
    static List<string> GetPropertyNames(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        Type type = obj.GetType();
        return type.GetProperties().Select(prop => prop.Name).ToList();
    }

    public static string GenerateTable(List<string> headers, List<List<string>> Rows)
    {

        if (Rows!=null && Rows.Count > 0)
        {
            int[] columnWidths = new int[headers.Count];
            for (int i = 0; i < headers.Count; i++)
            {
                columnWidths[i] = Math.Max(headers[i].Length, Rows.Max(row=>row[i].Length));
            }
            string horizontalLine = $"┌{string.Join("┬", columnWidths.Select(width => new string('─', width)))}┐";
            string endingLine = $"└{string.Join("┴", columnWidths.Select(width => new string('─', width)))}┘";
            string headerRow = $"│{string.Join("│", headers.Select((string header, int i) => Colored(header.PadRight(columnWidths[i]), ConsoleColor.Cyan)))}│";
            string separatorRow = $"├{string.Join("┼", columnWidths.Select(width => new string('─', width)))}┤";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(horizontalLine);
            sb.AppendLine(headerRow);
            sb.AppendLine(separatorRow);

            foreach (var row in Rows)
            {
                StringBuilder dataRowBuilder = new StringBuilder("│");

                foreach (var value in row)
                {
                    dataRowBuilder.Append(value.PadRight(columnWidths[row.IndexOf(value)])).Append("│");
                }

                sb.AppendLine(dataRowBuilder.ToString());
            }

            sb.AppendLine(endingLine);
            return sb.ToString();
        }
        else {
            return "No Rows added yet";
        }
    }
}