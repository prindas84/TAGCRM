using System.Text.Json;
using TAGCRM.Models;

namespace TAGCRM.Services
{
    public class MemberService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _filePath;

        public MemberService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _filePath = Path.Combine(_environment.WebRootPath, "data", "members.json");
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<Member>();
                }

                var jsonContent = await File.ReadAllTextAsync(_filePath);

                var members = JsonSerializer.Deserialize<List<Member>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return members ?? new List<Member>();
            }
            catch
            {
                return new List<Member>();
            }
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            try
            {
                var members = await GetAllMembersAsync();
                return members.FirstOrDefault(m => m.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<(List<Member> Data, int TotalRecords)> GetMembersAsync(int page, int pageSize, string searchTerm = "")
        {
            try
            {
                var allMembers = await GetAllMembersAsync();

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    allMembers = allMembers.Where(m =>
                        m.BusinessName?.ToLower().Contains(searchTerm) == true ||
                        m.Email?.ToLower().Contains(searchTerm) == true ||
                        m.Phone?.ToLower().Contains(searchTerm) == true ||
                        m.MemberId?.ToLower().Contains(searchTerm) == true
                    ).ToList();
                }

                var totalRecords = allMembers.Count;

                // Apply pagination
                var pagedMembers = allMembers
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (pagedMembers, totalRecords);
            }
            catch
            {
                return (new List<Member>(), 0);
            }
        }

        public async Task<bool> SaveMemberAsync(Member member)
        {
            try
            {
                var members = await GetAllMembersAsync();

                if (member.Id > 0)
                {
                    // Update existing
                    var existing = members.FirstOrDefault(m => m.Id == member.Id);
                    if (existing != null)
                    {
                        members.Remove(existing);
                        members.Add(member);
                    }
                }
                else
                {
                    // New member → assign next ID
                    var nextId = members.Any() ? members.Max(m => m.Id) + 1 : 1;
                    member.Id = nextId;
                    members.Add(member);
                }

                // Write JSON back to disk
                var json = JsonSerializer.Serialize(members.OrderBy(m => m.Id).ToList(),
                    new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, json);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<StaffMember>> GetStaffMembersAsync()
        {
            try
            {
                var staffFilePath = Path.Combine(_environment.WebRootPath, "data", "staff.json");

                if (!File.Exists(staffFilePath))
                {
                    return new List<StaffMember>();
                }

                var jsonContent = await File.ReadAllTextAsync(staffFilePath);

                var staff = JsonSerializer.Deserialize<List<StaffMember>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return staff ?? new List<StaffMember>();
            }
            catch
            {
                return new List<StaffMember>();
            }
        }

        public async Task<List<LegalStructure>> GetLegalStructuresAsync()
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "data", "legal.json");

                if (!File.Exists(filePath))
                {
                    return new List<LegalStructure>();
                }

                var jsonContent = await File.ReadAllTextAsync(filePath);

                var legalStructures = JsonSerializer.Deserialize<List<LegalStructure>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return legalStructures ?? new List<LegalStructure>();
            }
            catch
            {
                return new List<LegalStructure>();
            }
        }

        public async Task<List<Organisation>> GetOrganisationsAsync()
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "data", "organisations.json");

                if (!File.Exists(filePath))
                {
                    return new List<Organisation>();
                }

                var jsonContent = await File.ReadAllTextAsync(filePath);

                var organisations = JsonSerializer.Deserialize<List<Organisation>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return organisations ?? new List<Organisation>();
            }
            catch
            {
                return new List<Organisation>();
            }
        }

        public async Task<List<EstimatingPackage>> GetEstimatingPackagesAsync()
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "data", "estimating.json");

                if (!File.Exists(filePath))
                {
                    return new List<EstimatingPackage>();
                }

                var jsonContent = await File.ReadAllTextAsync(filePath);

                var estimatingPackages = JsonSerializer.Deserialize<List<EstimatingPackage>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return estimatingPackages ?? new List<EstimatingPackage>();
            }
            catch
            {
                return new List<EstimatingPackage>();
            }
        }

        public async Task<List<Region>> GetRegionsAsync()
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "data", "regions.json");

                if (!File.Exists(filePath))
                {
                    return new List<Region>();
                }

                var jsonContent = await File.ReadAllTextAsync(filePath);

                var regions = JsonSerializer.Deserialize<List<Region>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return regions ?? new List<Region>();
            }
            catch
            {
                return new List<Region>();
            }
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            try
            {
                var contactsFilePath = Path.Combine(_environment.WebRootPath, "data", "contacts.json");

                if (!File.Exists(contactsFilePath))
                {
                    return new List<Contact>();
                }

                var jsonContent = await File.ReadAllTextAsync(contactsFilePath);

                var contacts = JsonSerializer.Deserialize<List<Contact>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return contacts ?? new List<Contact>();
            }
            catch
            {
                return new List<Contact>();
            }
        }

        public async Task<List<MemberNote>> GetMemberNotesAsync(int memberId)
        {
            try
            {
                var notesFilePath = Path.Combine(_environment.WebRootPath, "data", "member-notes.json");

                if (!File.Exists(notesFilePath))
                {
                    return new List<MemberNote>();
                }

                var jsonContent = await File.ReadAllTextAsync(notesFilePath);
                var allNotes = JsonSerializer.Deserialize<List<MemberNote>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<MemberNote>();

                return allNotes.Where(n => n.MemberId == memberId).ToList();
            }
            catch
            {
                return new List<MemberNote>();
            }
        }

        public async Task<List<MemberAlert>> GetMemberAlertsAsync(int memberId)
        {
            try
            {
                var alertsFilePath = Path.Combine(_environment.WebRootPath, "data", "alerts.json");

                if (!File.Exists(alertsFilePath))
                {
                    return new List<MemberAlert>();
                }

                var jsonContent = await File.ReadAllTextAsync(alertsFilePath);
                var allAlerts = JsonSerializer.Deserialize<List<MemberAlert>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<MemberAlert>();

                return allAlerts.Where(a => a.MemberId == memberId).ToList();
            }
            catch
            {
                return new List<MemberAlert>();
            }
        }

        public async Task<List<AlertType>> GetAlertTypesAsync()
        {
            try
            {
                var alertTypesFilePath = Path.Combine(_environment.WebRootPath, "data", "alert-type.json");

                if (!File.Exists(alertTypesFilePath))
                {
                    return new List<AlertType>();
                }

                var jsonContent = await File.ReadAllTextAsync(alertTypesFilePath);

                var alertTypes = JsonSerializer.Deserialize<List<AlertType>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return alertTypes ?? new List<AlertType>();
            }
            catch
            {
                return new List<AlertType>();
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "data", "category.json");

                if (!File.Exists(filePath))
                {
                    return new List<Category>();
                }

                var jsonContent = await File.ReadAllTextAsync(filePath);

                var categories = JsonSerializer.Deserialize<List<Category>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return categories ?? new List<Category>();
            }
            catch
            {
                return new List<Category>();
            }
        }

        public async Task<List<Class>> GetClassesAsync()
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "data", "class.json");

                if (!File.Exists(filePath))
                {
                    return new List<Class>();
                }

                var jsonContent = await File.ReadAllTextAsync(filePath);

                var classes = JsonSerializer.Deserialize<List<Class>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return classes ?? new List<Class>();
            }
            catch
            {
                return new List<Class>();
            }
        }

        public async Task<List<PaymentMethod>> GetPaymentMethodsAsync()
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "data", "payment-method.json");

                if (!File.Exists(filePath))
                {
                    return new List<PaymentMethod>();
                }

                var jsonContent = await File.ReadAllTextAsync(filePath);

                var paymentMethods = JsonSerializer.Deserialize<List<PaymentMethod>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return paymentMethods ?? new List<PaymentMethod>();
            }
            catch
            {
                return new List<PaymentMethod>();
            }
        }



    }
}
