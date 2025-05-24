using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessErrorType ErrorType { get; }

        public BusinessException(BusinessErrorType errorType, string message = null)
            : base(message ?? errorType.ToString())
        {
            ErrorType = errorType;
        }
    }
}
