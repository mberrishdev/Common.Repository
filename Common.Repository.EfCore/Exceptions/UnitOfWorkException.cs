namespace Common.Repository.EfCore.Exceptions
{
    public class UnitOfWorkException : Exception
    {
        public UnitOfWorkException(string message) : base(message) { }
    }
}
