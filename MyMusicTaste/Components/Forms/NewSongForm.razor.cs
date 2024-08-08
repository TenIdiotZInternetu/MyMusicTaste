using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Database.Models;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Components.Forms;

public partial class NewSongForm : ComponentBase
{
    private class NewSongModel : ISongModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(128)]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Artist is required.")]
        [StringLength(64)]
        public string Artist { get; set; }

        public SongFullModel ToFullModel()
        {
            return new SongFullModel
            {
                Title = Title,
                Artist = Artist
            };
        }
    }
    
    [SupplyParameterFromForm]
    private NewSongModel _model { get; set; } = new();
    
    private bool _submitted { get; set; } = false;
    private bool _alreadyExists { get; set; } = false;

    private async Task SubmitAsync()
    {
        try
        {
            await SongSubmission.SubmitSongAsync(_model);
            _submitted = true;
        }
        catch (EntryAlreadyExistsException e)
        {
            _alreadyExists = true;
        }
    }
}