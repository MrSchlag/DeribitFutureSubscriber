using System;

namespace DeribitFutureDatabaseLibrary
{
    public static class Connexion
    {
        private static string Localhost = "localhost";
        private static string ContainerLocalhost = "postgres"; //to use instead of Localhost when launch with docker-compose
        private static string UserId = "postgres";
        private static string Password = "mysecretpassword";

        public static string ConnexionString => $"User ID={UserId};Password={Password};Host={ContainerLocalhost};Port=5432;Database=MarketData2;Pooling=true;";
    }
}
