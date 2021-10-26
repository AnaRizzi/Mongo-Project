using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste_Mongo
{
    public class MongoConnection
    {
        private readonly string _stringConnection;
        private readonly IMongoClient client;
        private readonly IMongoDatabase bancoDados;

        public MongoConnection()
        {
            _stringConnection = "mongodb://localhost:27017";
            client = new MongoClient(_stringConnection);

            //vai buscar o banco de dados, se não existir, ele cria
            bancoDados = client.GetDatabase("Biblioteca");
        }
        
        //insere um objeto do tipo BsonDocument:
       public async Task InserirDocManualNoBanco(BsonDocument document)
        {
            //acessar coleção, se não existir cria:
            IMongoCollection<BsonDocument> coleção = bancoDados.GetCollection<BsonDocument>("Livros");

            //incluir documento:
            await coleção.InsertOneAsync(document);

            Console.WriteLine("Documento incluído!");
        }

        //insere um objeto de uma classe, no caso a Livros:
        public async Task InserirLivroNoBanco(Livro document)
        {
            //acessar coleção, se não existir cria:
            IMongoCollection<Livro> coleção = bancoDados.GetCollection<Livro>("Livros");

            //incluir documento:
            await coleção.InsertOneAsync(document);

            Console.WriteLine("Documento incluído!");
        }
    }
}
