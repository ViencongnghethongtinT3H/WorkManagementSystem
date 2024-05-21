﻿namespace WorkManagementSystem.Features.User.GetListUser
{
    public class Endpoint : EndpointWithoutRequest<List<UserModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            AllowAnonymous();
            Post("/user/get-users");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            Response = await data.GetUserInfo();
            await SendAsync(Response);

        }
    }
}
