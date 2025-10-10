using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class AND : CommonLogic, ICanInversed, IAddInput { public AND() : base(LogicFunction.And, 2) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }
    public class AND3 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public AND3() : base(LogicFunction.And, 3) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }
    public class AND4 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public AND4() : base(LogicFunction.And, 4) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }
    public class AND5 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public AND5() : base(LogicFunction.And, 5) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }
    public class AND6 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public AND6() : base(LogicFunction.And, 6) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }
    public class AND7 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public AND7() : base(LogicFunction.And, 7) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }
    public class AND8 : CommonLogic, ICanInversed, IRemoveInput { public AND8() : base(LogicFunction.And, 8) { }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public static class AndHelper
    {
        public static void AddInputAt(this Model.Logic.AND and, Element element)
        {
            var and3 = new Model.Logic.AND3();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.AND3 and, Element element)
        {
            var and4 = new Model.Logic.AND4();
            Project.UpdateElementAndFunction(and, element, and4);
        }

        public static void RemoveInputAt(this Model.Logic.AND3 and, Element element)
        {
            var and2 = new Model.Logic.AND();
            Project.UpdateElementAndFunction(and, element, and2);
        }

        public static void AddInputAt(this Model.Logic.AND4 and, Element element)
        {
            var and5 = new Model.Logic.AND5();
            Project.UpdateElementAndFunction(and, element, and5);
        }

        public static void RemoveInputAt(this Model.Logic.AND4 and, Element element)
        {
            var and3 = new Model.Logic.AND3();
            Project.UpdateElementAndFunction(and, element, and3);
        }

        public static void AddInputAt(this Model.Logic.AND5 and, Element element)
        {
            var and6 = new Model.Logic.AND6();
            Project.UpdateElementAndFunction(and, element, and6);
        }

        public static void RemoveInputAt(this Model.Logic.AND5 and, Element element)
        {
            var and5 = new Model.Logic.AND5();
            Project.UpdateElementAndFunction(and, element, and5);
        }

        public static void AddInputAt(this Model.Logic.AND6 and, Element element)
        {
            var and7 = new Model.Logic.AND7();
            Project.UpdateElementAndFunction(and, element, and7);
        }

        public static void RemoveInputAt(this Model.Logic.AND6 and, Element element)
        {
            var and5 = new Model.Logic.AND5();
            Project.UpdateElementAndFunction(and, element, and5);
        }

        public static void AddInputAt(this Model.Logic.AND7 and, Element element)
        {
            var and8 = new Model.Logic.AND8();
            Project.UpdateElementAndFunction(and, element, and8);
        }

        public static void RemoveInputAt(this Model.Logic.AND7 and, Element element)
        {
            var and6 = new Model.Logic.AND6();
            Project.UpdateElementAndFunction(and, element, and6);
        }

        public static void RemoveInputAt(this Model.Logic.AND8 and, Element element)
        {
            var and7 = new Model.Logic.AND7();
            Project.UpdateElementAndFunction(and, element, and7);
        }

    }
}
