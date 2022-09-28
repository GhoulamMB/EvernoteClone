#pragma warning disable CS8618

namespace EvernoteClone.Model
{
    public class NoteBook : HasId
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}
