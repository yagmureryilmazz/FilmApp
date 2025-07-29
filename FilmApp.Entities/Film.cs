using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FilmApp.Entities.Enums;

namespace FilmApp.Entities
{
    public class Film
    {
        public Film(string title, string director, int releaseYear, int duration, Genre genre)
        {
            Title = title;
            Director = director;
            ReleaseYear = releaseYear;
            Duration = duration;
            Genre = genre;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public int ReleaseYear { get; set; }

        public int Duration { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(24)")]
        [JsonConverter(typeof(JsonStringEnumConverter))] // Swagger’da enumları string gösterir
        public Genre Genre { get; set; }

        public ICollection<WatchHistory> WatchHistories { get; set; } = new List<WatchHistory>();
    }
}



    