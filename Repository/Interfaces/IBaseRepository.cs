namespace JwtApplication.Repository.Interfaces
{
    public interface IBaseRepository<T, ID> where T : class
    {
        public Task<bool> Add(T entity);
        public Task<bool> AddRange(List<T> entityList);
        public Task<bool> Update(T entity);
        public Task<bool> Delete(T entity);
        public Task<IEnumerable<T>> FindAll();
        public Task<T> FindById(ID entityId);
        public Task<bool> IsExists(ID entityId);
    }
}
