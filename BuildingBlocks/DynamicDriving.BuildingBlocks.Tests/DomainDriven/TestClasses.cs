using System;
using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.BuildingBlocks.Tests.DomainDriven;

public class OrderId : BaseId
{
    public OrderId(Guid value) : base(value)
    {
    }
}

public class ProductId : BaseId
{
    public ProductId(Guid value) : base(value)
    {
    }
}
