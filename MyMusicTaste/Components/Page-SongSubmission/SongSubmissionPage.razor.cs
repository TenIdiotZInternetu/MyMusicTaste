using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Page_Auth;
using MyMusicTaste.Components.Page_Song;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;
using MyMusicTaste.Utils;

namespace MyMusicTaste.Components.Page_SongSubmission;

public partial class SongSubmissionPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/submit-song";
    public static string GetRoute() => ROUTE_TEMPLATE;
    
    [Inject] private ISongSubmission _songSubmission { get; set; } = null!;
    [Inject] private NavigationManager _navigation { get; set; } = null!;
    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    
    private class _NewSongDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(128)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Artist is required.")]
        [StringLength(64)]
        public string Author { get; set; } = null!;
        
        [StringLength(128)]
        public string Album { get; set; } = null!;
        
        [StringLength(32)]
        public string? Genre { get; set; }
        
        public DateTime ReleaseDate { get; set; } = DateTime.Today;
        public string? SourceLink { get; set; }
        public string? CoverImageLink { get; set; }
    }
    
    [SupplyParameterFromForm]
    private _NewSongDto _songDto { get; set; } = new();

    private bool _alreadyExists;
    private bool _invalidCover;
    private bool _invalidSource;

    protected override void OnInitialized()
    {
        if (!_identity.IsAuthenticated())
        {
            _navigation.NavigateTo(LoginPage.GetRoute());
        }
    }
    
    private async Task SubmitAsync()
    {
        try
        {
            _alreadyExists = false;
            _invalidCover = !await LinkValidation.IsImageLinkValidAsync(_songDto.CoverImageLink, true);
            _invalidSource = !LinkValidation.IsSongLinkValid(_songDto.SourceLink, true);

            if (_invalidCover || _invalidSource) return;
            
            Song model = ToModel(_songDto);
            string newSongId = await _songSubmission.SubmitSongAsync(model);
            _navigation.NavigateTo(SongPage.GetRoute(newSongId));
        }
        catch (EntryAlreadyExistsException)
        {
            _alreadyExists = true;
        }
    }
    
    private Song ToModel(_NewSongDto songDto)
    {
        return new Song
        {
            Title = songDto.Title,
            Author = songDto.Author,
            Genre = songDto.Genre,
            Album = songDto.Album,
            ReleaseDate = DateOnly.FromDateTime(songDto.ReleaseDate),
            SourceLink = songDto.SourceLink,
            CoverImageLink = songDto.CoverImageLink
        };
    }
}