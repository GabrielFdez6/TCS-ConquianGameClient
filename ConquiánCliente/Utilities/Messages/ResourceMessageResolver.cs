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
                { ServiceErrorType.ExistingRequest, () => Lang.ErrorExistingRequest },
                { ServiceErrorType.NotFound, () => Lang.ErrorNotFound },
                { ServiceErrorType.NotYourTurn, () => Lang.ErrorNotYourTurn },
                { ServiceErrorType.MustDiscardToFinish, () => Lang.ErrorMustDiscardToFinish },
                { ServiceErrorType.GameRuleViolation, () => Lang.ErrorGameRuleViolation },
                { ServiceErrorType.InvalidCardAction, () => Lang.ErrorInvalidCardAction },
                { ServiceErrorType.InvalidMeld, () => Lang.ErrorInvalidMeld },
                { ServiceErrorType.OperationFailed, () => Lang.ErrorOperationFailed },
                { ServiceErrorType.AlreadyDrawn, () => Lang.ErrorAlreadyDrawn },
                { ServiceErrorType.PendingDiscardAction, () => Lang.ErrorPendingDiscardAction },
                { ServiceErrorType.DeckEmpty, () => Lang.ErrorDeckEmpty },
                { ServiceErrorType.EmptyDiscaard, () => Lang.ErrorEmptyDiscaard },
                { ServiceErrorType.CommunicationError, () => Lang.ErrorCommunicationError },
                { ServiceErrorType.ServerInternalError, () => Lang.ErrorServerInternalError },
                { ServiceErrorType.LobbyNotFound, () => Lang.ErrorLobbyNotFound },
                { ServiceErrorType.HostUserNotFound, () => Lang.ErrorHostUserNotFound },
                { ServiceErrorType.ValidationFailed, () => Lang.ErrorValidationFailed },
                { ServiceErrorType.NotEnoughPlayers, () => Lang.ErrorNotEnoughPlayers },
                { ServiceErrorType.NotLobbyHost, () => Lang.ErrorNotLobbyHost },
                { ServiceErrorType.NotKickYourSelf, () => Lang.ErrorNotKickYourSelf },
                { ServiceErrorType.InvalidNameFormat, () => Lang.ErrorInvalidNameFormat },
                { ServiceErrorType.InvalidPasswordFormat, () => Lang.ErrorInvalidPasswordFormat },
                { ServiceErrorType.InvalidEmailFormat, () => Lang.ErrorEmailInvalidFormat },
                { ServiceErrorType.RegisteredMail, () => Lang.ErrorRegisteredMail },
                { ServiceErrorType.VerificationCodeExpired, () => Lang.ErrorVerificationCodeExpired },
                { ServiceErrorType.InvalidVerificationCode, () => Lang.ErrorInvalidVerificationCode },
            };
        }
    }
}
