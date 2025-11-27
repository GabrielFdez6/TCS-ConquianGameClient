using System;
using System.Collections.Generic;
using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLogin;

namespace ConquiánCliente.Utilities.Messages
{
    public class ResourceMessageResolver : IMessageResolver
    {
        private readonly Dictionary<ServiceErrorType, Func<string>> messageMap;

        public ResourceMessageResolver()
        {
            this.messageMap = InitializeMessageMap();
        }

        public string GetMessage(ServiceErrorType errorType)
        {
            if (messageMap.TryGetValue(errorType, out Func<string> messageFunc))
            {
                return messageFunc.Invoke();
            }
            return Lang.ErrorConnectingToServer;
        }

        private Dictionary<ServiceErrorType, Func<string>> InitializeMessageMap()
        {
            return new Dictionary<ServiceErrorType, Func<string>>
            {
                { ServiceErrorType.SessionActive, () => Lang.ErrorSessionActive },
                { ServiceErrorType.UserNotFound, () => Lang.ErrorUserNotFound },
                { ServiceErrorType.InvalidPassword, () => Lang.ErrorInvalidCredentials },
                { ServiceErrorType.DatabaseError, () => Lang.GlobalSqlError },
                { ServiceErrorType.LobbyFull, () => Lang.ErrorLobbyFull },
                { ServiceErrorType.GuestInviteUsed, () => Lang.ErrorUsedInvitation },
                { ServiceErrorType.RegisteredUserAsGuest, () => Lang.ErrorRegisteredMail },
                { ServiceErrorType.DuplicateRecord, () => Lang.ErrorNicknameExists },
                { ServiceErrorType.CommunicationError, () => Lang.GlobalSmtpError },
            };
        }
    }
}
