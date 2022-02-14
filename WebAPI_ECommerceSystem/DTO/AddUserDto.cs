using System.Text.RegularExpressions;

namespace WebAPI_ECommerceSystem.DTO
{
    public class AddUserDto
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private string _phone;
        private string _mobile;
        private string _street;
        private string _postalCode;
        private string _city;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value.Trim(); }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value.Trim(); }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?").IsMatch(value.Trim()))
                {
                    _email = value.Trim();
                }
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (new Regex(@"^(?=.*?[A-Ö])(?=.*?[a-ö])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$").IsMatch(value.Trim()))
                {
                    Password = value.Trim();
                }
            }
        }
        public string Phone 
        {
            get { return _phone; }
            set 
            { 
                if(new Regex(@"\+?\d").IsMatch(value.Trim()))
                {
                    Phone = value.Replace(" ", "");
                }
            }
        }
        public string Mobile
        {
            get { return _mobile; }
            set 
            { 
                if(new Regex(@"\+?\d").IsMatch(value.Trim()))
                {
                    Mobile = value.Replace(" ", "");
                }
            }
        }
        public string Street
        {
            get { return _street; }
            set { _street = value.Trim(); }
        }
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                if (new Regex(@"/s?/d{3}/s?/d{2}$").IsMatch(value.Trim()))
                {
                    _postalCode = value.Replace(" ", "");
                }
            }
        }
        public string City
        {
            get { return _city; }
            set { _city = value.Trim(); }
        }
    }

}

