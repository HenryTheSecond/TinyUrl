namespace Shared.Attributes
{
    public class ExportAttribute : Attribute
    {
        public LifeCycle LifeCycle { get; set; }
    }

    public enum LifeCycle
    {
        SINGLETON, SCOPE, TRANSIENT
    }
}
