namespace TiendaServicios.Api.Autor.Helpers
{
    public static class Global
    {
        public static Messages GetLanguage()
        {
            var redis = new
            {
                Language = "EN",
                Usuario = "Pepe"
            };

            switch (redis.Language)
            {
                case "EN":
                    return new EnglishMessages();

                case "SP":
                    return new SpanishMessages();

                default:
                    return new EnglishMessages();

            }
        }

    }
}
