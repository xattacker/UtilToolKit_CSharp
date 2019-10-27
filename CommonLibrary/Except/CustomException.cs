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

        public ErrorId ErrorId { get; private set; }

        public Type ThrownType { get; private set; }

        public Exception OriginalException { get; private set; }

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
