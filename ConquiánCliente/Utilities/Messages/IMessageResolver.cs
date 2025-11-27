using ConquiánCliente.ServiceLogin;

namespace ConquiánCliente.Utilities.Messages
{
    public interface IMessageResolver
    {
        string GetMessage(ServiceErrorType errorType);
    }
}
