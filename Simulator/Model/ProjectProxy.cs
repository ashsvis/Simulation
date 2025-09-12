using System.ComponentModel;

namespace Simulator.Model
{
    public class ProjectProxy
    {
        [Category("Проект"), DisplayName("Имя")]
        public string Name 
        { 
            get => Project.Name;
            set
            {
                if (Project.Name == value) return;
                Project.Name = value;
                Project.Changed = true;
            }
        }

        [Category("Проект"), DisplayName("Описание")]
        public string Description 
        { 
            get => Project.Description;
            set
            {
                if (Project.Description == value) return;
                Project.Description = value;
                Project.Changed = true;
            }
        }

    }
}
