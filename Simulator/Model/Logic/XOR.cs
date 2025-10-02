using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class XOR : CommonLogic, IAddInput { public XOR() : base(LogicFunction.Xor, 2) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class XOR3 : CommonLogic, IAddInput { public XOR3() : base(LogicFunction.Xor, 3) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class XOR4 : CommonLogic, IAddInput { public XOR4() : base(LogicFunction.Xor, 4) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class XOR5 : CommonLogic, IAddInput { public XOR5() : base(LogicFunction.Xor, 5) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class XOR6 : CommonLogic, IAddInput { public XOR6() : base(LogicFunction.Xor, 6) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class XOR7 : CommonLogic, IAddInput { public XOR7() : base(LogicFunction.Xor, 7) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class XOR8 : CommonLogic { public XOR8() : base(LogicFunction.Xor, 8) { } }
}
