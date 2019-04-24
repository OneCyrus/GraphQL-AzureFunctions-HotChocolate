using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLAzureFunctions
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute(Type type)
        {
            Type = type;
        }
        public Type Type { get; }
    }
}
