namespace TheCollection.Pages
{
    public class PaymentService
    {

        public bool ProcessPayment(string creditCardNumber, decimal amount)
        {

            return !string.IsNullOrEmpty(creditCardNumber);
        }
    }
}
