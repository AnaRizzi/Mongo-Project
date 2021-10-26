using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Teste_Mongo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await MainASync(args);
            Console.WriteLine("pressione enter");
            Console.ReadLine();
        }

        static async Task MainASync(string[] args)
        {
            var banco = new MongoConnection();

            //criar um Bson manualmente
            var doc = ManualDocument.GetNewDocument();
            //await banco.InserirDocManualNoBanco(doc);

            //criar um objeto a partir de uma classe
            var livro = new Livro()
            {
                Titulo = "It",
                Autor = "Stephen King",
                Ano = "2000",
                Tema = new List<string>() { "terror", "animais" }
            };

            await banco.InserirLivroNoBanco(livro);
        }
    }
}
