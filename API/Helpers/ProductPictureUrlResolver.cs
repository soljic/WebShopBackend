using API.DTOs;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Hosting;

namespace API.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<CommandProductDto, Product, string>
    {
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;

        public ProductPictureUrlResolver(Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string Resolve(CommandProductDto source, Product destination, string destMember, ResolutionContext context)
        {
            if (source.Picture == null) return null;
            //Prvo se generira jedinstveno ime slike korištenjem Guid.NewGuid().ToString() i uzima se ekstenzija slike korištenjem Path.GetExtension(command.Picture.FileName).
            //Ovo se radi kako bi se spriječilo preklapanje slika koje se pohranjuju u aplikaciju i kako bi se osiguralo da ime slike bude jedinstveno.
            string pictureName = Guid.NewGuid().ToString() + Path.GetExtension(source.Picture.FileName);
            //Zatim se stvara putanja na kojoj će se slika spremiti pomoću Path.Combine(_environment.WebRootPath, "images/products", pictureName).
            // _environment.WebRootPath sadrži apsolutnu putanju do web root direktorija aplikacije, a "images/products" je relativna putanja do direktorija u kojem će se slike pohraniti.
            string picturePath = Path.Combine(_environment.WebRootPath, "images/products", pictureName);
            //Nakon što se generira putanja, otvara se novi FileStream u načinu rada FileMode.Create, što znači da će se novi file stvoriti, a ako postoji, bit će prebrisano. 
            //Zatim se poziva metoda CopyToAsync koja kopira sadržaj IFormFile objekta koji je poslan u HTTP POST zahtjevu u otvoreni stream koji predstavlja file. 
            //Nakon što se kopiranje dovrši, stream se automatski zatvara i slika je spremljena na disku. 
            //Slika se sprema na disk zato što se podrazumijeva da se slike pohranjuju na lokalnom disku, ali se mogu spremiti i na cloud servis za pohranu datoteka, poput Azure Storage.
            using (var stream = new FileStream(picturePath, FileMode.Create))
            {
                source.Picture.CopyTo(stream);
            }

            return "/images/products/" + pictureName;
        }
    }
}
