using System;

namespace Xattacker.Utility.Except
{
    public enum ErrorId : int
    { 
        SUCCEED = 0,
        UNKNOW = -9999,

        UNSUPPORTED = 1,
        UNCONNECTED,
        UNREGISTERED,
        UNDEFINED,
        UNUSED,
        UNSAFE,

        DATABASE_CONNECTION_FAILED = 1001,
        HTTP_CONNECTION_FAILED,
        WEB_SERVICE_CONNECTION_FAILED,
        SOCKET_CONNECTION_FAILED,
        DATAGRAM_CONNECTION_FAILED,
        MAIL_SMTP_CONNECTION_FAILED,
        MAIL_POP3_CONNECTION_FAILED,
        MAIL_IMAP_CONNECTION_FAILED,

        OUT_OF_INDEX = 2001,
        OUT_OF_SIZE,
        OUT_OF_MEMORY,
        OUT_OF_DATE,

        INVALID_CAST = 3001,
        INVALID_TYPE,
        INVALID_PARARMETER,
        INVALID_NAME,
        INVALID_KEY,
        INVALID_FORMAT,

        NULL_POINTER = 4001,
        DUPLICATED,
        CONFIGURE_ERROR,
        URI_FORMAT_ERROR,
        MAIL_ADDRESS_ERROR,
        XML_PARSE,
        SERIALIZATION,
        DESERIALIZATION,
        DATA_EMPTY,

        TIMEOUT = 5001,
        SHOTDOWN,
        OVERLOAD,
        EXPIRATION,
        LOCKED,

        SQL_SYTAX_ERROR = 6001,
        SQL_NULL_PK,
        SQL_NULL_COLUMN_VALUE,
        SQL_RECORD_NOT_FOUND,
        SQL_INSERT_INTERDICT,
        SQL_UPDATE_INTERDICT,
        SQL_DELETE_INTERDICT,

        AUTHORITY_FAILED = 7001,
        SECURITY_FAILED,
        EXECUTE_FAILED,
        ACCESS_FAILED,

        FILE_NOT_FOUND = 8001,
        PATH_NOT_FOUND,
        DATA_NOT_FOUND,

        FILE_READ_FAILED = 9001,
        FILE_WRITE_FAILED,
        STREAM_READ_FAILED,
        STREAM_WRITE_FAILED,
        ARRAY_READ_FAILED,
        ARRAY_WRITE_FAILED,
        BINARY_READ_FAILED,
        BINARY_WRITE_FAILED,
        OBJECT_READ_FAILED,
        OBJECT_WRITE_FAILED
    }


    public sealed class ErrorIdTable
    {
        /// <summary>
        /// to hide the constructor
        /// </summary>
        private ErrorIdTable()
        { 
        }

