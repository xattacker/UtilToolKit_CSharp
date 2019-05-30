using System;
using System.Reflection;

namespace Xattacker.Utility.Except
{
    public class CustomException : Exception
    {
        #region constructor

        public CustomException(ErrorId id, Type type)
        {
            this.ErrorId = id;
            this.ThrownType = type;
        }

        public CustomException(ErrorId id, Type type, Exception ex)
            : this(id, type)
        {
            this.OriginalException = ex;
        }

        #endregion


        #region data member related function

        public ErrorId ErrorId { private set; get; }

        public Type ThrownType { private set; get; }

        public Exception OriginalException { private set; get; }

        public string ErrorDescription
        {
            get
            {
                return ErrorIdTable.GetErrorDesc(this.ErrorId);
            }
        }

        public MethodBase ThrownMedthod
        {
            get
            {
                return this.TargetSite;
            }
        }

        #endregion


        #region override from Exception

        public override string Message
        {
            get
            {
                return this.ErrorId.ToString() + ": " + this.ErrorDescription;
            }
        }

        #endregion
    }
}
