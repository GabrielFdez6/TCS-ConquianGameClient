using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConquiánCliente.ServiceLogin;
using PlayerLogin = ConquiánCliente.ServiceLogin.Player;

namespace ConquiánCliente.ViewModel
{
    public static class PlayerSession
    {
        public static PlayerLogin CurrentPlayer { get; private set; }

        public static bool IsLoggedIn => CurrentPlayer != null;

        public static void StartSession(Player player)
        {
            CurrentPlayer = player;
        }

        public static void UpdateSession(ServiceUserProfile.Player fullPlayerProfile)
        {
            if (IsLoggedIn && CurrentPlayer.nickname == fullPlayerProfile.nickname)
            {
                CurrentPlayer.name = fullPlayerProfile.name;
                CurrentPlayer.lastName = fullPlayerProfile.lastName;
                CurrentPlayer.email = fullPlayerProfile.email;
                CurrentPlayer.level = fullPlayerProfile.level;
                CurrentPlayer.pathPhoto = fullPlayerProfile.pathPhoto;
                CurrentPlayer.currentPoints = fullPlayerProfile.currentPoints;
            }
        }


        public static void EndSession()
        {
            CurrentPlayer = null;
        }
    }
}
