using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Reflection;
//using System.Xml.Serialization;

namespace RedBook.DataAccess
{
	public class GenericRepository<TEntity> where TEntity : class
	{
		internal DataContext Context;
		internal DbSet<TEntity> DbSet;

		private DbSet<TEntity> _privateDbSet;

		public GenericRepository(DataContext context)
		{
			Context = context;
			DbSet = context.Set<TEntity>();
		}

		public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
		{


			IQueryable<TEntity> query = DbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}

			query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
			//code above is equivalent to code below
			//foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			//{
			//    query = query.Include(includeProperty);
			//}

			if (orderBy != null)
			{
				return orderBy(query);
			}
			try
			{
				return query;
			}
			catch
			{
				throw new ArgumentException("Please consider checking the parameters.");
			}
		}

		//a generic method that returns List<Object> - involveds paginations
		public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int iDisplayStart = 0, int iDisplayLength = 0, string includeProperties = "", string sortProperty = "", string sortOrder = "")
		{
			IEnumerable<TEntity> tEntities = Get(filter, orderBy, includeProperties);
			tEntities = GetIEnumerableRange(tEntities, iDisplayStart, iDisplayLength);

			if (!string.IsNullOrEmpty(sortProperty) && !string.IsNullOrEmpty(sortOrder))
			{
				tEntities = Sorter(tEntities, sortProperty, sortOrder);
			}
			return tEntities.ToList();
		}


		public virtual IEnumerable<TEntity> GetIEnumerableRange(IEnumerable<TEntity> tEntities, int iDisplayStart, int iDisplayLength)
		{
			return iDisplayLength > 0 ? tEntities.Skip(iDisplayStart).Take(iDisplayLength) : tEntities;
		}



		//used for sorting based on individual property. This cannot support multiple property
		public virtual IEnumerable<TEntity> Sort(IEnumerable<TEntity> tEntities, string sortProperty, string sortOrder)
		{
			var param = Expression.Parameter(typeof(TEntity), string.Empty);
			IEnumerable<TEntity> enumerable = tEntities as IList<TEntity> ?? tEntities.ToList();
			try
			{
				var property = Expression.Property(param, sortProperty);

				var sortLambda = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(property, typeof(object)), param);

				if (sortProperty.Length > 1 && sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
				{
					return enumerable.AsQueryable().OrderByDescending(sortLambda);
				}
				return enumerable.AsQueryable().OrderBy(sortLambda);

			}
			catch (ArgumentException)
			{
				return enumerable;
			}
		}



		//used for sorting based on individual property. This cannot support multiple property
		public virtual IEnumerable<TEntity> Sorter(IEnumerable<TEntity> tEntities, string sortProperty, string sortOrder)
		{
			var sorter = EntitySorter<TEntity>.OrderBy(sortProperty);

			if (sortProperty.Length > 1 && sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
			{
				sorter = EntitySorter<TEntity>.OrderByDescending(sortProperty);
			}
			return sorter.Sort(tEntities.AsQueryable());

		}



		public virtual int GetCount(Expression<Func<TEntity, bool>> filter, string searchText = null)
		{
			if (string.IsNullOrEmpty(searchText))
			{
				return Get(filter).Count();
			}
			return Get().Count();
		}

		//we will use this method when we need to get all properties of a specific object - objects with relationship as --> includeProperties
		public virtual TEntity GetOne(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
		{
			try
			{
				return Get(filter, includeProperties: includeProperties).SingleOrDefault();

			}
			catch
			{
				throw new ArgumentException("Please check if the object exists.");
			}
		}

		//we need to get a simple object without relationship to other objects by filter ---> typical example is by Object Name --> SchoolName as example
		public virtual TEntity GetOne(Expression<Func<TEntity, bool>> filter)
		{
			try
			{
				return DbSet.SingleOrDefault(filter);

			}
			catch
			{
				throw new ArgumentException("Please check if the object exists.");
			}
		}

		//we need to get a simple object without relationship to other objects by id
		public virtual TEntity GetById(object id)
		{
			try
			{
				return DbSet.Find(id);
			}
			catch
			{
				throw new ArgumentException("Please check if the object exists.");
			}
		}


		public virtual void Insert(TEntity entity)
		{
			DbSet.Add(entity);
			try
			{
				Save();
			}
			catch
			{
				throw new ArgumentException("Please check if all the properties are valid.");
			}

		}

		public virtual void Save()
		{
			try
			{
				Context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
					}
				}
			}

		}

		public virtual bool Delete(object id)
		{
			try
			{
				TEntity entityToDelete = DbSet.Find(id);
				Delete(entityToDelete);
				Save();
				return true;
			}
			catch
			{
				throw new ArgumentException("System Encounters Error While Deleting Object with id = " + id + ". Please double check if the object Exists.");
			}
		}



		public virtual void Delete(TEntity entityToDelete)
		{
			if (Context.Entry(entityToDelete).State == EntityState.Detached)
			{
				DbSet.Attach(entityToDelete);

			}
			DbSet.Remove(entityToDelete);
			try
			{
				Save();
			}
			catch
			{
				throw new ArgumentException("System Encounters Error While Deleting Object " + entityToDelete + ". Please double check if the object Exists.");
			}
		}

		public virtual void Update(TEntity entityToUpdate)
		{
			using (var privateContext = new DataContext())
			{
				try
				{
					_privateDbSet = privateContext.Set<TEntity>();
					_privateDbSet.Attach(entityToUpdate); // works fine
					privateContext.Entry(entityToUpdate).State = EntityState.Modified;
					privateContext.SaveChanges();
				}
				catch
				{
					((IDisposable)privateContext).Dispose();
					throw new ArgumentException("System Encounters Error while updating the " + entityToUpdate + ".");
				}
				finally
				{
					((IDisposable)privateContext).Dispose();
				}

			}
		}


		public virtual bool Exist(Expression<Func<TEntity, bool>> filter)
		{
			return (GetOne(filter) != null);
		}



		public virtual bool ExecuteProcedure(string query)
		{
			return (Context.Database.ExecuteSqlCommand(query) != 0);
		}

		public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
		{
			return DbSet.SqlQuery(query, parameters).ToList();
		}



		//		public virtual string ToXML(TEntity entity)
		//		{
		//			var stringwriter = new StringWriter();
		//			var serializer = new XmlSerializer(entity.GetType());
		//			serializer.Serialize(stringwriter, entity);
		//			return stringwriter.ToString();
		//		}
		//
		//		public virtual TEntity LoadFromXMLString(string xmlText)
		//		{
		//			var stringReader = new StringReader(xmlText);
		//			var serializer = new XmlSerializer(typeof(TEntity));
		//			return serializer.Deserialize(stringReader) as TEntity;
		//		}
	}

	#region Sort Helper
	public interface IEntitySorter<TEntity>
	{
		IOrderedQueryable<TEntity> Sort(IQueryable<TEntity> collection);
	}

	internal enum SortDirection
	{
		Ascending,
		Descending
	}

	public static class EntitySorter<T>
	{
		public static IEntitySorter<T> AsQueryable()
		{
			return new EmptyEntitySorter();
		}

		public static IEntitySorter<T> OrderBy<TKey>(
			Expression<Func<T, TKey>> keySelector)
		{
			return new OrderBySorter<T, TKey>(keySelector,
				SortDirection.Ascending);
		}

		public static IEntitySorter<T> OrderByDescending<TKey>(
			Expression<Func<T, TKey>> keySelector)
		{
			return new OrderBySorter<T, TKey>(keySelector,
				SortDirection.Descending);
		}

		public static IEntitySorter<T> OrderBy(string propertyName)
		{
			//var builder = new EntitySorterBuilder<T>(propertyName);
			var builder = new EntitySorterBuilder<T>(propertyName) { Direction = SortDirection.Ascending };

			return builder.BuildOrderByEntitySorter();
		}

		public static IEntitySorter<T> OrderByDescending(
			string propertyName)
		{
			//var builder = new EntitySorterBuilder<T>(propertyName);
			var builder = new EntitySorterBuilder<T>(propertyName) { Direction = SortDirection.Descending };

			return builder.BuildOrderByEntitySorter();
		}

		private sealed class EmptyEntitySorter : IEntitySorter<T>
		{
			public IOrderedQueryable<T> Sort(
				IQueryable<T> collection)
			{
				const string exceptionMessage = "OrderBy should be called.";

				throw new InvalidOperationException(exceptionMessage);
			}
		}
	}

	public static class EntitySorterExtensions
	{
		public static IEntitySorter<T> OrderBy<T, TKey>(
			this IEntitySorter<T> sorter,
			Expression<Func<T, TKey>> keySelector)
		{
			return EntitySorter<T>.OrderBy(keySelector);
		}

		public static IEntitySorter<T> OrderByDescending<T, TKey>(
			this IEntitySorter<T> sorter,
			Expression<Func<T, TKey>> keySelector)
		{
			return EntitySorter<T>.OrderByDescending(keySelector);
		}

		public static IEntitySorter<T> ThenBy<T, TKey>(
			this IEntitySorter<T> sorter,
			Expression<Func<T, TKey>> keySelector)
		{
			return new ThenBySorter<T, TKey>(sorter,
				keySelector, SortDirection.Ascending);
		}

		public static IEntitySorter<T> ThenByDescending<T, TKey>(
			this IEntitySorter<T> sorter,
			Expression<Func<T, TKey>> keySelector)
		{
			return new ThenBySorter<T, TKey>(sorter,
				keySelector, SortDirection.Descending);
		}

		public static IEntitySorter<T> ThenBy<T>(
			this IEntitySorter<T> sorter, string propertyName)
		{
			//var builder = new EntitySorterBuilder<T>(propertyName);
			var builder = new EntitySorterBuilder<T>(propertyName) { Direction = SortDirection.Ascending };

			return builder.BuildThenByEntitySorter(sorter);
		}

		public static IEntitySorter<T> ThenByDescending<T>(
			this IEntitySorter<T> sorter, string propertyName)
		{
			//var builder = new EntitySorterBuilder<T>(propertyName);
			var builder = new EntitySorterBuilder<T>(propertyName) { Direction = SortDirection.Descending };

			return builder.BuildThenByEntitySorter(sorter);
		}
	}

	internal class OrderBySorter<T, TKey> : IEntitySorter<T>
	{
		private readonly Expression<Func<T, TKey>> _keySelector;
		private readonly SortDirection _direction;

		public OrderBySorter(Expression<Func<T, TKey>> selector,
			SortDirection direction)
		{
			_keySelector = selector;
			_direction = direction;
		}

		public IOrderedQueryable<T> Sort(IQueryable<T> col)
		{
			if (_direction == SortDirection.Ascending)
			{
				return col.OrderBy(_keySelector);
				//code above is equivalent to code below
				//return Queryable.OrderBy(col, _keySelector);
			}
			return col.OrderByDescending(_keySelector);
			//code above is equivalent to code below
			//return Queryable.OrderByDescending(col, _keySelector);
		}
	}

	internal sealed class ThenBySorter<T, TKey> : IEntitySorter<T>
	{
		private readonly IEntitySorter<T> _baseSorter;
		private readonly Expression<Func<T, TKey>> _keySelector;
		private readonly SortDirection _direction;

		public ThenBySorter(IEntitySorter<T> baseSorter,
			Expression<Func<T, TKey>> selector, SortDirection direction)
		{
			_baseSorter = baseSorter;
			_keySelector = selector;
			_direction = direction;
		}

		public IOrderedQueryable<T> Sort(IQueryable<T> col)
		{
			var sorted = _baseSorter.Sort(col);

			if (_direction == SortDirection.Ascending)
			{
				return sorted.ThenBy(_keySelector);
				//code above is equivalent to code below
				//return Queryable.ThenBy(sorted, _keySelector);
			}
			return sorted.ThenByDescending(_keySelector);
			//code above is equivalent to code below
			//return Queryable.ThenByDescending(sorted, _keySelector);
		}
	}

	internal class EntitySorterBuilder<T>
	{
		private readonly Type _keyType;
		private readonly LambdaExpression _keySelector;

		public EntitySorterBuilder(string propertyName)
		{
			List<MethodInfo> propertyAccessors =
				GetPropertyAccessors(propertyName);

			_keyType = propertyAccessors.Last().ReturnType;

			ILambdaBuilder builder = CreateLambdaBuilder(_keyType);

			_keySelector =
				builder.BuildLambda(propertyAccessors);
		}

		private interface ILambdaBuilder
		{
			LambdaExpression BuildLambda(
				IEnumerable<MethodInfo> propertyAccessors);
		}

		public SortDirection Direction { get; set; }

		public IEntitySorter<T> BuildOrderByEntitySorter()
		{
			Type[] typeArgs = { typeof(T), _keyType };

			Type sortType =
				typeof(OrderBySorter<,>).MakeGenericType(typeArgs);

			return (IEntitySorter<T>)Activator.CreateInstance(sortType,
				_keySelector, Direction);
		}

		public IEntitySorter<T> BuildThenByEntitySorter(
			IEntitySorter<T> baseSorter)
		{
			Type[] typeArgs = { typeof(T), _keyType };

			Type sortType =
				typeof(ThenBySorter<,>).MakeGenericType(typeArgs);

			return (IEntitySorter<T>)Activator.CreateInstance(sortType,
				baseSorter, _keySelector, Direction);
		}

		private static ILambdaBuilder CreateLambdaBuilder(Type keyType)
		{
			Type[] typeArgs = { typeof(T), keyType };

			Type builderType =
				typeof(LambdaBuilder<>).MakeGenericType(typeArgs);

			return (ILambdaBuilder)Activator.CreateInstance(builderType);
		}

		private static List<MethodInfo> GetPropertyAccessors(
			string propertyName)
		{
			try
			{
				return GetPropertyAccessorsFromChain(propertyName);
			}
			catch (InvalidOperationException ex)
			{
				string message = propertyName +
					" could not be parsed. " + ex.Message;

				// We throw a more expressive exception at this level.
				throw new ArgumentException(message, "propertyName");
			}
		}

		private static List<MethodInfo> GetPropertyAccessorsFromChain(
			string propertyNameChain)
		{
			var propertyAccessors = new List<MethodInfo>();

			var declaringType = typeof(T);

			foreach (string name in propertyNameChain.Split('.'))
			{
				var accessor = GetPropertyAccessor(declaringType, name);

				propertyAccessors.Add(accessor);

				declaringType = accessor.ReturnType;
			}

			return propertyAccessors;
		}

		private static MethodInfo GetPropertyAccessor(Type declaringType,
			string propertyName)
		{
			var prop = GetPropertyByName(declaringType, propertyName);

			return GetPropertyGetter(prop);
		}

		private static PropertyInfo GetPropertyByName(Type declaringType,
			string propertyName)
		{
			//BindingFlags flags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public;
			const BindingFlags flags = BindingFlags.IgnoreCase |
				BindingFlags.Instance |
				BindingFlags.Public;

			var prop = declaringType.GetProperty(propertyName, flags);

			if (prop == null)
			{
				string exceptionMessage = string.Format(
					"{0} does not contain a property named '{1}'.",
					declaringType, propertyName);

				throw new InvalidOperationException(exceptionMessage);
			}

			return prop;
		}

		private static MethodInfo GetPropertyGetter(PropertyInfo property)
		{
			var propertyAccessor = property.GetGetMethod();

			if (propertyAccessor == null)
			{
				string exceptionMessage = string.Format(
					"The property '{1}' does not contain a getter.",
					property.Name);

				throw new InvalidOperationException(exceptionMessage);
			}

			return propertyAccessor;
		}

		private sealed class LambdaBuilder<TKey> : ILambdaBuilder
		{
			public LambdaExpression BuildLambda(
				IEnumerable<MethodInfo> propertyAccessors)
			{
				ParameterExpression parameterExpression =
					Expression.Parameter(typeof(T), "entity");

				Expression propertyExpression = BuildPropertyExpression(
					propertyAccessors, parameterExpression);

				return Expression.Lambda<Func<T, TKey>>(
					propertyExpression, new[] { parameterExpression });
			}

			private static Expression BuildPropertyExpression(
				IEnumerable<MethodInfo> propertyAccessors,
				ParameterExpression parameterExpression)
			{
				Expression propertyExpression = null;

				foreach (var propertyAccessor in propertyAccessors)
				{
					var innerExpression =
						propertyExpression ?? parameterExpression;

					propertyExpression = Expression.Property(
						innerExpression, propertyAccessor);
				}

				return propertyExpression;
			}
		}
	}
	#endregion End of Sort Helper
}
