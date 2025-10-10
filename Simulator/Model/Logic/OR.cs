using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class OR : CommonLogic, ICanInversed, IAddInput { public OR() : base(LogicFunction.Or, 2) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class OR3 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public OR3() : base(LogicFunction.Or, 3) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class OR4 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public OR4() : base(LogicFunction.Or, 4) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class OR5 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public OR5() : base(LogicFunction.Or, 5) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class OR6 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public OR6() : base(LogicFunction.Or, 6) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class OR7 : CommonLogic, ICanInversed, IAddInput, IRemoveInput { public OR7() : base(LogicFunction.Or, 7) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class OR8 : CommonLogic, ICanInversed, IRemoveInput { public OR8() : base(LogicFunction.Or, 8) { }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public static class OrHelper
    {
        public static void AddInputAt(this OR or, Element element)
        {
            var or3 = new OR3();
            Project.UpdateElementAndFunction(or, element, or3);
        }

        public static void AddInputAt(this OR3 or3, Element element)
        {
            var or4 = new OR4();
            Project.UpdateElementAndFunction(or3, element, or4);
        }

        public static void RemoveInputAt(this OR3 or3, Element element)
        {
            var or2 = new OR();
            Project.UpdateElementAndFunction(or3, element, or2);
        }

        public static void AddInputAt(this OR4 or4, Element element)
        {
            var or5 = new OR5();
            Project.UpdateElementAndFunction(or4, element, or5);
        }

        public static void RemoveInputAt(this OR4 or4, Element element)
        {
            var or3 = new OR3();
            Project.UpdateElementAndFunction(or4, element, or3);
        }

        public static void AddInputAt(this OR5 or5, Element element)
        {
            var or6 = new OR6();
            Project.UpdateElementAndFunction(or5, element, or6);
        }

        public static void RemoveInputAt(this OR5 or5, Element element)
        {
            var or4 = new OR4();
            Project.UpdateElementAndFunction(or5, element, or4);
        }

        public static void AddInputAt(this OR6 or6, Element element)
        {
            var or7 = new OR7();
            Project.UpdateElementAndFunction(or6, element, or7);
        }

        public static void RemoveInputAt(this OR6 or6, Element element)
        {
            var or5 = new OR5();
            Project.UpdateElementAndFunction(or6, element, or5);
        }

        public static void AddInputAt(this OR7 or7, Element element)
        {
            var or8 = new OR8();
            Project.UpdateElementAndFunction(or7, element, or8);
        }

        public static void RemoveInputAt(this OR7 or7, Element element)
        {
            var or6 = new OR6();
            Project.UpdateElementAndFunction(or7, element, or6);
        }

        public static void RemoveInputAt(this OR8 or8, Element element)
        {
            var or7 = new OR7();
            Project.UpdateElementAndFunction(or8, element, or7);
        }

    }
}
