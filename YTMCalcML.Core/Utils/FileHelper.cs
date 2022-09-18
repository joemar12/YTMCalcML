using CsvHelper;
using System.Globalization;

namespace YTMCalcML.Core.Utils
{
    public static class FileHelper
    {
        public static void WriteToFile<T>(string path, IEnumerable<T> records)
        {
            using (var stream = new StreamWriter(path))
            {
                using (var csvWriter = new CsvWriter(stream, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(records);
                }
            }
        }
    }
}