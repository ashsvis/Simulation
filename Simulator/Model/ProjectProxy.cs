using System.ComponentModel;

namespace Simulator.Model
{
    public class ProjectProxy
    {
        [Category("Проект"), DisplayName("Имя")]
        public string Name { get => Project.Name; set => Project.Name = value; }

        [Category("Проект"), DisplayName("Описание")]
        public string Description { get => Project.Description; set => Project.Description = value; }

    }
}
