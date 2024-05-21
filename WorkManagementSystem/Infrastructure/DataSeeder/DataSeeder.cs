namespace WorkManagementSystem.Infrastructure.DataSeeder;

public class DataSeeder
{
    private readonly MainDbContext dbContext;
    public DataSeeder(MainDbContext _dbContext)
    {
        dbContext = _dbContext;
    }
    public async Task SeedAllAsync()
    {
        try
        {
            await SeedDataSetting();
            await SeedDataDepartment();
            await SeedDataPosition();
            await SeedDataUser();
        }
        catch (Exception ex )
        {

            throw ex;
        }
       
    }

    private async Task SeedDataSetting()
    {
        if (await dbContext.Settings.AnyAsync())
        {
            return;
        }
        var settings = new List<Setting>
        {
            new Setting
            {
                Type = Entities.Enums.SettingEnum.Industry,
                Key = "Industry_htqt",
                Value = "Hợp tác quốc tế"
            },
            new Setting
            {
                Type = Entities.Enums.SettingEnum.Industry,
                Key = "Industry_nckh",
                Value = "Nghiên cứu khoá học"
            },
            new Setting
            {
                Type = Entities.Enums.SettingEnum.Industry,
                Key = "Industry_kt",
                Value = "Khen thưởng"
            },
            new Setting
            {
                Type = Entities.Enums.SettingEnum.Industry,
                Key = "Industry_khct",
                Value = "Kế hoạch công tác"
            },
          /*----------------------------------------------------------------------------*/
            new Setting
            {
                Type = Entities.Enums.SettingEnum.DocumentType,
                Key = "DocumentType_bb",
                Value = "Báo cáo"
            },
            new Setting
            {
                Type = Entities.Enums.SettingEnum.DocumentType,
                Key = "DocumentType_cv",
                Value = "Công Văn"
            },
            new Setting
            {
                Type = Entities.Enums.SettingEnum.DocumentType,
                Key = "DocumentType_ct",
                Value = "Chỉ thị"
            },
            new Setting
            {
                Type = Entities.Enums.SettingEnum.DocumentType,
                Key = "DocumentType_cc",
                Value = "Chương trình"
            },
            new Setting
            {
                Type = Entities.Enums.SettingEnum.DocumentType,
                Key = "DocumentType_cd",
                Value = "Công điện"
            }, 
       /*----------------------------------------------------------------------------*/
       new Setting
            {
                Type = Entities.Enums.SettingEnum.Symbol,
                Key = "Symbol_DHL",
                Value = "QĐ_DHL"
            },
            new Setting
            {
                Type = Entities.Enums.SettingEnum.Symbol,
                Key = "Symbol_ct",
                Value = "CT_BBL"
            }

        };
        await dbContext.AddRangeAsync(settings);
        await dbContext.SaveChangesAsync();
    }
    private async Task SeedDataDepartment()
    {

        if (await dbContext.Departments.AnyAsync())
        {
            return;
        }
        var parent = new List<Department>
        {
            new Department
            {
                ParentId = null,
                Name = "Khối cơ quan cấp bộ",
                OrganizationLevel = Entities.Enums.OrganizationLevelEnum.CAP_BO
            },
            new Department
            {
                ParentId = null,
                Name = "Đảng uỷ cơ quan cấp bộ",
                OrganizationLevel = Entities.Enums.OrganizationLevelEnum.CAP_BO
            }
        };
        await dbContext.Departments.AddRangeAsync(parent);
        await dbContext.SaveChangesAsync();
        var lelver1 = new List<Department>
        {
             new Department
            {
                ParentId = dbContext.Departments.FirstOrDefault(x => x.Name == "Khối cơ quan cấp bộ").Id,
                Name = "Khối cơ quan UBND cấp tỉnh",
                OrganizationLevel = Entities.Enums.OrganizationLevelEnum.CAP_TINH
            },
            new Department
            {
                ParentId = dbContext.Departments.FirstOrDefault(x => x.Name == "Khối cơ quan cấp bộ").Id,
                Name = "UBND tỉnh",
                OrganizationLevel = Entities.Enums.OrganizationLevelEnum.CAP_TINH
            },
            new Department
            {
                ParentId = dbContext.Departments.FirstOrDefault(x => x.Name == "Đảng uỷ cơ quan cấp bộ").Id,
                Name = "Tỉnh uỷ",
                OrganizationLevel = Entities.Enums.OrganizationLevelEnum.CAP_TINH
            },
        };

        await dbContext.Departments.AddRangeAsync(lelver1);
        await dbContext.SaveChangesAsync();

        var lelver2 = new List<Department>
        {
             new Department
            {
                ParentId = dbContext.Departments.FirstOrDefault(x => x.Name == "UBND tỉnh").Id,
                Name = "Sở kế hoạch đầu tư",
                OrganizationLevel = Entities.Enums.OrganizationLevelEnum.CAP_TINH
            },
            new Department
            {
                ParentId = dbContext.Departments.FirstOrDefault(x => x.Name == "UBND tỉnh").Id,
                Name = "Sở giáo dục",
                OrganizationLevel = Entities.Enums.OrganizationLevelEnum.CAP_TINH
            },
            new Department
            {
                ParentId = dbContext.Departments.FirstOrDefault(x => x.Name == "UBND tỉnh").Id,
                Name = "Sở văn hoá - thể thao - du lịch",
                OrganizationLevel = Entities.Enums.OrganizationLevelEnum.CAP_TINH
            },
            new Department
            {
                ParentId = dbContext.Departments.FirstOrDefault(x => x.Name == "Tỉnh uỷ").Id,
                Name = "Đảng uỷ sở giáo dục",
                OrganizationLevel = Entities.Enums.OrganizationLevelEnum.CAP_TINH
            },
        };
        await dbContext.Departments.AddRangeAsync(lelver2);
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedDataPosition()
    {
        if (await dbContext.Positions.AnyAsync())
        {
            return;
        }
        var lst = new List<Position>
        {
            new Position
            {
                Name = "Trưởng phòng"
            },
            new Position
            {
                Name = "Chánh văn phòng"
            },
            new Position
            {
                Name = "Nhân viên"
            },
            new Position
            {
                Name = "Phó phòng"
            },
             new Position
            {
                Name = "Thư ký"
            },
              new Position
            {
                Name = "Kiểm soát viên"
            }
        };
        await dbContext.Positions.AddRangeAsync(lst);
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedDataUser()
    {
        if (await dbContext.Users.AnyAsync())
        {
            return;
        }
        var lst = new List<User>
        {
            new User
            {
                Name = "Lê Trung Đức",
                PositionId = dbContext.Positions.FirstOrDefault(x => x.Name == "Trưởng phòng").Id,
                DepartmentId = dbContext.Departments.FirstOrDefault().Id,
                Email= "vansy9x@gmail.com",
                Phone = "0971489926",
                PasswordHash = "123456"
            },
            new User
            {
                Name = "Vũ Chu Đức",
                PositionId = dbContext.Positions.FirstOrDefault(x => x.Name == "Chánh văn phòng").Id,
                DepartmentId = dbContext.Departments.FirstOrDefault(x => x.Name == "UBND tỉnh").Id,
                Email= "vansy9x@gmail.com",
                Phone = "0971489926",
                PasswordHash = "123456"
            },
            new User
            {
                Name = "Nguyễn Lê Thanh",
                PositionId = dbContext.Positions.FirstOrDefault(x => x.Name == "Nhân viên").Id,
                DepartmentId = dbContext.Departments.FirstOrDefault(x => x.Name == "Tỉnh uỷ").Id,
                Email= "vansy9x@gmail.com",
                Phone = "0971489926",
                PasswordHash = "123456"
            },
            new User
            {
                Name = "Nguyễn Văn Thành",
                PositionId = dbContext.Positions.FirstOrDefault(x => x.Name == "Nhân viên").Id,
                DepartmentId = dbContext.Departments.FirstOrDefault(x => x.Name == "Sở kế hoạch đầu tư").Id,
                Email= "vansy9x@gmail.com",
                Phone = "0971489926",
                PasswordHash = "123456"
            },
            new User
            {
                Name = "Lê Trung Đức",
                PositionId = dbContext.Positions.FirstOrDefault(x => x.Name == "Thư ký").Id,
                DepartmentId = dbContext.Departments.FirstOrDefault(x => x.Name == "Sở văn hoá - thể thao - du lịch").Id,
                Email= "vansy9x@gmail.com",
                Phone = "0971489926",
                PasswordHash = "123456"
            },
            new User
            {
                Name = "Hoành Thanh Hải",
                PositionId = dbContext.Positions.FirstOrDefault(x => x.Name == "Phó phòng").Id,
                DepartmentId = dbContext.Departments.FirstOrDefault(x => x.Name == "Đảng uỷ sở giáo dục").Id,
                Email= "vansy9x@gmail.com",
                Phone = "0971489926",
                PasswordHash = "123456"
            },
            new User
            {
                Name = "Vũ HoàiNam",
                PositionId = dbContext.Positions.FirstOrDefault(x => x.Name == "Nhân viên").Id,
                DepartmentId = dbContext.Departments.FirstOrDefault().Id,
                Email= "vansy9x@gmail.com",
                Phone = "0971489926",
                PasswordHash = "123456"
            },
            new User
            {
                Name = "Nguyễn Huy Hùng",
                PositionId = dbContext.Positions.FirstOrDefault(x => x.Name == "Kiểm soát viên").Id,
                DepartmentId = dbContext.Departments.FirstOrDefault(x => x.Name == "Khối cơ quan cấp bộ").Id,
                Email= "vansy9x@gmail.com",
                Phone = "0971489926",
                PasswordHash = "123456"
            }
        };

        await dbContext.Users.AddRangeAsync(lst);       
        await dbContext.SaveChangesAsync();
    }
}
