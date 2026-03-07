using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATMApp.Models;
using ATMApp.Data;

namespace ATMApp.Services
{
    public class LoggerService
    {
        private readonly LogRepository _repo = new LogRepository();

        // ლოგავს შეტყობინებას და მომხმარებლის პირად ნომერს
        public async Task LogAsync(string message, string userPersonalNumber)
        {
            await _repo.AddLogAsync(new LogEntry
            {
                Message = message,
                Time = DateTime.UtcNow,
                UserPersonalNumber = userPersonalNumber
            }).ConfigureAwait(false);
        }

        // ფილტრავს ლოგებს მოცემული პირობითი ფუნქციის მიხედვით
        public async IAsyncEnumerable<LogEntry> FilterLogsAsync(Func<LogEntry, bool> predicate)
        {
            await foreach (var log in _repo.LoadLogsAsync().ConfigureAwait(false))
            {
                if (predicate(log)) yield return log;
            }
        }

        
        public IAsyncEnumerable<LogEntry> GetAllLogsAsync() => _repo.LoadLogsAsync();
    }
}
