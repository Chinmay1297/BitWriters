﻿using BitWriters.API.Models.Domain;

namespace BitWriters.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
    }
}