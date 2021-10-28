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
        private readonly IMongoCollection<Livro> colecao;

        public MongoConnection()
        {
            _stringConnection = "mongodb://localhost:27017";
            client = new MongoClient(_stringConnection);

            //vai buscar o banco de dados, se não existir, ele cria
            bancoDados = client.GetDatabase("Biblioteca");

            //acessar coleção, se não existir cria:
            colecao = bancoDados.GetCollection<Livro>("Livros");
        }
        
       //insere um objeto do tipo BsonDocument:
       public async Task InserirDocManualNoBanco(BsonDocument document)
        {
            //acessar coleção, se não existir cria:
            IMongoCollection<BsonDocument> coleçãoManual = bancoDados.GetCollection<BsonDocument>("Livros");

            //incluir documento:
            await coleçãoManual.InsertOneAsync(document);

            Console.WriteLine("Documento incluído!");
        }

        //insere um objeto de uma classe, no caso a Livros:
        public async Task InserirLivroNoBanco(Livro document)
        {
            //incluir documento:
            await colecao.InsertOneAsync(document);

            Console.WriteLine("Documento incluído!");
        }

        //insere uma lista de objetos de uma classe, insere vários de uma vez só:
        public async Task InserirVariosLivrosNoBanco(Livro document)
        {
            var lista = new List<Livro>();
            lista.Add(document);

            //incluir documento:
            await colecao.InsertManyAsync(lista);

            Console.WriteLine("Documento incluído!");
        }

        //listar documentos de uma coleção:
        public async Task<List<Livro>> BuscarLivrosNoBanco()
        {
            //buscar documentos, sem nenhum critério de busca:
            var listaLivros = await colecao.Find(new BsonDocument()).ToListAsync();

            foreach(var doc in listaLivros)
            {
                //transformar em Json para poder ser exibido no console:
                Console.WriteLine(doc.ToJson<Livro>());
            }
            Console.WriteLine("Fim da lista de livros");

            return listaLivros;
        }

        //buscar documentos com filtros de uma coleção:
        public async Task<List<Livro>> BuscarLivrosComFiltroBsonNoBanco()
        {
            //cria o filtro que você precisa
            var filtro = new BsonDocument()
            {
                {"Autor", "J K Rowling" }
            };

            //buscar documentos, com critério de busca:
            var listaLivros = await colecao.Find(filtro).ToListAsync();

            foreach (var doc in listaLivros)
            {
                //transformar em Json para poder ser exibido no console:
                Console.WriteLine(doc.ToJson<Livro>());
            }
            Console.WriteLine("Fim da lista de livros");

            return listaLivros;
        }

        //buscar documentos usando uma classe como filtro de uma coleção:
        public async Task<List<Livro>> BuscarLivrosComFiltroDeClasseNoBanco()
        {
            //cria o filtro que você precisa
            var filtro = Builders<Livro>.Filter;
            //cria a condição (igual (eq), maior (gte = >=), etc....)
            var condicao = filtro.Eq(x => x.Autor, "J K Rowling") & filtro.Gte(x => x.Ano, "1998");
            //buscar dentro do array tema se existe algum item igual ao procurado
            var condicao2 = filtro.AnyEq(x => x.Tema, "terror");

            //buscar documentos, com critério de busca:
            var listaLivros = await colecao.Find(condicao2).ToListAsync();

            foreach (var doc in listaLivros)
            {
                //transformar em Json para poder ser exibido no console:
                Console.WriteLine(doc.ToJson<Livro>());
            }
            Console.WriteLine("Fim da lista de livros");

            return listaLivros;
        }


        //buscar documentos usando uma classe como filtro de uma coleção e ordenar a busca:
        public async Task<List<Livro>> BuscarLivrosComFiltroOrdenadoNoBanco()
        {
            //cria o filtro que você precisa
            var filtro = Builders<Livro>.Filter;
            //cria a condição (igual (eq), maior (gte = >=), etc....)
            var condicao = filtro.Gt(x => x.Ano, "1997");

            //buscar documentos, com ordenação e limitando a quantidade de resultados:
            var listaLivros = await colecao.Find(condicao).SortBy(x => x.Titulo).Limit(2).ToListAsync();

            foreach (var doc in listaLivros)
            {
                //transformar em Json para poder ser exibido no console:
                Console.WriteLine(doc.ToJson<Livro>());
            }
            Console.WriteLine("Fim da lista de livros");

            return listaLivros;
        }

        //buscar e alterar um documento específico:
        public async Task<Livro> BuscarEAlterarUmLivroNoBanco()
        {
            //cria o filtro que você precisa
            var filtro = Builders<Livro>.Filter;
            //cria a condição (igual (eq), maior (gte = >=), etc....)
            var condicao = filtro.Eq(x => x.Titulo, "Cujo");

            //buscar o documento:
            var livro = await colecao.Find(condicao).FirstOrDefaultAsync();

            Console.WriteLine(livro.ToJson<Livro>());

            livro.Ano = "1990";

            //atualizar no banco, passando a condição de busca do que sera alterado e a informação nova
            await colecao.ReplaceOneAsync(condicao, livro);

            Console.WriteLine(livro.ToJson<Livro>());

            return livro;
        }

        //alterar um documento específico:
        public async Task AlterarUmLivroNoBanco()
        {
            //cria o filtro que você precisa
            var filtro = Builders<Livro>.Filter;
            //cria a condição pra encontrar o livro
            var condicao = filtro.Eq(x => x.Titulo, "Cujo");

            //cria o filtro do tipo Update
            var filtroDeAlteracao = Builders<Livro>.Update;
            //passa a alteração a ser feita
            var alteracao = filtroDeAlteracao.Set(x => x.Ano, "1995");

            //atualizar o documento, passa a condição de busca e depois a alteração a ser feita:
            var livro = await colecao.UpdateOneAsync(condicao, alteracao);
        }

        //alterar vários documentos ao mesmo tempo:
        public async Task AlterarVariosLivrosNoBanco()
        {
            //cria o filtro que você precisa
            var filtro = Builders<Livro>.Filter;
            //cria a condição pra encontrar o livro
            var condicao = filtro.Eq(x => x.Autor, "J K Rowling");

            //cria o filtro do tipo Update
            var filtroDeAlteracao = Builders<Livro>.Update;
            //passa a alteração a ser feita
            var alteracao = filtroDeAlteracao.Set(x => x.Autor, "J. K. Rowling");

            //atualizar o documento, passa a condição de busca e depois a alteração a ser feita:
            var livro = await colecao.UpdateManyAsync(condicao, alteracao);
        }

        //excluir um documento específico:
        public async Task ExcluirUmLivroNoBanco()
        {
            //cria o filtro que você precisa
            var filtro = Builders<Livro>.Filter;
            //cria a condição pra encontrar o livro
            var condicao = filtro.Eq(x => x.Titulo, "Cujo");

            //excluir o documento, passa a condição de busca:
            var livro = await colecao.DeleteOneAsync(condicao);
        }
    }
}
