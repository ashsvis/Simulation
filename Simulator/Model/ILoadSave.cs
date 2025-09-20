using System.Xml.Linq;

namespace Simulator.Model
{
    public interface ILoadSave
    {
        string? Name { get; set; }
        void Load(XElement? xtem);
        void Save(XElement xtem);
    }
}