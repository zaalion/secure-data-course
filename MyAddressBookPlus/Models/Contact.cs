namespace MyAddressBookPlus.Models
{
    /// <summary>
    /// Contact view model which is used in the controller and views
    /// </summary>
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PictureName { get; set; }
    }
}