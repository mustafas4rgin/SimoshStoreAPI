﻿using SimoshStore;

namespace SimoshStoreAPI;

public static class ServiceRegistrationProvider
{
    public static void RegisterServices(IServiceCollection services)
    {
        var servicesToRegister = new (Type Interface, Type Implementation)[]
        {
            (typeof(IProductService), typeof(ProductService)),
            (typeof(IDataRepository), typeof(DataRepository)),
            (typeof(IProductImageService), typeof(ProductImageService)),
            (typeof(IUserService), typeof(UserService)),
            (typeof(ICategoryService), typeof(CategoryService)),
            (typeof(ICommentService), typeof(CommentService)),
            (typeof(IRoleService), typeof(RoleService)),
            (typeof(IOrderService), typeof(OrderService)),
            (typeof(IContactFormService), typeof(ContactFormService)),
            (typeof(IBlogService), typeof(BlogService)),
            (typeof(IEmailService), typeof(SmtpEmailService)),
            (typeof(ISubscriptionService), typeof(SubscriptionService)),
            (typeof(IResultService), typeof(ResultService)),
            (typeof(IAuthService), typeof(AuthService)),
            (typeof(ITokenService), typeof(TokenService)),
            (typeof(IProfileService), typeof(ProfileService)),
            (typeof(ICartService), typeof(CartService)),
            (typeof(IAdminService), typeof(AdminService))
        };

        foreach (var service in servicesToRegister)
        {
            services.AddTransient(service.Interface, service.Implementation);
        }
    }
}

