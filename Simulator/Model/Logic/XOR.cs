using Simulator.Model.Common;
using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class XOR : CommonLogic, IAddInput { public XOR() : base(LogicFunction.Xor, 2) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }
    }

    public class XOR3 : CommonLogic, IAddInput, IRemoveInput { public XOR3() : base(LogicFunction.Xor, 3) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class XOR4 : CommonLogic, IAddInput, IRemoveInput { public XOR4() : base(LogicFunction.Xor, 4) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class XOR5 : CommonLogic, IAddInput, IRemoveInput { public XOR5() : base(LogicFunction.Xor, 5) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class XOR6 : CommonLogic, IAddInput, IRemoveInput { public XOR6() : base(LogicFunction.Xor, 6) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class XOR7 : CommonLogic, IAddInput, IRemoveInput { public XOR7() : base(LogicFunction.Xor, 7) { }

        public void AddInput(Element element)
        {
            this.AddInputAt(element);
        }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public class XOR8 : CommonLogic, IRemoveInput { public XOR8() : base(LogicFunction.Xor, 8) { }

        public void RemoveInput(Element element)
        {
            this.RemoveInputAt(element);
        }
    }

    public static class XorHelper
    {
        public static void AddInputAt(this Model.Logic.XOR xor2, Element element)
        {
            var xor3 = new Model.Logic.XOR3();
            Project.UpdateElementAndFunction(xor2, element, xor3);
        }

        public static void AddInputAt(this Model.Logic.XOR3 xor3, Element element)
        {
            var xor4 = new Model.Logic.XOR4();
            Project.UpdateElementAndFunction(xor3, element, xor4);
        }

        public static void RemoveInputAt(this Model.Logic.XOR3 xor3, Element element)
        {
            var xor2 = new Model.Logic.XOR();
            Project.UpdateElementAndFunction(xor3, element, xor2);
        }

        public static void AddInputAt(this Model.Logic.XOR4 xor4, Element element)
        {
            var xor5 = new Model.Logic.XOR5();
            Project.UpdateElementAndFunction(xor4, element, xor5);
        }

        public static void RemoveInputAt(this Model.Logic.XOR4 xor4, Element element)
        {
            var xor3 = new Model.Logic.XOR3();
            Project.UpdateElementAndFunction(xor4, element, xor3);
        }

        public static void AddInputAt(this Model.Logic.XOR5 xor5, Element element)
        {
            var xor6 = new Model.Logic.XOR6();
            Project.UpdateElementAndFunction(xor5, element, xor6);
        }

        public static void RemoveInputAt(this Model.Logic.XOR5 xor5, Element element)
        {
            var xor4 = new Model.Logic.XOR4();
            Project.UpdateElementAndFunction(xor5, element, xor4);
        }

        public static void AddInputAt(this Model.Logic.XOR6 xor6, Element element)
        {
            var xor7 = new Model.Logic.XOR7();
            Project.UpdateElementAndFunction(xor6, element, xor7);
        }

        public static void RemoveInputAt(this Model.Logic.XOR6 xor6, Element element)
        {
            var xor5 = new Model.Logic.XOR5();
            Project.UpdateElementAndFunction(xor6, element, xor5);
        }

        public static void AddInputAt(this Model.Logic.XOR7 xor7, Element element)
        {
            var xor8 = new Model.Logic.XOR8();
            Project.UpdateElementAndFunction(xor7, element, xor8);
        }

        public static void RemoveInputAt(this Model.Logic.XOR7 xor7, Element element)
        {
            var xor6 = new Model.Logic.XOR6();
            Project.UpdateElementAndFunction(xor7, element, xor6);
        }

        public static void RemoveInputAt(this Model.Logic.XOR8 xor8, Element element)
        {
            var xor7 = new Model.Logic.XOR7();
            Project.UpdateElementAndFunction(xor8, element, xor7);
        }

    }
}
