namespace Simulator.Model
{
    public class Element
    {
        public Element() { }
        public Type? Type { get; set; }
        public object? Instance { get; set; }
        public PointF Location { get; set; }

        public string[] DrawCommads()
        {
            List<string> lines = [];
            if (Instance is ICalculate instance)
            {
                var max = Math.Max(instance.Inputs.Length, instance.Outputs.Length);
                var step = 5;
                var height = step;
                for (var i = 0; i < max; i++)
                {
                    if (i < instance.Inputs.Length)
                    {

                    }
                    if (i < instance.Outputs.Length)
                    {

                    }
                }
            }

            return [.. lines];
        }
    }
}
