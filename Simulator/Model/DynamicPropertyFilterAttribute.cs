namespace Simulator.Model
{
    /// <summary>
    /// ������� ��� ��������� ����������� ������������ �������
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DynamicPropertyFilterAttribute : Attribute
    {
        string propertyName;
        ///<summary>
        ///�������� ��������, �� �������� ����� �������� ���������
        ///</summary>
        public string PropertyName
        {
            get { return propertyName; }
        }

        string showOn;

        /// <summary>
        /// �������� ��������, �� �������� ������� ��������� 
        /// (����� �������, ���� ���������), ��� ������� ��������, �
        /// �������� �������� �������, ����� ������. 
        /// </summary>
        public string ShowOn
        {
            get { return showOn; }
        }

        ///<summary>
        /// �����������  
        ///</summary>
        ///<param name="propName">�������� ��������, �� �������� ����� �������� ���������</param>
        ///<param name="value">�������� �������� (����� �������, ���� ���������), ��� ������� ��������, � �������� �������� �������, ����� ������.</param>
        public DynamicPropertyFilterAttribute(string propertyName, string value)
        {
            this.propertyName = propertyName;
            showOn = value;
        }
    }
}
