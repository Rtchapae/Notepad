using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Diary.Attributes;

namespace Diary
{
    public sealed class DependencyContainer : IDependencyContainer
    {
        readonly Dictionary<Type, Func<object>> registeredCreators = new Dictionary<Type, Func<object>>();
        readonly Dictionary<Type, Func<object>> registeredSingletonCreators = new Dictionary<Type, Func<object>>();
        readonly Dictionary<Type, object> registeredSingletons = new Dictionary<Type, object>();
        object locker = new object();

        /// <summary>
        /// Register a type with the container. This is only necessary if the 
        /// type has a non-default constructor or needs to be customized in some fashion.
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type<typeparam>
        /// <typeparam name="TImpl">Type to create</typeparam>
        public void Register<TAbstraction, TImpl>()
            where TImpl : new()
        {
            registeredCreators.Add(typeof(TAbstraction), () => new TImpl());
        }

        /// <summary>
        /// Register a type with the container. This is only necessary if the 
        /// type has a non-default constructor or needs to be customized in some fashion.
        /// </summary>
        /// <param name="creator">Function to create the given type.</param>
        /// <typeparam name="T">Type to create</typeparam>
        public void Register<T>(Func<object> creator)
        {
            registeredCreators.Add(typeof(T), creator);
        }

        /// <summary>
        /// Register a type with the container. This is only necessary if the 
        /// type has a non-default constructor or needs to be customized in some fashion.
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type<typeparam>
        public void RegisterInstance<TAbstraction>(object instance)
        {
            registeredCreators.Add(typeof(TAbstraction), () => instance);
        }

        /// <summary>
        /// Creates a factory for a type so it may be created through
        /// the container without taking a dependency on the container directly.
        /// </summary>
        /// <returns>Creator function</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public Func<T> FactoryFor<T>()
        {
            return () => Resolve<T>();
        }

        /// <summary>
        /// Creates the given type, either through a registered function
        /// or through the default constructor.
        /// </summary>
        /// <typeparam name="T">Type to create</typeparam>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Creates the given type, either through a registered function
        /// or through the default constructor.
        /// </summary>
        /// <param name="type">Type to create</param>
        public object Resolve(Type type)
        {
            object result = null;
            var targetType = type;

            TypeInfo typeInfo = type.GetTypeInfo();

            if (registeredSingletonCreators.TryGetValue(type, out Func<object> creator))
            {
                lock (locker)
                {
                    if (registeredSingletons.ContainsKey(type))
                    {
                        result = registeredSingletons[type];
                    }
                    else
                    {
                        result = registeredSingletonCreators[type]();
                        registeredSingletons.Add(type, result);
                    }
                }

                if (result != null)
                {
                    targetType = result.GetType();
                }
            }
            else
            if (registeredCreators.TryGetValue(type, out creator))
            {
                result = registeredCreators[type]();

                if (result != null)
                {
                    targetType = result.GetType();
                }
            }
            else
            {
                var ctors = typeInfo.DeclaredConstructors.Where(c => c.IsPublic).ToArray();
                var ctor = ctors.FirstOrDefault(c => c.GetParameters().Length == 0);

                if (ctor != null || ctors.Count() == 0)
                {
                    result = Activator.CreateInstance(type);
                }
                else
                {
                    // Pick the first constructor found and create any parameters.
                    ctor = ctors[0];
                    var parameters = new List<object>();
                    foreach (var p in ctor.GetParameters())
                    {
                        parameters.Add(Resolve(p.ParameterType));
                    }

                    result = Activator.CreateInstance(type, parameters.ToArray());
                }
            }

            //Create [Resolve] marked property
            var props = targetType.GetRuntimeProperties()
                .Where(x => x.CanWrite && x.IsDefined(typeof(ResolveAttribute)));

            foreach (var item in props)
            {
                item.SetValue(result, Resolve(item.PropertyType));
            }

            return result;
        }

        public void Clear()
        {
            registeredCreators.Clear();
            registeredSingletonCreators.Clear();
            registeredSingletons.Clear();
        }

        public void RegisterSingleton(Type tInterface, object service)
        {
            RegisterSingleton(tInterface, () => service);
        }

        public void RegisterSingleton<TAbstraction>(TAbstraction service) where TAbstraction : class
        {
            registeredSingletonCreators.Add(typeof(TAbstraction), () => service);
        }
        public void RegisterSingleton<TAbstraction, TImpl>() where TAbstraction : class where TImpl : new()
        {
            registeredSingletonCreators.Add(typeof(TAbstraction), () => new TImpl());
        }

        public void RegisterSingleton(Type tInterface, Func<object> serviceConstructor)
        {
            registeredSingletonCreators.Add(tInterface, serviceConstructor);
        }

        public void RegisterSingleton<TAbstraction>(Func<TAbstraction> serviceConstructor) where TAbstraction : class
        {
            registeredSingletonCreators.Add(typeof(TAbstraction), serviceConstructor);
        }
    }
}
