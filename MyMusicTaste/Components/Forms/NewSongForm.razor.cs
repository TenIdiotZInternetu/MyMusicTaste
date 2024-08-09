using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Forms;

public partial class NewSongForm : ComponentBase
{
    private class NewSongDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(128)]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Artist is required.")]
        [StringLength(64)]
        public string Author { get; set; }
    }
    
    [SupplyParameterFromForm]
    private NewSongDto _songDto { get; set; } = new();
    
    private bool _submitted { get; set; } = false;
    private bool _alreadyExists { get; set; } = false;

    private async Task SubmitAsync()
    {
        try
        {
            Song model = ToModel(_songDto);
            await SongSubmission.SubmitSongAsync(model);
            _submitted = true;
        }
        catch (EntryAlreadyExistsException e)
        {
            _alreadyExists = true;
        }
    }
    
    private Song ToModel(NewSongDto songDto)
    {
        return new Song
        {
            Title = songDto.Title,
            Author = songDto.Author
        };
    }
}