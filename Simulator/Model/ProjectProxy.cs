using Simulator.Model.Interfaces;
using System.ComponentModel;

namespace Simulator.Model
{
    public class ProjectProxy : IVariable
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

        [Category("Проект"), DisplayName("Задачи")]
        public List<Module> Modules => Project.Modules;

        public void Clear()
        {
            Project.Clear();
        }

        public int CountVariables(Guid moduleId)
        {
            return Project.CountVariables(moduleId);
        }

        public ValueItem[] GetElementVariablesByIndex(Guid elementId)
        {
            return Project.GetElementVariablesByIndex(elementId);
        }

        public ValueItem? GetVariableByIndex(Guid moduleId, int itemIndex)
        {
            return Project.GetVariableByIndex(moduleId, itemIndex);
        }

        public ValueItem? ReadValue(Guid elementId, int pin, ValueSide side, ValueKind kind)
        {
            return Project.ReadValue(elementId, pin, side, kind);
        }

        public void WriteValue(Guid elementId, int pin, ValueSide side, ValueKind kind, object? value)
        {
            Project.WriteValue(elementId, pin, side, kind, value);
        }
    }
}
