using System;

namespace DynamicDriving.TripManagement.Application.IntegrationTests;

[AttributeUsage(AttributeTargets.Method)]
public class CustomDataSourceAttribute : AutoDataAttribute
{
    public CustomDataSourceAttribute() : base(() =>
        new Fixture().Customize(new AutoMoqCustomization()))
    {
    }

    protected CustomDataSourceAttribute(ICustomization customization) : base(() =>
        new Fixture().Customize(new CompositeCustomization(
            new AutoMoqCustomization(), customization)))
    {
    }
}
