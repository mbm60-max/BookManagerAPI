using BManagerAPi.Entities;
using BManagerAPi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BManagerAPi.Controllers{
        [ApiController]
        [Route("notes")]
        public class NotesController : ControllerBase{
            private readonly NoteService _noteService;

            public NotesController(NoteService noteService)
            {
                _noteService = noteService;
            }
            [HttpGet("{id}")]
            public async Task<IActionResult> GetNote([FromRoute] Guid id)
            {
                try
                {
                    var note = await _noteService.GetNoteAsync(id);
                    return Ok(note);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid id, [FromBody]string newContent)
        {
            try
            {
                await _noteService.UpdateNoteAsync(id,newContent);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            try
            {
                await _noteService.DeleteNoteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        }
}