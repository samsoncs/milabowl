using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Milabowl.Test.Helpers
{
    public static class NotNullAsserter
    {
        public static void AssertAllPropertiesNotNull<T>(this T obj)
        {
            var propertiesNotNull = typeof(T).GetProperties()
                .Where(p =>
                    (p.PropertyType == typeof(string) || !typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                    && p.PropertyType != typeof(object)
                    && p.GetValue(obj) == null
                )
                .ToList();

            if (propertiesNotNull.Any())
            {
                throw new Exception(
                    $"Following properties are null: {string.Join(", ", propertiesNotNull.Select(p => p.Name))}"
                );
            }
        }
    }
}
