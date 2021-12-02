#region Namespace References
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using UKParliament.CodeTest.Pattern.Service.Interfaces;
#endregion

namespace UKParliament.CodeTest.Pattern.Service
{
    public class SmartServiceFactory : ISmartServiceFactory
    {
        public SmartServiceFactory(DbContext dbContext)
        {
            Context = dbContext;
        }

        public virtual ISmartService<T> GetService<T>() where T : class
        {
            return (ISmartService<T>)GetOrAddService(typeof(T), new SmartService<T>(Context));
        }

        public DbContext Context { get; }
        private Dictionary<(Type type, string name), object> services;
        internal object GetOrAddService(Type type, object serivce)
        {
            services ??= new Dictionary<(Type type, string Name), object>();

            if (services.TryGetValue((type, serivce.GetType().FullName), out var service)) return service;
            services.Add((type, serivce.GetType().FullName), serivce);
            return serivce;
        }
    }
}
