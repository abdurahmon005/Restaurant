using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models
{
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        private ResponseModel(bool isSuccess, string message, T? data, IEnumerable<string> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public static ResponseModel<T> Ok(T data, string message = "")
        {
            return new ResponseModel<T>(true, message, data, Enumerable.Empty<string>());
        }

        public static ResponseModel<T> Fail(string message, IEnumerable<string> errors)
        {
            return new ResponseModel<T>(false, message, default, errors);
        }

        public static ResponseModel<T> Fail(string message, string error)
        {
            return new ResponseModel<T>(false, message, default, new[] { error });
        }
    }
}
