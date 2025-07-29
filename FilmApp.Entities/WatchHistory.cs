using System;
using System.Text.Json.Serialization;

namespace FilmApp.Entities
{
    public class WatchHistory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FilmId { get; set; }

        public DateTime WatchedDate { get; set; }

        // ✅ User döngüsel referansa sebep olabilir, bu yüzden gizliyoruz (kullanmıyorsan kalabilir)
        [JsonIgnore]
        public User? User{ get; set; }
        public Film? Film { get; set; }

    }
}