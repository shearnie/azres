using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azres
{
    class Program
    {
        static void Main(string[] args)
        {
            var fin = "d:\\all.txt"; // args[0];
            var fout = "d:\\out.csv"; // args[1];

            var items = new List<Item>();
            using (var sr = new StreamReader(fin))
            {
                var data = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd());
                foreach (var item in data.value)
                {
                    var id = item.id.Value;
                    items.Add(new Item()
                    {
                        Name = item.name.Value,
                        Type = item.type.Value,
                        Location = item.location.Value,
                        ResGrp = ((string)id).Split("/".ToCharArray())[4]
                    });
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine("name,type,resource group,location");

            foreach (var item in items.OrderBy(i => i.ResGrp).ThenBy(i => i.Type))
            {
                sb.AppendLine(string.Join(",", new List<string>
                {
                    item.Name, item.Type, item.ResGrp, item.Location
                }.ToArray()));
            }
            File.WriteAllText(fout, sb.ToString());
        }

        public class Item
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string ResGrp { get; set; }
            public string Location { get; set; }
        }
    }
}
