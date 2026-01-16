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

        private static Dictionary<ServiceErrorType, Func<string>> InitializeMessageMap()
        {
            return new Dictionary<ServiceErrorType, Func<string>>
            { 
                { ServiceErrorType.SessionActive, () => Lang.ErrorSessionActive },
                { ServiceErrorType.UserNotFound, () => Lang.ErrorUserNotFound },
                { ServiceErrorType.InvalidPassword, () => Lang.ErrorInvalidCredentials },
                { ServiceErrorType.InvalidEmailFormat, () => Lang.ErrorEmailInvalidFormat },
                { ServiceErrorType.InvalidPasswordFormat, () => Lang.ErrorInvalidPasswordFormat },
                { ServiceErrorType.InvalidNameFormat, () => Lang.ErrorInvalidNameFormat },
                { ServiceErrorType.InvalidVerificationCode, () => Lang.ErrorInvalidVerificationCode },
                { ServiceErrorType.VerificationCodeExpired, () => Lang.ErrorVerificationCodeExpired },
                { ServiceErrorType.RegisteredMail, () => Lang.ErrorRegisteredMail },

                { ServiceErrorType.DatabaseError, () => Lang.GlobalSqlError },
                { ServiceErrorType.DuplicateRecord, () => Lang.ErrorNicknameExists },
                
                { ServiceErrorType.LobbyFull, () => Lang.ErrorLobbyFull },
                { ServiceErrorType.LobbyNotFound, () => Lang.ErrorLobbyNotFound },
                { ServiceErrorType.NotEnoughPlayers, () => Lang.ErrorNotEnoughPlayers },
                { ServiceErrorType.NotLobbyHost, () => Lang.ErrorNotLobbyHost },
                { ServiceErrorType.NotKickYourSelf, () => Lang.ErrorNotKickYourSelf },
                { ServiceErrorType.RoomNotFound, () => Lang.ErrorLobbyNotFound },
                
                { ServiceErrorType.GuestInviteUsed, () => Lang.ErrorUsedInvitation },
                { ServiceErrorType.RegisteredUserAsGuest, () => Lang.ErrorRegisteredMail },
                
                { ServiceErrorType.GameNotFound, () => Lang.ErrorGameNotFound },
                { ServiceErrorType.GameInProgress, () => Lang.ErrorGameInProgress },
                { ServiceErrorType.NotYourTurn, () => Lang.ErrorNotYourTurn },
                { ServiceErrorType.MustDiscardToFinish, () => Lang.ErrorMustDiscardToFinish },
                { ServiceErrorType.AlreadyDrawn, () => Lang.ErrorAlreadyDrawn },
                { ServiceErrorType.PendingDiscardAction, () => Lang.ErrorPendingDiscardAction },
                { ServiceErrorType.DeckEmpty, () => Lang.ErrorDeckEmpty },
                { ServiceErrorType.InvalidMeld, () => Lang.ErrorInvalidMeld },
                { ServiceErrorType.CardNotFound, () => Lang.ErrorCardNotFound },
                { ServiceErrorType.InvalidCardAction, () => Lang.ErrorInvalidCardAction },
                { ServiceErrorType.GameRuleViolation, () => Lang.ErrorGameRuleViolation },
                { ServiceErrorType.EmptyDiscaard, () => Lang.ErrorEmptyDiscaard },
                
                { ServiceErrorType.UserOffline, () => Lang.ErrorUserOffline },
                { ServiceErrorType.UserInGame, () => Lang.ErrorPlayerInGame },
                { ServiceErrorType.UserInLobby, () => Lang.ErrorPlayerInLobby },
                { ServiceErrorType.PlayerBanned, () => Lang.ErrorPlayerBanned },
                
                { ServiceErrorType.CommunicationError, () => Lang.ErrorCommunicationError },
                { ServiceErrorType.OpponentConnectionLost, () => Lang.OpponentConnectionLost },
                { ServiceErrorType.InvitationFailed, () => Lang.ErrorInvitationFailed },
                
                { ServiceErrorType.OperationFailed, () => Lang.ErrorOperationFailed },
                { ServiceErrorType.ServerInternalError, () => Lang.ErrorServerInternalError },
                { ServiceErrorType.ValidationFailed, () => Lang.ErrorValidationFailed },
                { ServiceErrorType.NotFound, () => Lang.ErrorNotFound },
                { ServiceErrorType.ExistingRequest, () => Lang.ErrorExistingRequest },
                { ServiceErrorType.HostUserNotFound, () => Lang.ErrorHostUserNotFound },
                { ServiceErrorType.Unknown, () => Lang.ErrorUnknown }
            };
        }
    }
}
