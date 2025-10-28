﻿using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstraction
{
    public interface IRoomtypeRepository
    {
        Task<RoomType?> GetByIdAsync(int id);
        Task<IEnumerable<RoomType>> GetAllAsync();
    }
}
