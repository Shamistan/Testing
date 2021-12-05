using ApplicationCore.Utility;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private string _errorMessage = string.Empty;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _unitOfWork = new UnitOfWork(context);
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public ICollection<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        //public DataTable SelectSqlRaw(string sqlQuery)
        //{
        //    DataTable dataTable = new DataTable();

        //    var connection = _context.Database.GetDbConnection();

        //    DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);

        //    using (var cmd = dbFactory.CreateCommand())
        //    {
        //        try
        //        {
        //            cmd.Connection = connection;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = sqlQuery;

        //            using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
        //            {
        //                adapter.SelectCommand = cmd;

        //                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();

        //                adapter.Fill(dataTable);

        //                cmd.Connection.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (cmd.Connection.State != ConnectionState.Closed) cmd.Connection.Close();

        //            throw ex;
        //        }
        //    }

        //    return dataTable;
        //}

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T GetByUniqueId(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByUniqueIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }

        public TransResult<T> Add(T entity)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                result.Object = entity;
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> AddUnCommitted(T entity)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _context.Set<T>().Add(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                result.IsSuccess = false;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                throw new Exception(_errorMessage, dbEx);
            }

            return result;
        }

        public async Task<TransResult<T>> AddAsync(T entity)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().Add(entity);
                await _unitOfWork.Commit();
                result.Object = entity;
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> AddRange(ICollection<T> entities)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().AddRange(entities);
                _context.SaveChanges();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> AddRangeUnCommitted(ICollection<T> entities)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().AddRange(entities);
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public async Task<TransResult<T>> AddRangeAsync(ICollection<T> entities)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                await _context.Set<T>().AddRangeAsync(entities);
                await _unitOfWork.Commit();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> Update(T updated)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            if (updated == null)
            {
                return new TransResult<T>() { IsSuccess = false }; ;
            }
            try
            {
                _context.Set<T>().Attach(updated);
                _context.Entry(updated).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> UpdateUnCommitted(T updated)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().Attach(updated);
                _context.Entry(updated).State = EntityState.Modified;
            }
            catch (DbEntityValidationException dbEx)
            {
                result.IsSuccess = false;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                throw new Exception(_errorMessage, dbEx);
            }
            return result;
        }

        public TransResult<T> UpdateRange(ICollection<T> entities)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                foreach (var updated in entities)
                {
                    _context.Set<T>().Attach(updated);
                    _context.Entry(updated).State = EntityState.Modified;
                }

                _context.SaveChanges();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> UpdateRangeUnCommitted(ICollection<T> entities)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                foreach (var updated in entities)
                {
                    _context.Set<T>().Attach(updated);
                    _context.Entry(updated).State = EntityState.Modified;
                }
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public async Task<TransResult<T>> UpdateAsync(T updated)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            if (updated == null)
            {
                return new TransResult<T>() { IsSuccess = false }; ;
            }
            try
            {
                _context.Set<T>().Attach(updated);
                _context.Entry(updated).State = EntityState.Modified;
                await _unitOfWork.Commit();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public async Task<TransResult<T>> UpdateRangeAsync(ICollection<T> entities)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                foreach (var updated in entities)
                {
                    _context.Set<T>().Attach(updated);
                    _context.Entry(updated).State = EntityState.Modified;
                }

                await _unitOfWork.Commit();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> Delete(T t)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().Remove(t);
                _context.SaveChanges();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> DeleteUnCommitted(T t)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().Remove(t);
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public async Task<TransResult<T>> DeleteAsync(T t)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().Remove(t);

                await _unitOfWork.Commit();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> DeleteRange(ICollection<T> entities)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().RemoveRange(entities);
                _context.SaveChanges();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public TransResult<T> DeleteRangeUnCommitted(ICollection<T> entities)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().RemoveRange(entities);
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public async Task<TransResult<T>> DeleteRangeAsync(ICollection<T> entities)
        {
            TransResult<T> result = new TransResult<T>() { IsSuccess = true };

            try
            {
                _context.Set<T>().RemoveRange(entities);

                await _unitOfWork.Commit();
            }
            catch
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public IEnumerable<T> Filter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? page = null,
            int? pageSize = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includeProperties != null)
            {
                foreach (
                    var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query.ToList();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            var exist = _context.Set<T>().Where(predicate);
            return exist.Any() ? true : false;
        }

    }

}
