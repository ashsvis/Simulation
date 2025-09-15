using Simulator.Model.Logic;
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model.Diagram
{
    public class CommonDiagram : FilterablePropertyBase, ILink, ILoadSave
    {
        private readonly object[] getInputs;
        private readonly object[] getOutputs;
        private readonly GetLinkValueMethod?[] getLinkInputs;
        private readonly (Guid, int)[] getLinkSources;

        public CommonDiagram() : this(DiagramFunction.None)
        {
        }

        public CommonDiagram(DiagramFunction func)
        {
            getInputs = [];
            getOutputs = [];
            getLinkInputs = [];
            getLinkSources = [];
            Out = new object();
            var inputCount = 1;
            var outputCount = 1;
            if (func == DiagramFunction.Finish)
                outputCount = 0;
            if (func == DiagramFunction.Start)
                inputCount = 0;
            getInputs = new object[inputCount];
            getOutputs = new object[outputCount];
            getLinkInputs = new GetLinkValueMethod?[inputCount];
            getLinkSources = new (Guid, int)[inputCount];
        }

        [Browsable(false)]
        public bool[] LinkedInputs
        {
            get
            {
                List<bool> list = [];
                for (var i = 0; i < getInputs.Length; i++)
                {
                    if (getLinkInputs[i] is GetLinkValueMethod _)
                        list.Add(true);
                    else
                        list.Add(false);
                }
                return [.. list];
            }
        }

        [Browsable(false)]
        public (Guid, int)[] InputLinkSources => getLinkSources;

        [Browsable(false)]
        public object Out { get; set; }

        public GetLinkValueMethod? GetResultLink(int outputIndex)
        {
            return () => Out;
        }

        public void ResetValueLinkToInp(int inputIndex)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length)
            {
                getLinkInputs[inputIndex] = null;
                getLinkSources[inputIndex] = (Guid.Empty, 0);
            }
        }

        public void SetValueLinkToInp(int inputIndex, GetLinkValueMethod? getMethod, Guid sourceId, int outputPinIndex)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length)
            {
                getLinkInputs[inputIndex] = getMethod;
                getLinkSources[inputIndex] = (sourceId, outputPinIndex);
            }
        }

        public void SetValueToInp(int inputIndex, object? value)
        {
            if (inputIndex >= 0 && inputIndex < getLinkInputs.Length &&
                value != null && getLinkInputs[inputIndex] == null)
                getInputs[inputIndex] = value;
        }

        public void Load(XElement? xtance)
        {
            var xinputs = xtance?.Element("Inputs");
            if (xinputs != null)
            {
                foreach (XElement item in xinputs.Elements("Input"))
                {
                    if (int.TryParse(item.Attribute("Index")?.Value, out int index))
                    {
                        if (Guid.TryParse(item.Element("SourceId")?.Value, out Guid guid) && guid != Guid.Empty)
                        {
                            if (int.TryParse(item.Element("OutputIndex")?.Value, out int outputIndex))
                                getLinkSources[index] = (guid, outputIndex);
                            else
                                getLinkSources[index] = (guid, 0);
                        }
                    }
                }
            }
            var xoutputs = xtance?.Element("Outputs");
            if (xoutputs != null)
            {
                foreach (XElement item in xoutputs.Elements("Output"))
                {
                    if (int.TryParse(item.Attribute("Index")?.Value, out int index))
                    {

                    }
                }
            }
        }

        public void Save(XElement xtem)
        {
            var xtance = new XElement("Instance");

            XElement xinputs = new("Inputs");
            bool customInputs = false;
            for (var i = 0; i < getInputs.Length; i++)
            {
                (Guid id, int output) = getLinkSources[i];
                customInputs = true;
                XElement xinput = new("Input");
                xinputs.Add(xinput);
                xinput.Add(new XAttribute("Index", i));

                if (id != Guid.Empty)
                {
                    xinput.Add(new XElement("SourceId", id));
                    if (output > 0)
                        xinput.Add(new XElement("OutputIndex", output));
                }
            }
            if (customInputs)
                xtance.Add(xinputs);

            XElement xoutputs = new("Outputs");
            bool customOutputs = false;
            for (var i = 0; i < 1; i++)
            {
                customOutputs = true;
                XElement xoutput = new("Output");
                xoutputs.Add(xoutput);
                xoutput.Add(new XAttribute("Index", i));
            }
            if (customOutputs)
                xtance.Add(xoutputs);

            if (customInputs || customOutputs)
                xtem.Add(xtance);
        }
    }
}
