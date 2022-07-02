using Microsoft.EntityFrameworkCore;
using MyMinimalAPI.Data;
using MyMinimalAPI.Models;

namespace MyMinimalAPI.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _db;
        public CommandRepo(AppDbContext db)
        {
            _db = db;
        } 
        public async Task CreateCommandAsync(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            await _db.Commands.AddAsync(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _db.Commands.Remove(cmd);
        }

        public async Task<IEnumerable<Command>> GetAllCommandsAsync()
        {
            return await _db.Commands.ToListAsync();
        }

        public async Task<Command?> GetCommandByIdAsync(int id)
        {
            return await _db.Commands.Where(p => p.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }

}