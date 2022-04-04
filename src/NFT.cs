using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace NFTGen;

public class NFT
{
    private readonly string _input;
    private readonly string _output;
    private int _max;
    private Dictionary<string, string[]> _parts;
    public int Max => _max;

    public NFT(string input, string output)
    {
        _input = input;
        _output = output;
        _max = 1;
        
        List<string> folders = Directory.GetDirectories(_input).ToList();
        folders.Sort((x, y) => String.CompareOrdinal(x, y));
        
        _parts = folders.Select(part => new
        {
            Part = new FileInfo(part).Name,
            Files = Directory.GetFiles(part)
        }).ToDictionary(p => p.Part, p => p.Files);
        
        foreach (KeyValuePair<string,string[]> part in _parts)
            _max *= part.Value.Length;
    }

    public void GenerateRange(int qtd)
    {
        List<string> nfts = new();
        if (qtd > _max) throw new Exception("Quantity is greater than the maximum amount of possibilities. ");
        for(int i = 0; i < qtd; i++)
        {
            string nft;
            do
            {
                nft = string.Join(',', _parts.Select((part, index) =>
                {
                    List<int> range = Enumerable.Range(0, part.Value.Length).ToList();

                    return range[new Random().Next(range.Count)];
                }));
            } while (nfts.Exists(e => e == nft));
            nfts.Add(nft);
        }

        for(int i = 0; i < nfts.Count; i++)
        {
            List<int> nft = nfts[i].Split(',').Select(int.Parse).ToList();
            List<string> files = _parts.Select((part, index) => part.Value[nft[index]]).ToList();
            Generate(files, $"{i}.png");
        }
    }

    private void Generate(List<string> files, string name)
    {
        List<Image> images = files.Select(file => Image.Load(file)).ToList();

        GraphicsOptions options = new GraphicsOptions();

        Image? nft = images[0];
        for (int i = 1; i < images.Count; i++)
            nft = nft.Clone(clone => clone.DrawImage(images[i], options));
        
        nft.Save($"{_output}/{name}");
        nft.Dispose();

        foreach (Image image in images)
        {
            image.Dispose();
        }
    }
}