using ServiceContracts;

namespace Service
{
    public class PersonService : IPersonService
    {
        public string AddUser()
        {
            return "Added User Successfully...";
        }
        public string UpdateUser()
        {
            return "Update User Successfully...";
        }
        public string DeleteUser()
        {
            return "Delete User Successfully...";
        }
    }
}
