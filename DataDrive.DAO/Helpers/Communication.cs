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
        public static readonly string USER_NOT_EXISTS = "User not exists";
        public static readonly string PARENT_DIRECTORY_NOT_FOUND = "Parent directory not found";
        public static readonly string FILE_NOT_FOUND = "File not found";
        public static readonly string CANNOT_DELETE_FILE = "File cannot be deleted";
        public static readonly string FILE_DELETED = "File deleted";
        public static readonly string DIRECTORY_NOT_FOUND = "Directory not found";
        public static readonly string FAILED_TO_SAVE_FILES = "Failed to save files";
        public static readonly string TOKEN_NOT_FOUND = "Token not found";
        public static readonly string PASSWORD_REQUIRED = "Password required";
        public static readonly string PASSWORD_IS_WRONG = "Password is wrong";
    }
}
