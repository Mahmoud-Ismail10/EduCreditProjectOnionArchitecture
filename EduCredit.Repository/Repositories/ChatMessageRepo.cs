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

        public async Task<IReadOnlyList<ChatMessage>> GetMessagesByCourseIdAsync(Guid courseId)
        {
            return await _dbcontext.ChatMessages
                .Include(msg => msg.Sender) // Include the sender details
                .Where(msg => msg.CourseId == courseId)
                .OrderByDescending(msg => msg.SendAt)
                .ToListAsync();
        }
    }
}
