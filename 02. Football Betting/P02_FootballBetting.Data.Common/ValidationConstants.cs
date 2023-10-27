using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Common
{
    public class ValidationConstants
    {
        // Team
        public const int TeamNameMaxLength = 50;
        public const int TeamLogoUrlMaxLength = 2048;
        public const int TeamInitialsMaxLength = 4;

        // Color
        public const int ColorNameMaxLength = 10;

        // Town
        public const int TownNameMaxLength = 58;

        //Country

        public const int CountryNameMaxLength = 56;

        //player

        public const int PlayerNameMaxLength = 100;

        //position 

        public const int PositionNameMaxLength = 50;

        //Game

        public const int GameResultMaxLength = 7;

        //User

        public const int UserNameMaxLength = 36;
        public const int PasswordMaxLength = 255;
        public const int UserEmailMaxLength = 255;
        public const int NameMaxLength = 100;
    }
}
