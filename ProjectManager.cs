using System.IO;
using Newtonsoft.Json;
using Spd.Console.Models;
using System.Linq;
using Spd.Console.Options;

namespace Spd.Console
{
    public class ProjectManager
    {
        public ProjectManager(ConfigManager configManager)
        {
            _configManager = configManager;
        }

        private string _Path = ".spd";
        public bool IsInitialized => File.Exists(_Path);

        private Project _project;
        private readonly ConfigManager _configManager;

        public Project Project
        {
            get
            {
                if (_project == null && IsInitialized)
                    _project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(_Path));
                return _project;
            }
        }

        public void Run(Verbs opts)
        {

            if (!IsSpdSolution()) return;
            Add(opts.Add);
            Edit(opts.Edit);
            MoveTo(opts.To);
            
            //Project.ListCurrentWorkItem();
        }

        public void Init()
        {
            if (!File.Exists(_Path))
                File.WriteAllText(_Path, "{}");
        }

        private bool IsSpdSolution()
        {
            if (IsInitialized) return true;

            System.Console.WriteLine("This is not a spd project. Would you like to make it one? Y/N");
            var response = System.Console.ReadLine();
            if (response?.ToLower() != "yes" && response?.ToLower() != "y")
                return false;
            Init();
            return true;
        }

        private void Add(AddOptions opts)
        {
            if (opts?.Title == null) return;

            Project.HighestID += 1;
            var wi = new WorkItem(opts.Title, Project.HighestID);
            if (Project.CurrentWorkItem == null)
            {
                Project.WorkItems.Add(wi);
                Project.CurrentWorkItem = wi;
            }
            else
            {
                Project.CurrentWorkItem.Children.Add(wi);
            }
            Save();
        }

        private void Edit(EditOptions opts)
        {
            if (opts == null)
                return;

            if (opts.Description != null)
                Project.CurrentWorkItem.Description = opts.Description;
            if (opts.Priority.HasValue)
                Project.CurrentWorkItem.Priority = opts.Priority.Value;
            if (opts.Owner != null)
                Project.CurrentWorkItem.Owner = opts.Owner;
            if (opts.Size.HasValue)
                Project.CurrentWorkItem.Size = opts.Size.Value;
            if (opts.Title != null)
                Project.CurrentWorkItem.Title = opts.Title;
            Save();
        }

        private void MoveTo(ToOptions opts)
        {
            if (opts?.ID == null) return;

            int id;
            if (int.TryParse(opts.ID, out id))
                Project.CurrentWorkItem = Project.FindWorkItem(id);

            if (opts.ID.ToLower().Contains(".."))
                MoveUp(opts.ID.Count(c => c == '.') / 2);

            if (opts.ID.ToLower() == "root" || opts.ID.ToLower() == "r")
                Project.CurrentWorkItem = null;

            Save();
        }

        private void MoveUp(int? levels)
        {
            if (!Project.CurrentID.HasValue) //we're at the root
                return;

            if (levels == null) //move to parent of current work item
                Project.CurrentWorkItem = Project.FindParentWorkItem(Project.CurrentID.Value);
            for (var i = 0; i < levels; i++)
            {
                if (Project.CurrentID.HasValue) //stop if we hit the root.
                    Project.CurrentWorkItem = Project.FindParentWorkItem(Project.CurrentID.Value);
            }

            Save();
        }
        
        private void Save()
        {
            File.WriteAllText(_Path, JsonConvert.SerializeObject(_project, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }
    }
}
