using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Model
{
    public class HotelsDistributionModel : List<HotelsDistributionRoomModel>
    {
        public HotelsDistributionModel(string distribution)
        {
            HotelsDistributionRoomModel tmpRoom;
            string[] rooms = distribution.Split('!');
            string[] childs;

            for (int i = 0; i < rooms.Length; i++)
            {
                tmpRoom = new HotelsDistributionRoomModel();
                tmpRoom.Children = new List<string>();
                childs = rooms[i].Split('-');
                tmpRoom.Adults = childs[0];
                if (childs.Length > 1)
                    for (int e = 1; e < childs.Length; e++)
                        tmpRoom.Children.Add(childs[e]);

                this.Add(tmpRoom);
            }
        }

        public int TotalAdults()
        {
            int total = 0;
            for (int i = 0; i < this.Count; i++)
                total += int.Parse(this[i].Adults);
            return total;
        }

        public int TotalChildren()
        {
            int total = 0;
            for (int i = 0; i < this.Count; i++)
                total += this[i].Children.Count;
            return total;
        }

        public int Total()
        {
            return this.TotalAdults() + this.TotalChildren();
        }

        public override string ToString()
        {
            string builder = "";
            for (int i = 0; i < this.Count; i++)
                builder += (i>0?"!":"") + this[i].ToString();

            return builder;
        }
    }

    public class HotelsDistributionRoomModel 
    {
        public string Adults { get; set; }
        public List<string> Children { get; set; }

        public override string ToString()
        {
            string builder = "";
            builder += this.Adults;
            if (this.Children.Count > 0)
                builder += "-" + string.Join("-", this.Children.ToArray());

            return builder;
        }
    }
}
