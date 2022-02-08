﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Get Entity with id
        /// </summary>
        /// <param name="id">The id of entity</param>
        /// <returns>Entity or null</returns>
        Task<T> Get(string id);

        /// <summary>
        /// Get List of entities
        /// </summary>
        /// <returns>List of entities</returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Add new entity to database
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        /// <returns>The added entity</returns>
        Task<T> Add(T entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        void Delete(T entity);

        /// <summary>
        /// Delete entity with only id specified
        /// </summary>
        /// <param name="id">Id of entity to be deleted</param>
        void Delete(string id);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">New information of entity to be updated</param>
        void Update(T entity);
    }
}
