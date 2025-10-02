using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class OR : CommonLogic, IAddInput { public OR() : base(LogicFunction.Or, 2) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class OR3 : CommonLogic, IAddInput { public OR3() : base(LogicFunction.Or, 3) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class OR4 : CommonLogic, IAddInput { public OR4() : base(LogicFunction.Or, 4) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class OR5 : CommonLogic, IAddInput { public OR5() : base(LogicFunction.Or, 5) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class OR6 : CommonLogic, IAddInput { public OR6() : base(LogicFunction.Or, 6) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class OR7 : CommonLogic, IAddInput { public OR7() : base(LogicFunction.Or, 7) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class OR8 : CommonLogic { public OR8() : base(LogicFunction.Or, 8) { } }
}
