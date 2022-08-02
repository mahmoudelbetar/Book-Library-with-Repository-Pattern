using B_Gallery.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Gallery.Utility
{
    public static class Extensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.Set<T>(key, value);
        }
        public static T Get<T>(this ISession session, string key)
        {
            return session.Get<T>(key);
        }
    }
}
