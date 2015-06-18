using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OES.Modules.Core
{
    public class Result<T> 
    {
        public bool Success { get; set; }

        public List<ResultError> Errors { get; set; }

        public Exception AttachedException { get; set; }

        public T ReturnObject { get; set; }
    }

    public class ResultError
    {

        public ResultError()
        { }

        public static ResultError AddPropertyError<TSource, TProperty>(TSource source,
    Expression<Func<TSource, TProperty>> expression, string message)
        {
            return new ResultError((expression.Body as MemberExpression).Member.Name, message);
        }
        public ResultError(string key, string message)
        {
            Key = key;
            Message = message;
        }
        public ResultError(Exception ex)
        {
            Key = "";
            Message = ex.Message;
        }

        public string Message { get; set; }

        public string Key { get; set; }
    }
}
