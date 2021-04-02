using Microsoft.WindowsAzure.Storage.Table;

namespace CustomerFunc.Azure.Storage.Table.Read.Client
{
    public class CustomerProfile : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerProfile"/> class.
        /// </summary>
        public CustomerProfile()
        {

        }
        /// <summary>
        /// The customer identifier
        /// </summary>
        public int customerID { get; set; }

        /// <summary>
        /// The first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// The last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The mobile number
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// The customer type
        /// </summary>
        public string CustomerType { get; set; }

        /// <summary>
        /// Assigns the row key.
        /// </summary>
        private void AssignRowKey()
        {
            this.RowKey = customerID.ToString();
        }

        /// <summary>
        /// Assigns the partition key.
        /// </summary>
        private void AssignPartitionKey()
        {
            this.PartitionKey = CustomerType;
        }
    }
}
