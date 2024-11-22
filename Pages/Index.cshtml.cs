using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ofentse.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string Message { get; set; }
        public string SpotifyPlaylistLink { get; set; } // Property for the playlist link

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Message = "Ofentse...My tiny nigthmare thank you for intrusting me with your heart..." +
                "Think of me when you listen to this...";
            SpotifyPlaylistLink = "https://open.spotify.com/playlist/4Vbuvm4hCiRBiuBOH66OVM?si=Z-zTn9YUSeKNJjauB_oRlA"; // Replace with your playlist link
        }
    }
}
