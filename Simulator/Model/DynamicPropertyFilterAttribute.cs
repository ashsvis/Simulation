namespace Simulator.Model
{
    /// <summary>
    /// Атрибут для поддержки динамически показываемых свойств
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DynamicPropertyFilterAttribute : Attribute
    {
        string propertyName;
        ///<summary>
        ///Название свойства, от которого будет зависить видимость
        ///</summary>
        public string PropertyName
        {
            get { return propertyName; }
        }

        string showOn;

        /// <summary>
        /// Значения свойства, от которого зависит видимость 
        /// (через запятую, если несколько), при котором свойство, к
        /// которому применен атрибут, будет видимо. 
        /// </summary>
        public string ShowOn
        {
            get { return showOn; }
        }

        ///<summary>
        /// Конструктор  
        ///</summary>
        ///<param name="propName">Название свойства, от которого будет зависеть видимость</param>
        ///<param name="value">Значения свойства (через запятую, если несколько), при котором свойство, к которому применен атрибут, будет видимо.</param>
        public DynamicPropertyFilterAttribute(string propertyName, string value)
        {
            this.propertyName = propertyName;
            showOn = value;
        }
    }
}
