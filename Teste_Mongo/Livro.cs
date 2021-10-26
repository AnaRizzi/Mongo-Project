using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Teste_Mongo
{
    public class Livro
    {
        //no mongo o Id é desse tipo ObjectId, então precisa disso
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Ano { get; set; }
        public List<string> Tema { get; set; }
    }
}
