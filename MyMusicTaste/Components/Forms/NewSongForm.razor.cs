using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Database.Models;

namespace MyMusicTaste.Components.Forms;

public partial class NewSongForm : ComponentBase
{
    private class NewSongModel : ISongModel
    {
        [Required]
        [StringLength(256)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(128)]
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
    
    private NewSongModel _model = new();
    private bool _submitted = false;

    private void Submit()
    {
        _submitted = true;
    }
}