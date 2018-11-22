using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace DistinctSongsLinq
{
    class Program
    {
       

        static void Main(string[] args)
        {

            List<Music> list = ReadCsvModel();
            var countList = new List<int>();

            var query = list.AsEnumerable().Where(c => c.PLAY_TS.Contains("10/08/2016")).Select(c => new { SONG_ID = c.SONG_ID, CLIENT_ID = c.CLIENT_ID }).Distinct();

            foreach (var line in query.GroupBy(info => info.CLIENT_ID)
                        .Select(group => new {
                            Metric = group.Key,
                            Count = group.Count()
                        })
                        .OrderBy(x => x.Metric))
            {
                countList.Add(line.Count);
            }

            var g = countList.GroupBy(i => i);

            foreach (var grp in g)
            {
                Console.WriteLine("Distinct Songs : {0} - Clients : {1}", grp.Key, grp.Count());
            }
            Console.ReadLine();

        }

        private static List<Music> ReadCsvModel()
        {
            var csvPath = "../exhibitA-input.csv";

            var allLines = File.ReadLines(csvPath).Select(a => a.Split(';'));
            var list = new List<Music>();
            
            foreach (var line in allLines)
            {
                string str = line[0];
                if (!str.Contains("ID"))
                {
                    var values = str.Split('\t');
                    list.Add(new Music { PLAY_ID = values[0], SONG_ID = values[1], CLIENT_ID = values[2], PLAY_TS = values[3] });
                }

            }

            return list;
        }

  
        
    }
}
