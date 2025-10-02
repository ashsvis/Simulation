using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class XOR : CommonLogic, IAddInput { public XOR() : base(LogicFunction.Xor, 2) { } }
    public class XOR3 : CommonLogic, IAddInput { public XOR3() : base(LogicFunction.Xor, 3) { } }
    public class XOR4 : CommonLogic, IAddInput { public XOR4() : base(LogicFunction.Xor, 4) { } }
    public class XOR5 : CommonLogic, IAddInput { public XOR5() : base(LogicFunction.Xor, 5) { } }
    public class XOR6 : CommonLogic, IAddInput { public XOR6() : base(LogicFunction.Xor, 6) { } }
    public class XOR7 : CommonLogic, IAddInput { public XOR7() : base(LogicFunction.Xor, 7) { } }
    public class XOR8 : CommonLogic { public XOR8() : base(LogicFunction.Xor, 8) { } }
}
