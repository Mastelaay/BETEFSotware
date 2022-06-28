namespace BET.Domain
{
    public interface IBETUnitOfWork
    {
        void Commit();

        IBETRepository<T> GetRepo<T>() where T : class;
    }
}
