namespace WorkManagementSystem.Features.Setting.CreateSettingValue
{
    public class Request : SettingModel
    {
        public class Validator : Validator<Request>
        {
            public Validator()
            {

                RuleFor(x => x.Key)
                    .NotEmpty().WithMessage("Thông tin không được để trống")
                    .MaximumLength(100).WithMessage("Không được vượt quá 100 ký tự");

                RuleFor(x => x.Value)
                    .NotEmpty().WithMessage("Thông tin không được để trống!");

            }
        }
    }
}
