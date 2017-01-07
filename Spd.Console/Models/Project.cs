using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Spd.Console.Models
{
    public class Project
    {
        public List<WorkItem> WIS { get; set; } //WorkItem List
        public int? CID { get; set; } //Current WorkItem ID
        public int HID { get; set; }//Highest WorkItem ID


        private WorkItem _CurrentWorkItem;
        [JsonIgnore]
        public WorkItem CurrentWorkItem
        {
            get
            {
                if (_CurrentWorkItem == null && CID != null)
                    _CurrentWorkItem = FindWorkItem(CID.Value);
                return _CurrentWorkItem;
            }
            set
            {
                _CurrentWorkItem = value;
                CID = _CurrentWorkItem?.ID;
            }
        }

        public Project()
        {
            WIS = new List<WorkItem>();
        }

        public WorkItem FindParentWorkItem(int childID)
        {
            return WIS.Select(wi => wi.FindParent(childID)).FirstOrDefault(parent => parent != null);
        }

        public WorkItem FindWorkItem(int id)
        {
            return WIS.Select(wi => wi.Find(id)).FirstOrDefault(found => found != null);
        }

        public bool ShouldSerializeWIS()
        {
            return WIS != null && WIS.Any();
        }
    }
}
