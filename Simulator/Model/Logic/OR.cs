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

    public static class OrHelper
    {
        public static void AddInputAt(this Model.Logic.OR and, Element element)
        {
            var and3 = new Model.Logic.OR3();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.OR3 and, Element element)
        {
            var and3 = new Model.Logic.OR4();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.OR4 and, Element element)
        {
            var and3 = new Model.Logic.OR5();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.OR5 and, Element element)
        {
            var and3 = new Model.Logic.OR6();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.OR6 and, Element element)
        {
            var and3 = new Model.Logic.OR7();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.OR7 and, Element element)
        {
            var and3 = new Model.Logic.OR8();
            Project.UpdateElementAndFunction(and, element, and3);
        }

    }
}
