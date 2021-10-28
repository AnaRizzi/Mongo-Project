using MongoDB.Bson;

namespace Teste_Mongo
{
    public static class ManualDocument
    {
        public static BsonDocument GetNewDocument()
        {
            var doc = new BsonDocument
            {
                {"Titulo", "Harry Potter 2" },
                {"Autor", "J K Rowling" }
            };
            doc.Add("Ano", "1998");

            var temaArray = new BsonArray();
            temaArray.Add("Fantasia");
            temaArray.Add("Bruxaria");

            doc.Add("Tema", temaArray);

            return doc;
        }
    }
}
