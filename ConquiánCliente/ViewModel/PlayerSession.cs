using ConquiánCliente.ServiceLogin;
using PlayerLogin = ConquiánCliente.ServiceLogin.PlayerDto;

namespace ConquiánCliente.ViewModel
{
    public static class PlayerSession
    {
        public static PlayerLogin CurrentPlayer { get; private set; }

        public static bool IsGuest { get; private set; }

        public static bool IsNetworkDown { get; set; } = false;
        public static bool IsLoggedIn => CurrentPlayer != null;

        static PlayerSession()
        {
            IsGuest = false;
        }

        public static void StartSession(PlayerLogin player)
        {
            CurrentPlayer = player;
            IsGuest = false;
        }

        public static void StartGuestSession(ServiceLobby.PlayerDto guestPlayer)
        {
            if (guestPlayer == null) return;

            CurrentPlayer = new PlayerLogin
            {
                idPlayer = guestPlayer.idPlayer,
                nickname = guestPlayer.nickname,
                pathPhoto = guestPlayer.pathPhoto
            };

            IsGuest = true;
        }


        public static void UpdateSession(ServiceUserProfile.PlayerDto fullPlayerProfile)
        {
            if (IsLoggedIn && CurrentPlayer.nickname == fullPlayerProfile.nickname)
            {
                CurrentPlayer.name = fullPlayerProfile.name;
                CurrentPlayer.lastName = fullPlayerProfile.lastName;
                CurrentPlayer.email = fullPlayerProfile.email;
                CurrentPlayer.idLevel = fullPlayerProfile.idLevel;
                CurrentPlayer.pathPhoto = fullPlayerProfile.pathPhoto;
                CurrentPlayer.currentPoints = fullPlayerProfile.currentPoints;
                CurrentPlayer.nickname = fullPlayerProfile.nickname;
            }
        }
        public static void UpdateProfilePicture(string newPhotoPath)
        {
            if (IsLoggedIn)
            {
                CurrentPlayer.pathPhoto = newPhotoPath;
            }
        }

        public static void EndSession()
        {
            CurrentPlayer = null;
            IsGuest = false;
        }
    }
}