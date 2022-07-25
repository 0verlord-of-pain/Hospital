using Microsoft.AspNetCore.Identity;
using Moq;

namespace Hospital.Tests
{
    public class MockUserManager
    {
        public static UserManager<TUser> UserManager<TUser>() where TUser : class
        {
            var mockStore = Mock.Of<IUserStore<TUser>>();
            var mockUserManager = new Mock<UserManager<TUser>>(mockStore, null, null, null, null, null, null, null, null);

            mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            return mockUserManager.Object;
        }
    }
}