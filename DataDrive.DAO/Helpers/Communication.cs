using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDrive.DAO.Helpers.Communication
{
    public class StatusCode<T>
    {
        public int Code { get; }
        public string Message { get; }
        public T Body { get; }

        public StatusCode(int code, string message, T body)
        {
            Code = code;
            Message = message;
            Body = body;
        }

        public StatusCode(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public StatusCode(int code, T body)
        {
            Code = code;
            Body = body;
        }

        public StatusCode(int code)
        {
            Code = code;
        }
    }

    public static class StatusMessages
    {
        public static string USER_NOT_EXISTS = "User not exists";
        public static string PARENT_DIRECTORY_NOT_FOUND = "Parent directory not found";
    }
}
