﻿using Budget_CoolBooks.Data;
using Budget_CoolBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget_CoolBooks.Services.Moderators
{
    public class ModeratorServices
    {
        private readonly ApplicationDbContext _context;

        public ModeratorServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Review>> GetFlaggedReviews()
        {
            return _context.Reviews
                    .Include(r => r.User)
                    .Where(r => r.Flag > 0 && !r.IsDeleted)
                    .OrderBy(r => r.Created)
                    .ToList();
        }
        public async Task<bool> UnflagReview(Review review)
        {
            _context.Reviews.Update(review);
            return Save();
        }
    

        public async Task<ICollection<Comment>> GetFlaggedComments()
        {
            return _context.Comments
                    .Include(c => c.Review)
                    .Include(c => c.User)
                    .Where(c => c.Flag > 0)
                    .OrderBy(c => c.Created)
                    .ToList();
        }

        public async Task<bool> UnflagComment(Comment comment)
        {
            _context.Comments.Update(comment);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}