using NFTGen;

if (args.Length > 0)
{
    if (args.Contains("-h")) Console.WriteLine(Help());
    else
    {
        try
        {
            NFT nft = new(args[0], args[1]);
            nft.GenerateRange(int.Parse(args[2]));
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error has occurred:\n{e.Message}");
        }
    }
}
else Console.WriteLine(Help());

static string Help() => @"
NFTGen - Application for generate unique and random images
usage: NFTGen <images-input> <images-output> <qtd_gen>
<images-input> is a folder with subfolder where each subfolder is a part of the NFT to be made.
<images-output> is a folder where the NFT will be saved.
<qtd_gen> is the quantity of NFT to be generated.
Ex.:
  cats  
  ├───head  
  ├───paw  
  ├───tail  
commands:
  -h: show this  ";