namespace MHB.BL
{
    public class TransactionManager : TransactionManagerBase
    {
        public TransactionManager()
        {
        }

        public TransactionManager(string connectionString)
            : base(connectionString)
        {
        }
    }
}