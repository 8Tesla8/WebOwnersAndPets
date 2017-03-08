using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repository;

namespace Domain.ErrorsWriter
{
    public static class ErrorsWriter
    {
        public static string GetErrors(List<string> errors)
        {
            string error = null;

            foreach (var item in errors)
            {
                error += item;
            }

            return error;
        }
    }
}
