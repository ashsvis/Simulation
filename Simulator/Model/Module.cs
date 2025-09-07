namespace Simulator.Model
{
    public class Module
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<IFunction> Items { get; set; } = [];
    }
}
