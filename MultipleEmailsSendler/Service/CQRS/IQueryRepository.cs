namespace MultipleEmailsSendler.Service.Interfaces
{
    public interface IQueryRepository<TEntity> where TEntity : class
    {
        void Remove(TEntity item);
        void Update(TEntity item);

        void Create(TEntity item);

    }
}
