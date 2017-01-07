using System;
using System.IO;
using Newtonsoft.Json;
using Spd.Console.Models;

namespace Spd.Console
{
    public class ProjectManager
    {
        private string _Path = ".spd";
        public bool IsInitialized => File.Exists(_Path);

        private Project _Project;

        public Project Project
        {
            get
            {
                if (_Project == null && IsInitialized)
                    _Project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(_Path));
                return _Project;
            }
        }

        public void Init()
        {
            if (!File.Exists(_Path))
                File.WriteAllText(_Path, "{}");
        }

        public void Add(string title, string username)
        {
            Project.HID += 1;
            var wi = new WorkItem(title, Project.HID, username);
            if (Project.CurrentWorkItem == null)
            {
                Project.WIS.Add(wi);
                Project.CurrentWorkItem = wi;
            }
            else
            {
                Project.CurrentWorkItem.CHD.Add(wi);
            }
            Save();
        }

        public void Update(string description = null)
        {
            if (description != null)
                Project.CurrentWorkItem.D = description;

            Save();
        }

        public void MoveTo(int id)
        {
            Project.CurrentWorkItem = Project.FindWorkItem(id);
            Save();
        }

        public void MoveUp(int? levels)
        {
            if (!Project.CID.HasValue) //we're at the root
                return;

            if (levels == null) //move to parent of current work item
                Project.CurrentWorkItem = Project.FindParentWorkItem(Project.CID.Value);
            for (var i = 0; i < levels; i++)
            {
                if(Project.CID.HasValue) //stop if we hit the root.
                    Project.CurrentWorkItem = Project.FindParentWorkItem(Project.CID.Value);
            }

            Save();
        }

        public void Save()
        {
            File.WriteAllText(_Path, JsonConvert.SerializeObject(_Project, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }
    }
}
