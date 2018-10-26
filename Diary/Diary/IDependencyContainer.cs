using System;
using System.Collections.Generic;
using System.Text;

namespace Diary
{
   public interface IDependencyContainer
   {
       object Resolve(Type type);
       void Clear();
       void Register<T>(Func<object> creator);
       void Register<TAbstraction, TImpl>() where TImpl : new();
       void RegisterSingleton(Type tInterface, object service);
       void RegisterSingleton(Type tInterface, Func<object> serviceConstructor);

   }

}
