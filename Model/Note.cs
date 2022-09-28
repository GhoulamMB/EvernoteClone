using System;
#pragma warning disable CS8618
namespace EvernoteClone.Model
{
    public interface HasId
    {
        public string Id { get; set; }
    }
    public class Note : HasId
    {
        public string Id { get; set; }
        public string NoteBookId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string FileLocation { get; set; }
    }
}
