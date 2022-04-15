using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DynamicDriving.TripManagement.API.Tests;

[AttributeUsage(AttributeTargets.Method)]
public class CustomDataSourceAttribute : AutoDataAttribute
{
    public CustomDataSourceAttribute() : base(() =>
        new Fixture().Customize(new CompositeCustomization(
            new AutoMoqCustomization(),
            new ControllersCustomization())))
    {
    }

    protected CustomDataSourceAttribute(ICustomization customization) : base(() =>
        new Fixture().Customize(new CompositeCustomization(
            new AutoMoqCustomization(),
            new ControllersCustomization(),
            customization)))
    {
    }

    private class ControllersCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
        }
    }
}
