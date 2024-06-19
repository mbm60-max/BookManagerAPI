using BManagerAPi.Entities;

namespace BManagerAPi.Repositories{
    public class NoteService{
        private readonly Supabase.Client _supabase;

        public NoteService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }
        public async Task<Note> GetNoteAsync(Guid id){
          var response = await _supabase
                .From<Note>()
                .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id.ToString())
                .Single();

            if (response == null)
            {
                throw new Exception("Note not found.");
            }
            return response;  
        }
       public async Task UpdateNoteAsync(Guid id, string newContent)
{
    var update = await _supabase
        .From<Note>()
        .Where(x => x.Id == id)
        .Set(x => x.Content, newContent)
        .Update();
    if (update == null)
    {
        throw new Exception("Failed to update note.");
    }
}

        public async Task DeleteNoteAsync(Guid id){
            try{
                Console.WriteLine("Deleting");
                Console.WriteLine(id);
                await _supabase.From<Note>().Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id.ToString()).Delete();
            }catch{
                throw new Exception("Failed to delete note");
            }
        }
    }
}