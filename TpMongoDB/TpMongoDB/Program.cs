using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TpMongoDB.Interfaces;
using TpMongoDB.Repositories;
using TpMongoDB.Services;

class Program
{
    static void Main(string[] args)
    {
        // Initialise la connexion à MongoDB
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("euro");

        // Configuration des services
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(_ => database)
            .AddTransient<IEquipeRepository, EquipeRepository>()
            .AddTransient<IGroupeRepository, GroupeRepository>()
            .AddTransient<TirageAuSortPouleService>()
            .AddTransient<SimuilerCalendrierService>()
            .BuildServiceProvider();

        // Utilisation d'un scope pour accéder aux services configurés
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                // Récupère le service de tirage au sort
                var tirageAuSortService = services.GetRequiredService<TirageAuSortPouleService>();

                // Appelle la méthode d'effectuer le tirage au sort
                tirageAuSortService.EffectuerTirageAuSort();
                Console.WriteLine("Le tirage au sort a été effectué avec succès.");

                // Récupère le service de calendrier
                var tsimulationCalendrier = services.GetRequiredService<SimuilerCalendrierService>();

                // Appelle la méthode d'effectuer le calendrier des matchs
                tsimulationCalendrier.GenererCalendrierMatches();
                Console.WriteLine("La génération de calendrier a été effectuée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }
        }
    }
}