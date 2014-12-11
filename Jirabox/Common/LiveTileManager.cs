using Microsoft.Phone.Shell;
using System.Linq;

namespace Jirabox.Common
{
    public class LiveTileManager
    {
        public static void DeleteAllSecondaryTiles()
        {
            var tiles = ShellTile.ActiveTiles.Where(tile => tile.NavigationUri.ToString().Contains("/View/ProjectDetailView.xaml?Key="));
            tiles.ToList().ForEach(tile => tile.Delete());
        }      
    }
}
