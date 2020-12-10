using MongoDB.Driver;
using Nyous.API.NoSQL.Contexts;
using Nyous.API.NoSQL.Domains;
using Nyous.API.NoSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nyous.API.NoSQL.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly IMongoCollection<EventoDomain> _eventos;

        public EventoRepository(INyousDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _eventos = database.GetCollection<EventoDomain>(settings.EventosCollectionName);
        }

        public void Adicionar(EventoDomain evento)
        {
            try
            {
                _eventos.InsertOne(evento);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Alterar(string id, EventoDomain evento)
        {
            try
            {
                _eventos.ReplaceOne(c => c.Id == id, evento);
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EventoDomain BuscarPorId(string id)
        {
            try
            {
                return _eventos.Find<EventoDomain>(e => e.Id == id).First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EventoDomain> Listar()
        {
            try
            {
                return _eventos.AsQueryable<EventoDomain>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remover(string id)
        {
            try
            {
                var evento = _eventos.Find<EventoDomain>(e => e.Id == id);

                if (evento == null)
                    throw new Exception("Evento não encontrado!");

                _eventos.DeleteOne(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
