﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {

        //трябва да са едновременно pk and fk
        [ForeignKey(nameof(Song))]
        public int SongId  { get; set; }
        public virtual Song? Song { get; set; }

        [ForeignKey(nameof(Performer))]
        public int PerformerId { get; set; }
        public virtual Performer? Performer { get; set; }
    }
}
