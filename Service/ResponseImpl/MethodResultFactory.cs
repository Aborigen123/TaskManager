namespace Service.ResponseImpl
{
    public class MethodResultFactory
    {
        public MethodResult<T> Create<T>()
            where T : class
        {
            return new MethodResult<T>();
        }
    }
}