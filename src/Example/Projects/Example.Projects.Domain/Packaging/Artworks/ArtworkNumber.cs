using Modular.Framework.Domain.Patterns;

namespace Example.Projects.Domain.Packaging.Artworks;

public class ArtworkNumber : ValueObject
{
    public string Text { get; private set; }

    private ArtworkNumber()
    {
    }

    private ArtworkNumber(string artworkNumber)
    {
        Validate();

        Text = artworkNumber;
    }

    private void Validate()
    {
    }

    public static ArtworkNumber FromString(string artworkNumber)
    {
        return new ArtworkNumber(artworkNumber);
    }
}