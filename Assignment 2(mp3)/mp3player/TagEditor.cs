namespace mp3player
{
    public class TagEditor
    {
        public static void ReadTags(string filePath, out string title, out string artist, out string album, out uint year)
        {
            var file = TagLib.File.Create(filePath);
            title = file.Tag.Title;
            artist = string.Join(", ", file.Tag.Performers);
            album = file.Tag.Album;
            year = file.Tag.Year;
        }

        public static void WriteTags(string filePath, string title, string artist, string album, uint year)
        {
            var file = TagLib.File.Create(filePath);
            file.Tag.Title = title;
            file.Tag.Performers = new[] { artist };
            file.Tag.Album = album;
            file.Tag.Year = year;
            file.Save();
        }

    }
}
