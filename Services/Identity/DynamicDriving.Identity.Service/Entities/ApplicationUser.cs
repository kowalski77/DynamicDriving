﻿using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace DynamicDriving.Identity.Service.Entities;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
    public int Credits { get; set; }

    public HashSet<Guid> MessageIds { get; set; } = new(); // NOTE: Idempotency in consumers
}
