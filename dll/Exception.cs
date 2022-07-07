namespace lib;
[System.Serializable]
public class IntegralException : System.Exception
{
    public IntegralException() { }
    public IntegralException(string message) : base(message) { }
    public IntegralException(string message, System.Exception inner) : base(message, inner) { }
    protected IntegralException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}