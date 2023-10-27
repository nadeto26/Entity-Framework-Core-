using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models.Enums
{
    //Enumerations are not entities in the DB 
    //There are string representation of int values
    //In the DB -> int 
    public enum Prediction
    {
        Win =1,
        Lose = 2,
        Draw = 0 //равенство 
    }
}
