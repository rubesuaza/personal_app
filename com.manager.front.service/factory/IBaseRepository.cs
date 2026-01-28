using com.manager.front.service.factory.dtos;

namespace com.manager.front.service.factory
{
	public interface IBaseRepository<T> where T : class
	{
		Task<ObjectResponse<IEnumerable<T>>> GetAllAsync(string endpoint);
		Task<ObjectResponse<T>> GetByIdAsync(string endpoint);
		Task<ObjectResponse<T>> AddAsync(T entity, string endpoint);
		Task<ObjectResponse<T>> UpdateAsync(T entity, string endpoint);
		Task DeleteAsync(int id, string endpoint);
	}
}
