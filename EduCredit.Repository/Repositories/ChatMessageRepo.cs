using EduCredit.Core.Chat;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Repositories
{
    public class ChatMessageRepo : IChatMessageRepo
    {
        private readonly EduCreditContext _dbcontext;

        public ChatMessageRepo(EduCreditContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesByCourseIdAsync(Guid courseId)
        {
            return await _dbcontext.ChatMessages
                .Where(msg => msg.CourseId == courseId)
                .OrderBy(msg => msg.SendAt)
                .ToListAsync();
        }

        public async Task AddAsync(ChatMessage message)
        {
            _dbcontext.ChatMessages.Add(message);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
