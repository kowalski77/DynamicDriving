using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            fixture.Customize<ControllerContext>(c =>
                c.OmitAutoProperties()
                .Do(h => h.HttpContext = httpContext));
        }
    }
}
