using System;
using System.Collections.Generic;
using System.Linq;

namespace MobilePlayer.Models.Extentions
{
    public static class LinqExtentions
    {
        public static (IEnumerable<T>, IEnumerable<T>) Split<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            var fst = new LinkedList<T>();
            var snd = new LinkedList<T>();
            
            foreach (var elem in source)
            {
                if (predicate(elem))
                {
                    fst.AddLast(elem);
                }
                else
                {
                    snd.AddLast(elem);
                }
            }

            return (fst, snd);
        }
    }
}