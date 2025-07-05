
using SimpleAgenda.DTOS.Publics;

namespace SimpleAgenda.Interfaces
{
    /// <summary>
    /// Ensure the following contract interface.
    /// The class implementing this interface must provide a method to apply filters to solve specific queries.
    /// </summary>
    /// <typeparam name="T">The provided class</typeparam>
    internal interface IQueryFilter<T> where T : class
    {
        /// <summary>
        /// Applies the specified filter criteria to the queryable data source.
        /// </summary>
        /// <remarks>This method is designed to apply filtering logic based on the properties of the  <see
        /// cref="QueryDto"/> parameter. The caller is responsible for ensuring that  the <see cref="QueryDto"/>
        /// contains valid filtering criteria.</remarks>
        /// <param name="param">The filter parameters encapsulated in a <see cref="QueryDto"/> object.  This object defines the filtering
        /// conditions to be applied.</param>
        /// <returns>An <see cref="IQueryable{T}"/> representing the filtered data source. The returned queryable reflects the
        /// applied filter criteria.</returns>
        IQueryable<T> ApplyFilter(QueryDto param);
    }
}
