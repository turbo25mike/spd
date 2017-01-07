using System.Collections.Generic;
using System.Linq;

namespace Spd.Console.Models
{
    public class WorkItem
    {
        public List<WorkItem> CHD { get; set; } //Children
        public string T { get; set; }//Title
        public string D { get; set; } //Description
        public int ID { get; set; } //Work Item ID
        public List<string> TGS { get; set; } //Tags
        public string OWNR { get; set; } //Owner

        public WorkItem(string title, int id, string username)
        {
            T = title;
            ID = id;
            OWNR = username;
            CHD = new List<WorkItem>();
        }

        public WorkItem Find(int id)
        {
            if (ID == id)
                return this;

            foreach (var wi in CHD)
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
            foreach (var wi in CHD)
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
            return CHD != null && CHD.Any();
        }

    }
}
