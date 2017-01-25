using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Spd.Console.Models
{
    public class Project
    {
        [JsonProperty("w")]
        public List<WorkItem> WorkItems { get; set; } //WorkItem List

        [JsonProperty("c")]
        public int? CurrentID { get; set; } //Current WorkItem ID

        [JsonProperty("h")]
        public int HighestID { get; set; }//Highest WorkItem ID


        private WorkItem _CurrentWorkItem;
        [JsonIgnore]
        public WorkItem CurrentWorkItem
        {
            get
            {
                if (_CurrentWorkItem == null && CurrentID != null)
                    _CurrentWorkItem = FindWorkItem(CurrentID.Value);
                return _CurrentWorkItem;
            }
            set
            {
                _CurrentWorkItem = value;
                CurrentID = _CurrentWorkItem?.ID;
            }
        }

        public Project()
        {
            WorkItems = new List<WorkItem>();
        }

        public WorkItem FindParentWorkItem(int childID)
        {
            return WorkItems.Select(wi => wi.FindParent(childID)).FirstOrDefault(parent => parent != null);
        }

        public WorkItem FindWorkItem(int id)
        {
            return WorkItems.Select(wi => wi.Find(id)).FirstOrDefault(found => found != null);
        }

        public bool ShouldSerializeWIS()
        {
            return WorkItems != null && WorkItems.Any();
        }
    }
}