        public static string GetErrorDesc(ErrorId id)
        { 
            string desc = string.Empty;


            #region description list

            switch (id)
            {
                case ErrorId.SUCCEED:
                    desc = "succeed !!";
                    break;

                case ErrorId.UNSUPPORTED:
                    desc = "unsupported";
                    break;

                case ErrorId.UNCONNECTED:
                    desc = "unconnected";
                    break;

                case ErrorId.UNREGISTERED:
                    desc = "unregistered";
                    break;

                case ErrorId.UNDEFINED:
                    desc = "undefined";
                    break;

                case ErrorId.UNUSED:
                    desc = "unused";
                    break;

                case ErrorId.UNSAFE:
                    desc = "unsafe";
                    break;

                case ErrorId.DATABASE_CONNECTION_FAILED:
                    desc = "database connection failed";
                    break;

                case ErrorId.HTTP_CONNECTION_FAILED:
                    desc = "http connection failed";
                    break;

                case ErrorId.WEB_SERVICE_CONNECTION_FAILED:
                    desc = "web service connection failed";
                    break;

                case ErrorId.SOCKET_CONNECTION_FAILED:
                    desc = "socket connection failed";
                    break;

                case ErrorId.DATAGRAM_CONNECTION_FAILED:
                    desc = "datagram connection failed";
                    break;

                case ErrorId.MAIL_SMTP_CONNECTION_FAILED:
                    desc = "mail smtp connection failed";
                    break;

                case ErrorId.MAIL_POP3_CONNECTION_FAILED:
                    desc = "mail pop3 connection failed";
                    break;

                case ErrorId.MAIL_IMAP_CONNECTION_FAILED:
                    desc = "mail imap connection failed";
                    break;

                case ErrorId.FILE_NOT_FOUND:
                    desc = "file not found";
                    break;

                case ErrorId.PATH_NOT_FOUND:
                    desc = "path not found";
                    break;

                case ErrorId.DATA_NOT_FOUND:
                    desc = "data not found";
                    break;

                case ErrorId.OUT_OF_INDEX:
                    desc = "the index out of range";
                    break;

                case ErrorId.OUT_OF_SIZE:
                    desc = "the value out of size";
                    break;

                case ErrorId.OUT_OF_MEMORY:
                    desc = "out of memory";
                    break;

                case ErrorId.OUT_OF_DATE:
                    desc = "out of date";
                    break;

                case ErrorId.INVALID_CAST:
                    desc = "invalid type casting";
                    break;

                case ErrorId.INVALID_TYPE:
                    desc = "invalid type";
                    break;

                case ErrorId.INVALID_PARARMETER:
                    desc = "invalid parameter";
                    break;

                case ErrorId.INVALID_NAME:
                    desc = "invalid name";
                    break;

                case ErrorId.INVALID_KEY:
                    desc = "invalid key";
                    break;

                case ErrorId.INVALID_FORMAT:
                    desc = "invalid format";
                    break;

                case ErrorId.NULL_POINTER:
                    desc = "null pointer";
                    break;

                case ErrorId.DUPLICATED:
                    desc = "data duplicated";
                    break;

                case ErrorId.CONFIGURE_ERROR:
                    desc = "configure error";
                    break;

                case ErrorId.URI_FORMAT_ERROR:
                    desc = "uri format error";
                    break;

                case ErrorId.MAIL_ADDRESS_ERROR:
                    desc = "mail address format error";
                    break;

                case ErrorId.XML_PARSE:
                    desc = "xml parse failed";
                    break;

                case ErrorId.SERIALIZATION:
                    desc = "failed to serialize";
                    break;

                case ErrorId.DESERIALIZATION:
                    desc = "failed to deserialize";
                    break;

                case ErrorId.DATA_EMPTY:
                    desc = "data empty";
                    break;

                case ErrorId.SQL_SYTAX_ERROR:
                    desc = "sql script sytax error";
                    break;

                case ErrorId.SQL_NULL_PK:
                    desc = "sql primary key value is null";
                    break;

                case ErrorId.SQL_NULL_COLUMN_VALUE:
                    desc = "sql column value is null";
                    break;

                case ErrorId.SQL_RECORD_NOT_FOUND:
                    desc = "sql record not found";
                    break;

                case ErrorId.SQL_INSERT_INTERDICT:
                    desc = "sql insert interdict";
                    break;

                case ErrorId.SQL_UPDATE_INTERDICT:
                    desc = "sql update interdict";
                    break;

                case ErrorId.SQL_DELETE_INTERDICT:
                    desc = "sql delete interdict";
                    break;

                case ErrorId.TIMEOUT:
                    desc = "timeout";
                    break;

                case ErrorId.SHOTDOWN:
                    desc = "shotdonwn";
                    break;

                case ErrorId.OVERLOAD:
                    desc = "overload";
                    break;

                case ErrorId.EXPIRATION:
                    desc = "expiration";
                    break;

                case ErrorId.LOCKED:
                    desc = "locked";
                    break;

                case ErrorId.AUTHORITY_FAILED:
                    desc = "authority failed";
                    break;

                case ErrorId.SECURITY_FAILED:
                    desc = "security failed";
                    break;

                case ErrorId.EXECUTE_FAILED:
                    desc = "execute failed";
                    break;

                case ErrorId.ACCESS_FAILED:
                    desc = "access failed";
                    break;
                               
                case ErrorId.FILE_READ_FAILED:
                    desc = "file read failed";
                    break;

                case ErrorId.FILE_WRITE_FAILED:
                    desc = "file write failed";
                    break;

                case ErrorId.STREAM_READ_FAILED:
                    desc = "stream read failed";
                    break;

                case ErrorId.STREAM_WRITE_FAILED:
                    desc = "stream write failed";
                    break;

                case ErrorId.ARRAY_READ_FAILED:
                    desc = "array read failed";
                    break;

                case ErrorId.ARRAY_WRITE_FAILED:
                    desc = "array write failed";
                    break;

                case ErrorId.BINARY_READ_FAILED:
                    desc = "binary read failed";
                    break;

                case ErrorId.BINARY_WRITE_FAILED:
                    desc = "binary write failed";
                    break;

                case ErrorId.OBJECT_READ_FAILED:
                    desc = "object read failed";
                    break;

                case ErrorId.OBJECT_WRITE_FAILED:
                    desc = "object write failed";
                    break;

                case ErrorId.UNKNOW:
                    desc = "unknown error";
                    break;

                default:
                    desc = "unknown or undefined error id";
                    break;
            }

            #endregion 


            return desc;
        }
    }
}
