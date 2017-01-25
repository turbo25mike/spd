using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Spd.Console.Models
{
    public class WorkItem
    {
        [JsonProperty("chd")]
        public List<WorkItem> Children { get; set; }

        [JsonProperty("t")]
        public string Title { get; set; }

        [JsonProperty("d")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("tgs")]
        public List<string> Tags { get; set; }

        [JsonProperty("o")]
        public string Owner { get; set; }

        [JsonProperty("z")]
        public int Size { get; set; }

        [JsonProperty("p")]
        public int Priority { get; set; } 

        [JsonProperty("h")]
        public double HoursWorked { get; set; } 

        [JsonProperty("s")]
        public DateTime StartDate { get; set; } 

        [JsonProperty("e")]
        public DateTime CompleteDate { get; set; } 

        public WorkItem(string title, int id)
        {
            Title = title;
            ID = id;
            Children = new List<WorkItem>();
            Tags = new List<string>();
        }

        public WorkItem Find(int id)
        {
            if (ID == id)
                return this;

            foreach (var wi in Children)
            {
                if (wi.ID == id)
                    return wi;

                var found = wi.Find(id);
                if (found != null)
                    return found;
            }
            return null;
        }

        public WorkItem FindParent(int childID)
        {
            foreach (var wi in Children)
            {
                if (wi.ID == childID)
                    return this;

                var found = wi.FindParent(childID);
                if (found != null)
                    return found;
            }
            return null;
        }


        public bool ShouldSerializeCHD()
        {
            return Children != null && Children.Any();
        }

    }
}
