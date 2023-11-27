namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;

    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ImportDto;
    using System.Text;
    using System.Globalization;
    using VaporStore.Utilities;

    public static class Deserializer
    {

        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportGameDto[] gDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

            ICollection<Game> validGames = new HashSet<Game>();
            List<Developer> developers = new List<Developer>();
            List<Genre> genres = new List<Genre>();
            List<Tag> tags = new List<Tag>();
            List<Game> games = new List<Game>();

            foreach (ImportGameDto gDto in gDtos)
            {
                if (!IsValid(gDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime releaseDate;
                bool isReleaseDateValid = DateTime.TryParseExact(gDto.ReleaseDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                if (!isReleaseDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (gDto.Tags == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game g = new Game()
                {
                    Name = gDto.Name,
                    ReleaseDate = releaseDate,
                    Price = gDto.Price
                };

                Developer gameDeveloper = developers.FirstOrDefault
                    (d => d.Name == gDto.Name);

                if (gameDeveloper == null)
                {
                    Developer newdeveloper = new Developer()
                    {
                        Name = gDto.Developer
                    };
                    developers.Add(newdeveloper);
                    g.Developer = newdeveloper;
                }
                else
                {
                    g.Developer = gameDeveloper;
                }

                Genre genre = new Genre()
                {
                    Name = gDto.Name
                };

                Genre gameGenre = genres
                   .FirstOrDefault(g => g.Name == gDto.Name);

                if (gameGenre == null)
                {
                    Genre newGenre = new Genre()
                    {
                        Name = gDto.Name,
                    };

                    g.Genre = newGenre;
                    genres.Add(newGenre);
                }
                else
                {
                    g.Genre = gameGenre;
                }

                // zashoto e masiv 
                foreach (string tagName in gDto.Tags)
                {
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }

                    Tag gameTag = tags
                        .FirstOrDefault(t => t.Name == tagName);

                    if (gameTag == null)
                    {
                        Tag newGameTag = new Tag()
                        {
                            Name = tagName
                        };
                        tags.Add(newGameTag);
                        g.GameTags.Add(new GameTag()
                        {
                            Game = g,
                            Tag = newGameTag
                        });
                    }
                    else
                    {
                        g.GameTags.Add(new GameTag()
                        {
                            Game = g,
                            Tag = gameTag
                        });
                    }

                    if (g.GameTags.Count == 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    games.Add(g);
                    sb.AppendLine(String.Format(SuccessfullyImportedGame, g.Name, g.Genre.Name, g.GameTags.Count));
                }
            }

            context.Games.AddRange(games);
            context.SaveChanges();



            return sb.ToString().TrimEnd();





        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportUserDto[] uDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

            List<User> users = new List<User>();


            foreach (ImportUserDto uDto in uDtos)
            {
                if (!IsValid(uDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                List<Card> cards = new List<Card>();
                bool areAllCardsTrue = true;

                foreach (ImportCardDto cDto in uDto.Cards)
                {
                    if (!IsValid(cDto))
                    {
                        areAllCardsTrue = false;
                        break;

                    }

                    Object cardTypeRes;
                    bool isCardTypeValid = Enum.TryParse(typeof(CardType), cDto.Type, out cardTypeRes);

                    if (!isCardTypeValid)
                    {
                        areAllCardsTrue = false;
                        break;
                    }
                    CardType cardType = (CardType)cardTypeRes;

                    cards.Add(new Card()
                    {
                        Number = cDto.Number,
                        Cvc = cDto.Cvc,
                        Type = cardType
                    });

                }

                if (!areAllCardsTrue)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (cards.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User u = new User()
                {
                    Username = uDto.Username,
                    FullName = uDto.FullName,
                    Email = uDto.Email,
                    Age = uDto.Age,
                    Cards = cards
                };

                users.Add(u);
                sb.AppendLine(String.Format(SuccessfullyImportedUser, u.Username, u.Cards.Count));
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }



        static XmlHelper xmlHelper;

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            xmlHelper = new XmlHelper();

            ImportPurchaseDto[] pDtos = xmlHelper.Deserialize<ImportPurchaseDto[]>(xmlString, "Purchases");

            ICollection<Purchase> validPurchase = new HashSet<Purchase>();

            foreach (ImportPurchaseDto pDto in pDtos)
            {

                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                object purchaseTypeObj;
                bool isPurchaseTypeValid =
                    Enum.TryParse(typeof(PurchaseType), pDto.PurchaseType, out purchaseTypeObj);

                PurchaseType purchaseType = (PurchaseType)purchaseTypeObj;

                if (!isPurchaseTypeValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }



                DateTime date;
                bool isDateValid = DateTime.TryParseExact(pDto.Date, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }



                Card card = context
                  .Cards
                  .FirstOrDefault(c => c.Number == pDto.CardNumber);

                if (card == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game game = context
                    .Games
                    .FirstOrDefault(g => g.Name == pDto.Title);

                if (game == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Purchase p = new Purchase()
                {
                    Type = purchaseType,
                    Date = date,
                    ProductKey = pDto.ProductKey,
                    Game = game,
                    Card = card
                };

                validPurchase.Add(p);
                sb.AppendLine(String.Format(SuccessfullyImportedPurchase, p.Game.Name, p.Card.User.Username));
            }

            context.Purchases.AddRange(validPurchase);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
    

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}