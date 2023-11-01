using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusicHub.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {
            SongPerformers = new HashSet<SongPerformer>();
        }
        [Key]
        public int Id { get; set; }

        // In EF <= 3.1.x we use [Required] attribute
        // In Ef >= 6.x everything is required and we add "?" to be nullable
        [MaxLength(ValidationConstants.SongNameMaxLength)]
        public string Name { get; set; } = null!; //This is required!

        // TimeSpan datatype is required by default!
        // In the DB this will be stored as BIGINT <=> Ticks count
        public TimeSpan Duration { get; set; }

        public DateTime CreatedOn { get; set; }

        // Enumerations are stored in the DB as INT
        public Genre Genre { get; set; }

        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; } //към този форейн кий
        public virtual Album? Album { get; set; } // тук се пълнят данните по форейн кея 

        [ForeignKey(nameof(Song))]
        public int WriterId { get; set; }
        public virtual Writer Writer { get; set; } = null!; // навижационно пропърти

        public decimal Price { get; set; }


        public ICollection<SongPerformer> SongPerformers { get; set; }

    }
}
