using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Model
{
    public class ChannelsModel : List<ChannelModel> { }
    
    public class ChannelModel
    {
        public string id { get; set; }
        public string description { get; set; }
    }
}
