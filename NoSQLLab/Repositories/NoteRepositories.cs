using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using NoSQLLab.Models;

namespace NoSQLLab.Repositories
{
    public class NoteRepository
    {
        private readonly IMongoCollection<Note> collection;

        public NoteRepository(IConfiguration configuration)
        {
            var connString =
                configuration.GetConnectionString("MongoDBConnection");
            collection = new MongoClient(connString)
                .GetDatabase("notes_db")
                .GetCollection<Note>("notes");
        }
        public IReadOnlyCollection<Note> GetAll()
        {
            return collection
                .Find(x => true)
                .ToList();
        }

        public Note Insert(Note note)
        {
            note.Id = Guid.NewGuid();
            collection.InsertOne(note);
            return note;
        }

        public Note GetById(Guid id)
        {
            return collection
                .Find(x => x.Id == id)
                .FirstOrDefault();
        }

        public IReadOnlyCollection<Note> GetByUserId(Guid
            userId)
        {
            return collection
                .Find(x => x.UserId == userId)
                .ToList();
        }

        public void Delete(Guid noteId)
        {
            collection.DeleteOne((x) => x.Id == noteId);
        }

        public IReadOnlyCollection<Note> Search(Guid userId,
            string searchQuery)
        {
            string lowerCaseQuery = searchQuery.ToLower();
            return collection.Find(x =>
                x.Text.ToLower().Contains(lowerCaseQuery) && x.UserId ==
                userId).ToList();
        }

        public Note Update(Note note)
        {
            var existingNote = GetById(note.Id);
            if (existingNote == null)
                throw new Exception("Note with this id doesn't exist");

            collection.ReplaceOne(n => n.Id == note.Id, note);
            return existingNote;
        }

        public async void CreateIndexes()
        {
            await collection.Indexes
                .CreateOneAsync(new
                    CreateIndexModel<Note>(Builders<Note>.IndexKeys.Ascending(_ =>
                        _.Id)))
                .ConfigureAwait(false);
            await collection.Indexes
                .CreateOneAsync(new
                    CreateIndexModel<Note>(Builders<Note>.IndexKeys.Ascending(_ =>
                        _.UserId)))
                .ConfigureAwait(false);
        }
    }
}