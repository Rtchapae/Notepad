using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Diary.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected void OnPropertyChanged(Expression<Func<object>> propertyExpression)
        {
            MemberExpression expression = ToMemberExpression(propertyExpression);
            if (expression == null)
            {
                return;
            }
            OnPropertyChanged(expression.Member.Name);
        }

        protected static MemberExpression ToMemberExpression(Expression<Func<object>> property)
        {
            var expression = property.Body as MemberExpression;
            if (property.Body is UnaryExpression)
            {
                expression = ((UnaryExpression)property.Body).Operand as MemberExpression;
            }

            return expression;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        ///   Update all properties 
        /// </summary>
        public void RefreshBinding()
        {
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                OnPropertyChanged(property.Name);
            }
        }

        public virtual void Initialize()
        {
        }

        public virtual void Deinitialize()
        {
        }
    }
}
