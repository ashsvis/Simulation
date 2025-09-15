using System.Xml.Linq;

namespace Simulator.Model
{
    public interface ILoadSave
    {
        void Load(XElement? xtem);
        void Save(XElement xtem);
    }
}