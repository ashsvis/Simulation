using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class OR : CommonLogic, IAddInput { public OR() : base(LogicFunction.Or, 2) { } }
    public class OR3 : CommonLogic, IAddInput { public OR3() : base(LogicFunction.Or, 3) { } }
    public class OR4 : CommonLogic, IAddInput { public OR4() : base(LogicFunction.Or, 4) { } }
    public class OR5 : CommonLogic, IAddInput { public OR5() : base(LogicFunction.Or, 5) { } }
    public class OR6 : CommonLogic, IAddInput { public OR6() : base(LogicFunction.Or, 6) { } }
    public class OR7 : CommonLogic, IAddInput { public OR7() : base(LogicFunction.Or, 7) { } }
    public class OR8 : CommonLogic { public OR8() : base(LogicFunction.Or, 8) { } }
}
