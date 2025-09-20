using System.Text.Json;
using System.Text.Json.Serialization;
using TAGCRM.Models;

namespace TAGCRM.Services
{
    public class ContactService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly SemaphoreSlim _ioLock = new(1, 1);

        private string ContactsPath => Path.Combine(_environment.WebRootPath, "data", "contacts.json");
        private string MembersPath => Path.Combine(_environment.WebRootPath, "data", "members.json");

        private static readonly JsonSerializerOptions ReadOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly JsonSerializerOptions WriteOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public ContactService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // ---------------------------
        // Helpers
        // ---------------------------
        private static string DisplayName(Contact c)
        {
            if (!string.IsNullOrWhiteSpace(c.PreferredName)) return c.PreferredName;
            var full = $"{c.FirstName} {c.Surname}".Trim();
            if (!string.IsNullOrWhiteSpace(full)) return full;
            return c.Name ?? string.Empty; // legacy fallback
        }

        private async Task EnsureDataFolderAsync()
        {
            var dir = Path.GetDirectoryName(ContactsPath)!;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                await Task.CompletedTask;
            }
        }

        private async Task SaveAllContactsAsync(List<Contact> contacts)
        {
            await EnsureDataFolderAsync();
            await _ioLock.WaitAsync();
            try
            {
                var json = JsonSerializer.Serialize(contacts, WriteOptions);
                await File.WriteAllTextAsync(ContactsPath, json);
            }
            finally
            {
                _ioLock.Release();
            }
        }

        // ---------------------------
        // Members (for business name join)
        // ---------------------------
        public async Task<List<Member>> GetAllMembersAsync()
        {
            try
            {
                if (!File.Exists(MembersPath)) return new List<Member>();
                var json = await File.ReadAllTextAsync(MembersPath);
                var members = JsonSerializer.Deserialize<List<Member>>(json, ReadOptions);
                return members ?? new List<Member>();
            }
            catch
            {
                return new List<Member>();
            }
        }

        // ---------------------------
        // Contacts
        // ---------------------------
        public async Task<List<Contact>> GetAllContactsAsync()
        {
            try
            {
                if (!File.Exists(ContactsPath)) return new List<Contact>();
                var json = await File.ReadAllTextAsync(ContactsPath);
                var contacts = JsonSerializer.Deserialize<List<Contact>>(json, ReadOptions) ?? new List<Contact>();
                return contacts;
            }
            catch
            {
                return new List<Contact>();
            }
        }

        public async Task<PagedResult<Contact>> GetContactsAsync(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            try
            {
                var allContacts = await GetAllContactsAsync();
                var allMembers = await GetAllMembersAsync();

                // Join: fill BusinessName from members file (don’t persist here)
                foreach (var c in allContacts)
                {
                    var member = allMembers.FirstOrDefault(m => m.Id.ToString() == c.MemberId);
                    c.BusinessName = member?.BusinessName ?? "Unknown Company";
                }

                var query = allContacts.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var s = searchTerm.Trim().ToLowerInvariant();
                    query = query.Where(c =>
                        (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.ToLower().Contains(s)) ||
                        (!string.IsNullOrEmpty(c.Surname) && c.Surname.ToLower().Contains(s)) ||
                        (!string.IsNullOrEmpty(c.PreferredName) && c.PreferredName.ToLower().Contains(s)) ||
                        (!string.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(s)) ||
                        (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(s)) ||
                        (!string.IsNullOrEmpty(c.Phone) && c.Phone.ToLower().Contains(s)) ||
                        (!string.IsNullOrEmpty(c.BusinessName) && c.BusinessName.ToLower().Contains(s)) ||
                        (!string.IsNullOrEmpty(c.MemberId) && c.MemberId.ToLower().Contains(s))
                    );
                }

                var total = query.Count();

                var pageData = query
                    .OrderBy(c => DisplayName(c))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PagedResult<Contact>
                {
                    Data = pageData,
                    TotalRecords = total,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }
            catch
            {
                return new PagedResult<Contact>
                {
                    Data = new List<Contact>(),
                    TotalRecords = 0,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }
        }

        public async Task<Contact?> GetContactByIdAsync(int id)
        {
            var contacts = await GetAllContactsAsync();
            var contact = contacts.FirstOrDefault(c => c.Id == id);

            if (contact != null)
            {
                var members = await GetAllMembersAsync();
                var member = members.FirstOrDefault(m => m.Id.ToString() == contact.MemberId);
                contact.BusinessName = member?.BusinessName ?? "Unknown Company";
            }

            return contact;
        }

        public async Task<List<SmsTemplate>> GetSmsTemplatesAsync()
        {
            try
            {
                var smsPath = Path.Combine(_environment.WebRootPath, "data", "sms-templates.json");
                if (!File.Exists(smsPath)) return new List<SmsTemplate>();

                var json = await File.ReadAllTextAsync(smsPath);
                var templates = JsonSerializer.Deserialize<List<SmsTemplate>>(json, ReadOptions);
                return templates ?? new List<SmsTemplate>();
            }
            catch
            {
                return new List<SmsTemplate>();
            }
        }

        public async Task<List<EmailTemplate>> GetEmailTemplatesAsync()
        {
            try
            {
                var emailPath = Path.Combine(_environment.WebRootPath, "data", "email-templates.json");
                if (!File.Exists(emailPath)) return new List<EmailTemplate>();

                var json = await File.ReadAllTextAsync(emailPath);
                var templates = JsonSerializer.Deserialize<List<EmailTemplate>>(json, ReadOptions);
                return templates ?? new List<EmailTemplate>();
            }
            catch
            {
                return new List<EmailTemplate>();
            }
        }

        public async Task<List<ContactNote>> GetContactNotesAsync(int contactId)
        {
            try
            {
                var notesPath = Path.Combine(_environment.WebRootPath, "data", "contact-notes.json");
                if (!File.Exists(notesPath)) return new List<ContactNote>();

                var json = await File.ReadAllTextAsync(notesPath);
                var allNotes = JsonSerializer.Deserialize<List<ContactNote>>(json, ReadOptions) ?? new List<ContactNote>();

                return allNotes
                    .Where(n => n.ContactId == contactId)
                    .OrderByDescending(n => n.Timestamp)
                    .ToList();
            }
            catch
            {
                return new List<ContactNote>();
            }
        }

        public async Task<List<MemberAlert>> GetMemberAlertsAsync(int memberId)
        {
            try
            {
                var alertsFilePath = Path.Combine(_environment.WebRootPath, "data", "alerts.json");
                if (!File.Exists(alertsFilePath)) return new List<MemberAlert>();

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
                if (!File.Exists(alertTypesFilePath)) return new List<AlertType>();

                var jsonContent = await File.ReadAllTextAsync(alertTypesFilePath);
                return JsonSerializer.Deserialize<List<AlertType>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<AlertType>();
            }
            catch
            {
                return new List<AlertType>();
            }
        }

    }
}
