using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class AND : CommonLogic, IAddInput { public AND() : base(LogicFunction.And, 2) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }
    public class AND3 : CommonLogic, IAddInput { public AND3() : base(LogicFunction.And, 3) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }
    public class AND4 : CommonLogic, IAddInput { public AND4() : base(LogicFunction.And, 4) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }
    public class AND5 : CommonLogic, IAddInput { public AND5() : base(LogicFunction.And, 5) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }
    public class AND6 : CommonLogic, IAddInput { public AND6() : base(LogicFunction.And, 6) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }
    public class AND7 : CommonLogic, IAddInput { public AND7() : base(LogicFunction.And, 7) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }
    public class AND8 : CommonLogic { public AND8() : base(LogicFunction.And, 8) { } }

    public static class AndHelper
    {
        public static void AddInputAt(this Model.Logic.AND and, Element element)
        {
            var and3 = new Model.Logic.AND3();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.AND3 and, Element element)
        {
            var and3 = new Model.Logic.AND4();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.AND4 and, Element element)
        {
            var and3 = new Model.Logic.AND5();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.AND5 and, Element element)
        {
            var and3 = new Model.Logic.AND6();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.AND6 and, Element element)
        {
            var and3 = new Model.Logic.AND7();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.AND7 and, Element element)
        {
            var and3 = new Model.Logic.AND8();
            Project.UpdateElementAndFunction(and, element, and3);
        }

    }
}
