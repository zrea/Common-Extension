using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Carpass.Common.Extensions
{
     public static class IQueryableExtensions
     {
         public static T First<T>(this IQueryable<T> list, Exception e)
         {
             try
             {
                 return list.First();
             }
             catch
             {
                 throw e;
             }
         }

         public static T First<T>(this IQueryable<T> list, Expression<Func<T, bool>> predicate, Exception e)
         {
             try
             {
                 return list.First(predicate);
             }
             catch
             {
                 throw e;
             }
         }

         public static T Single<T>(this IQueryable<T> list, Exception e)
         {
             try
             {
                 return list.Single();
             }
             catch
             {
                 throw e;
             }
         }
         
         public static T Single<T>(this IQueryable<T> list, Expression<Func<T, bool>> predicate, Exception e)
         {
             try
             {
                 return list.Single(predicate);
             }
             catch
             {
                 throw e;
             }
         }

         public static T SingleOrDefault<T>(this IQueryable<T> list, Exception e)
         {
             try
             {
                 return list.SingleOrDefault();
             }
             catch
             {
                 throw e;
             }
         }

         public static T SingleOrDefault<T>(this IQueryable<T> list, 
             Expression<Func<T, bool>> predicate, Exception e)
         {
             try
             {
                 return list.SingleOrDefault(predicate);
             }
             catch
             {
                 throw e;
             }
         }
    }
}
