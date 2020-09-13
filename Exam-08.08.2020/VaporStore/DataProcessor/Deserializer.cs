namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		private const string ErrorMessage = "Invalid Data";

		private const string SuccessfullyImportedGame
			= "Added {0} ({1}) with {2} tags";

		private const string SuccessfullyImportedUser
			= "Imported {0} with {1} cards";

		private const string SuccessfullyImportedPurchasses
			= "Imported {0} for {1}";

		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();

			ImportGamesDto[] importGamesDtos = JsonConvert.DeserializeObject<ImportGamesDto[]>(jsonString);

			List<Game> games = new List<Game>();
			List<Tag> tags = new List<Tag>();
			List<Developer> developers = new List<Developer>();
			List<Genre> genres = new List<Genre>();

			foreach (var gameDto in importGamesDtos)
            {

				if (!IsValid(gameDto) || gameDto.Tags.Length == 0)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				Developer dev = developers.FirstOrDefault(d => d.Name == gameDto.Developer);
				Genre genre = genres.FirstOrDefault(g => g.Name == gameDto.Genre);

				DateTime date;
				var isDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

				var game = new Game
				{
					Name = gameDto.Name,
					Price = gameDto.Price,
					ReleaseDate = date,
					Developer = dev == null ? new Developer() { Name = gameDto.Developer } : dev,
					Genre = genre == null ? new Genre() { Name = gameDto.Genre } : genre,
				};

				int tagCounter = 0;
				foreach (var tag in gameDto.Tags)
				{
					GameTag searchTag;
					if (tags.FirstOrDefault(t => t.Name == tag) == null)
					{
						var tagtoAdd = new Tag() { Name = tag };
						game.GameTags.Add(new GameTag { Game = game, Tag = tagtoAdd });
						tags.Add(tagtoAdd);
					}
					else
					{
						var foundTag = tags.FirstOrDefault(t => t.Name == tag);
						game.GameTags.Add(new GameTag { Game = game, Tag = foundTag });
					}
					tagCounter++;
				}

				games.Add(game);

				if (dev == null)
				{
					developers.Add(game.Developer);
				}
				if (genre == null)
				{
					genres.Add(game.Genre);
				}

				sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, tagCounter));

			}
			context.Tags.AddRange(tags);
			context.Games.AddRange(games);
			context.SaveChanges();

			return sb.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();

			ImportUserDto[] importUserDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

			List<User> users = new List<User>();

            foreach (var userDto in importUserDtos)
            {

				if (!IsValid(userDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var user = new User
				{
					FullName = userDto.FullName,
					Username = userDto.Username,
					Email = userDto.Email,
					Age = userDto.Age,
				};

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
						sb.AppendLine(ErrorMessage);
						continue;
					}

					Card card = new Card()
					{
						Number = cardDto.Number,
						Cvc = cardDto.CVC,
						Type = (CardType)cardDto.Type
					};

					user.Cards.Add(card);

                }
				users.Add(user);
				sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));

			}
			context.Users.AddRange(users);
			context.SaveChanges();

			string result = sb.ToString().TrimEnd();
			return result;
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{

            var serializer = new XmlSerializer(typeof(ImportPurchaseDto[]), new XmlRootAttribute("Purchases"));

            StringBuilder sb = new StringBuilder();

            using (StringReader sr = new StringReader(xmlString))
            {
                var purchasesDtos = (ImportPurchaseDto[])serializer.Deserialize(sr);

                List<Purchase> lists = new List<Purchase>();
                List<Purchase> purchases = lists;

                foreach (var purchaseDto in purchasesDtos)
                {
                    if (!IsValid(purchaseDto))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    var card = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card);

                    var game = context.Games.FirstOrDefault(x => x.Name == purchaseDto.Title);

                    Purchase purchase = new Purchase
                    {
                        Type = Enum.Parse<PurchaseType>(purchaseDto.Type),
                        ProductKey = purchaseDto.ProductKey,
                        Card = card,
                        Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        Game = game
                    };

                    var username = context.Users.FirstOrDefault(x => x.Id == purchase.Card.UserId);

                    purchases.Add(purchase);

                    sb.AppendLine($"Imported {purchase.Game.Name} for {username.Username}");
                }

                context.Purchases.AddRange(purchases);
                context.SaveChanges();
            }

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